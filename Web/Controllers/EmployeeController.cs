using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
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
        private readonly IEmployeeNOKDetailService _employeeNOKDetailService;
        private readonly IEmployeeAddressService _employeeAddressService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public EmployeeController(IEmployeeService employeeService, IMapper mapper, IRoleService roleService, IDepartmentService departmentService, IAuthService authService, IApprovalWorkItemService approvalWorkItemService, IEmployeeNOKDetailService employeeNOKDetailService, IEmployeeAddressService employeeAddressService)
        {
            _employeeService = employeeService;
            _approvalWorkItemService = approvalWorkItemService;
            _authService = authService;
            _departmentService = departmentService;
            _employeeNOKDetailService = employeeNOKDetailService;
            _employeeAddressService = employeeAddressService;
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

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddNextOfKin(string noKFirstName, string nokLastName, string nokPhonenumber, Guid nokRelationShipId, string nokEmail, string nokAddress, DateTime? nokDOB)
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

                    var nokDetail = new EmployeeNOKDetail()
                    {
                        Emp_No = authData.Emp_No,
                        EmployeeId = authData.Id,
                        FirstName = noKFirstName,
                        LastName = nokLastName,
                        PhoneNumber = nokPhonenumber,
                        RelationshipId = nokRelationShipId,
                        Email = nokEmail,
                        DOB = nokDOB,
                        Status = ApprovalStatus.Pending,
                        Address = nokAddress,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now
                    };

                    var response = await _employeeNOKDetailService.Create(nokDetail);
                   
                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message ?? "Failed to create Next of Kin Detail."
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

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddDependents(string depFirstName, string depLastName, string depPhonenumber, Guid depRelationShipId)
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



                    var nokDetail = await _employeeNOKDetailService.Create(null); //dependent service

                    return Json(new
                    {
                        status = nokDetail.Status,
                        message = nokDetail.Message ?? "Failed to create Next of Kin Detail."
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

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddEddress(string state, string city, string streetAddress, string country, string stateOfOrigin, string lgOfOrigin)
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

                    var addressDetail = new EmployeeAddress()
                    {
                        EmployeeId = authData.Id,
                        Emp_No = authData.Emp_No,
                        Country = country,
                        State = state,
                        City = city,
                        StreetAddress = streetAddress,
                        StateOfOrigin = stateOfOrigin,
                        LGOfOrigin = lgOfOrigin,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        Status = ApprovalStatus.Pending
                    };

                    var response = await _employeeAddressService.Create(addressDetail); //dependent service

                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message ?? "Failed to create Next of Kin Detail."
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