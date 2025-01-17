﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class ExitProcessController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IExitProcessService _exitProcessService;
        private readonly IExitProcessPriorityItemService _exitProcessPriorityItemService;
        private readonly IDepartmentService _departmentService;
        private readonly IUserRoleService _userRoleService;
        private readonly IMapper _mapper;

        public ExitProcessController(IEmployeeService employeeService, IExitProcessService exitProcessService, IExitProcessPriorityItemService exitProcessPriorityItemService, IMapper mapper, IDepartmentService departmentService, IUserRoleService userRoleService)
        {
            _employeeService = employeeService;
            _exitProcessService = exitProcessService;
            _exitProcessPriorityItemService = exitProcessPriorityItemService;
            _departmentService = departmentService;
            _userRoleService = userRoleService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var authData = JwtHelper.GetAuthData(Request);
            if (authData == null)
            {
                return RedirectToAction("Signout", "Employee");
            }

            ExitProcessViewModel exitProcessViewModel = new ExitProcessViewModel();
            var employee = await _employeeService.GetByEmployerIdOrEmail(authData.Emp_No);

            exitProcessViewModel = _mapper.Map<ExitProcessViewModel>(authData);
            exitProcessViewModel.Employee = _mapper.Map<EmployeeModel>(employee);

            return View(exitProcessViewModel);
        }

        public async Task<ActionResult> ApproveRequest()
        {
            var authData = JwtHelper.GetAuthData(Request);
            if (authData == null)
            {
                return RedirectToAction("Signout", "Employee");
            }

            ExitProcessViewModel exitProcessViewModel = new ExitProcessViewModel();
            var exitApplication = (await _exitProcessService.GetUnapprovedApplication()).Where(x => x.Status == ExitProcessStatus.Pending);
            var clearingDepartment = (await _departmentService.GetByExitApproval()).ToList();

            var userRole = await _userRoleService.GetByClearanceRole(authData.Id);
            exitProcessViewModel = _mapper.Map<ExitProcessViewModel>(authData);
            var getHod = await _employeeService.GetHOD(authData.Id);

            if (userRole != null || getHod != null)
            {
                exitProcessViewModel.ExitProcessList = _mapper.Map<IEnumerable<ExitProcessModel>>(exitApplication);

                foreach (var item in exitApplication)
                {
                    clearingDepartment.Add(item.Employee.Department);
                }
            }
            
            exitProcessViewModel.ClearanceDepartment = _mapper.Map<List<DepartmentModel>>(clearingDepartment);
           
            return View(exitProcessViewModel);
        }

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RequestExit(string exitDate, string reason)
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

                    var exitTime = DateTime.ParseExact(exitDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var exitProcess = new ExitProcess()
                    {
                        Emp_No = authData.Emp_No,
                        EmployeeId = authData.Id,
                        ExitDate = exitTime,
                        NoticeDate = DateTime.Now,
                        Reason = reason,
                        Status = ExitProcessStatus.Pending,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now
                    };

                    var response = await _exitProcessService.Create(exitProcess);

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

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ApproveRequestExit(Guid exitId, Guid departmentId, string exitComment)
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

                    var response = await _exitProcessPriorityItemService.Create(exitId, departmentId,authData.Emp_No,exitComment);

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
        public async Task<ActionResult> GetExitById(Guid exitId)
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
                    var response = await _exitProcessService.GetById(exitId);

                    return Json(new
                    {
                        status = response.Status,
                        data = new { reason = response.Reason, noticeDate= response.NoticeDate.ToString("dddd, dd MMMM yyyy"), exitDate = response.ExitDate.ToString("dddd, dd MMMM yyyy"), firstName = response.Employee.FirstName, lastName = response.Employee.LastName, empNo = response.Emp_No }
                    }) ;
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