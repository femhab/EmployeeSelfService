using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Enumeration;
using ViewModel.Model;
using Web.Helper.JWT;

namespace Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly IDepartmentService _departmentService;
        private readonly IApprovalWorkItemService _approvalWorkItemService;

        public AdminController(IEmployeeService employeeService, IRoleService roleService, IMapper mapper, IDepartmentService departmentService, IApprovalWorkItemService approvalWorkItemService)
        {
            _employeeService = employeeService;
            _roleService = roleService;
            _departmentService = departmentService;
            _approvalWorkItemService = approvalWorkItemService;
            _mapper = mapper;
        }

        [Route("AddAdmin")]
        public async Task<ActionResult> AddAdmin()
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                AdminViewModel adminViewModel = new AdminViewModel();
                var employeeList = await _employeeService.GetAll();

                adminViewModel.EmployeeList = _mapper.Map<IEnumerable<EmployeeModel>>(employeeList);
                return View(adminViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [Route("AddRole")]
        public async Task<ActionResult> AddRole()
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                AdminViewModel adminViewModel = new AdminViewModel();
                var roleList = await _roleService.GetAll();

                adminViewModel.Roles = _mapper.Map<IEnumerable<RoleModel>>(roleList);
                return View(adminViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [Route("AddWorkItem")]
        public async Task<ActionResult> AddWorkItem()
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                AdminViewModel adminViewModel = new AdminViewModel();
                var workItemList = await _approvalWorkItemService.GetAll();

                adminViewModel.ApprovalWorkItem = _mapper.Map<IEnumerable<ApprovalWorkItemModel>>(workItemList);
                return View(adminViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        public async Task<ActionResult> ManageDepartment()
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                AdminViewModel adminViewModel = new AdminViewModel();
                var departmentList = await _departmentService.GetAll();
                var clearingDepartment = await _departmentService.GetByExitApproval();

                adminViewModel.DepartmentList = _mapper.Map<IEnumerable<DepartmentModel>>(departmentList);
                adminViewModel.ClearingDepartment = _mapper.Map<IEnumerable<DepartmentModel>>(clearingDepartment);
                return View(adminViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        public IActionResult SetApprovalPeriod()
        {
            return View();
        }

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ManageAdmin(Guid employeeId, AccessTypeEnum accessType)
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

                    var updateResponse = await _employeeService.UpdateAccessType(employeeId, _mapper.Map<AccessType>(accessType));
                    
                    return Json(new
                    {
                        status = updateResponse.Status,
                        message = updateResponse.Message
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
        public async Task<ActionResult> ManageRole(string name)
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

                    Role role = new Role()
                    {
                        Description = name,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now
                    };
                    var response = await _roleService.Create(role);
                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message
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
        public async Task<ActionResult> ManageWorkItem(string name)
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

                    ApprovalWorkItem workItem = new ApprovalWorkItem()
                    {
                        Name = name,
                        Description = name + " Service",
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now
                    };
                    var response = await _approvalWorkItemService.Create(workItem);
                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message
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
        public async Task<ActionResult> DeleteRole(Guid roleId)
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

                    var response = await _roleService.Delete(roleId);
                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message
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
        public async Task<ActionResult> DeleteWorkItem(Guid workItemId)
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

                    var response = await _approvalWorkItemService.Delete(workItemId);
                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message
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
        public async Task<ActionResult> ManageDepartmentAuth(Guid departmentId, bool canClear)
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

                    var response = await _departmentService.ChangeClearanceRole(departmentId, canClear);
                    return Json(new
                    {
                       status = response.Status,
                       message = response.Message
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
    }
}