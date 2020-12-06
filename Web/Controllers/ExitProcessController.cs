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
    public class ExitProcessController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IExitProcessService _exitProcessService;
        private readonly IExitProcessPriorityItemService _exitProcessPriorityItemService;
        private readonly IMapper _mapper;

        public ExitProcessController(IEmployeeService employeeService, IExitProcessService exitProcessService, IExitProcessPriorityItemService exitProcessPriorityItemService, IMapper mapper)
        {
            _employeeService = employeeService;
            _exitProcessService = exitProcessService;
            _exitProcessPriorityItemService = exitProcessPriorityItemService;
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
            var exitApplication =await _exitProcessService.GetUnapprovedApplication();

            exitProcessViewModel.ExitProcessList = _mapper.Map<IEnumerable<ExitProcessModel>>(exitApplication);

            return View(exitProcessViewModel);
        }
    }
}