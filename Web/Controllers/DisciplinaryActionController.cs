using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
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
            var query = await _disciplinaryActionService.GetByTargetEmployee(authData.Id);

            disciplinaryActionViewModel.DisciplinaryActions = _mapper.Map<IEnumerable<DisciplinaryActionModel>>(query);

            return View(disciplinaryActionViewModel);
        }
    }
}