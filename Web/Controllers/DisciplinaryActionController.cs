using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViewModel.Model;
using Web.Helper.JWT;

namespace Web.Controllers
{
    public class DisciplinaryActionController : BaseController
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDisciplinaryActionService _disciplinaryActionService;
        private readonly IMapper _mapper;

        public DisciplinaryActionController(IEmployeeService employeeService, IDisciplinaryActionService disciplinaryActionService, IMapper mapper)
        {
            _employeeService = employeeService;
            _disciplinaryActionService = disciplinaryActionService;
            _mapper = mapper;
        }

        public async Task<ActionResult> IssueQuery()
        {
            var authData = JwtHelper.GetAuthData(Request);
            if (authData == null)
            {
                return RedirectToAction("Signout", "Employee");
            }

            DisciplinaryActionViewModel disciplinaryActionViewModel = new DisciplinaryActionViewModel();
            var lowGradeStaff = await _employeeService.GetAllLowGradeEmployee(authData.Id);

            disciplinaryActionViewModel.Employee = _mapper.Map<IEnumerable<EmployeeModel>>(lowGradeStaff);

            return View(disciplinaryActionViewModel);
        }

        public async Task<ActionResult> ViewQuery()
        {
            var authData = JwtHelper.GetAuthData(Request);
            if (authData == null)
            {
                return RedirectToAction("Signout", "Employee");
            }

            DisciplinaryActionViewModel disciplinaryActionViewModel = new DisciplinaryActionViewModel();
            var query = await _disciplinaryActionService.GetByTargetEmployee(authData.Id);

            disciplinaryActionViewModel.DisciplinaryActions = _mapper.Map<IEnumerable<DisciplinaryActionModel>>(query);

            return View(disciplinaryActionViewModel);
        }

        public async Task<ActionResult> QueryAction()
        {
            var authData = JwtHelper.GetAuthData(Request);
            if (authData == null)
            {
                return RedirectToAction("Signout", "Employee");
            }

            DisciplinaryActionViewModel disciplinaryActionViewModel = new DisciplinaryActionViewModel();
            var query = await _disciplinaryActionService.GetByEmployee(authData.Id);

            disciplinaryActionViewModel.DisciplinaryActions = _mapper.Map<IEnumerable<DisciplinaryActionModel>>(query);

            return View(disciplinaryActionViewModel);
        }

        //action section
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> AddQuery(Guid queriedId, string queriedEmpNo, string subject, string message)
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

                    var response = await _disciplinaryActionService.CreateQuery(authData.Id, authData.Emp_No, subject, message, queriedId, queriedEmpNo);

                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message ?? "Oop! please try again later."
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
        public async Task<ActionResult> ReplyQuery(Guid id, string reply)
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

                    var response = await _disciplinaryActionService.ReplyQuery(id, reply);

                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message ?? "Oop! please try again later."
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
        public async Task<ActionResult> TakeQueryAction(Guid id, string comment, QueryAction action)
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

                    var response = await _disciplinaryActionService.GiveAction(id, comment, action);

                    return Json(new
                    {
                        status = response.Status,
                        message = response.Message ?? "Oop! please try again later."
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