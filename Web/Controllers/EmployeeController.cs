using AutoMapper;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.Model;
using Web.Helper.JWT;

namespace Web.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IApprovalWorkItemService _approvalWorkItemService;
        private readonly IAuthService _authService;
        private readonly IDepartmentService _departmentService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, IRoleService roleService, IDepartmentService departmentService, IAuthService authService, IApprovalWorkItemService approvalWorkItemService)
        {
            _employeeService = employeeService;
            _approvalWorkItemService = approvalWorkItemService;
            _authService = authService;
            _departmentService = departmentService;
            _roleService = roleService;
            _mapper = mapper;
        }

        //route section
        public IActionResult Index()
        {
            return View();
        }

        [Route("Employee")]
        public async Task<ActionResult> Enroll()
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                EmployeeViewModel employeeViewModel = new EmployeeViewModel();
                var deptList = await _departmentService.GetAll();
                var roleList = await _roleService.GetAll();
                var employeeList = await _employeeService.GetAll();
                var employeeListResponse = await _employeeService.GetUnregisteredBaseEmployee();
                employeeViewModel.Employeee = _mapper.Map<IEnumerable<HRUserModel>>(employeeListResponse);
                employeeViewModel.Roles = _mapper.Map<IEnumerable<RoleModel>>(roleList);
                employeeViewModel.Departments = _mapper.Map<IEnumerable<DepartmentModel>>(deptList);
                employeeViewModel.EmployeeList = _mapper.Map<IEnumerable<EmployeeModel>>(employeeList);

                return View(employeeViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        //[Route("Profile")]
        public async Task<ActionResult> Profile(string id)
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }
                if(id.ToLower() == "default")
                {
                    id = authData.Id.ToString();
                }

                EmployeeProfileViewModel profileViewModel = new EmployeeProfileViewModel();
                var employee = await _employeeService.GetById(Guid.Parse(id));
                var approvalWorkItem = await _approvalWorkItemService.GetAll();
                profileViewModel.Employee = _mapper.Map<EmployeeModel>(employee);
                profileViewModel.ApprovalWorkItem = _mapper.Map<IEnumerable<ApprovalWorkItemModel>>(approvalWorkItem);
                return View(profileViewModel);
            }
            catch(Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [Route("Transfer")]
        public IActionResult Transfer()
        {
            return View();
        }

        //action section
        [Route("Register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(string email, string password, string confirmPassword, string firstName, string lastName, string userName, string empNo, int roleId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var authData = JwtHelper.GetAuthData(Request);
                    if (authData == null)
                    {
                        return RedirectToAction("Signout", "Employee");
                    }

                    if (password.ToLower() == confirmPassword.ToLower())
                    {
                        var authRegister = await _authService.Register(email, password, lastName, firstName, userName, empNo, roleId); //, authData.Emp_No
                        if (authRegister.status)
                        {
                            //await _emailHelper.SendQuickMail(model.Email, "Registration Successful", $"Congratulations {model.FirstName} <br/>You are onbaorded successfully on Employee Self Service Platform. Your temporary password is {model.Password}, Please visit here ... to reset your password.<br/> If this is a wrong notification, please reach out on the hotline. <br/>Regards");
                            //return RedirectToAction("Index", "Dashboard");
                            return Json(new
                            {
                                authRegister.status,
                                authRegister.message
                            });
                        }
                    }                      
                    return Json(new
                    {
                        status = false,
                        message = "Please check the password"
                    });
                }
                return Json(new
                {
                    status = false,
                    message = "Error with Current Request"
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        public async Task<IActionResult> Signout()
        {
            try
            {
                var result = await _authService.Logout();

                return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult> CountProcessor(int approvalCount)
        {
            try
            {
                return Json(new
                {
                    status = true,
                    message = "Processing",
                    data = approvalCount
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }
    }
}