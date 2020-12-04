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
    public class LeaveController : BaseController
    {
        private readonly IGradeLevelService _gradeLevelService;
        private readonly ILeaveService _leaveService;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public LeaveController(IGradeLevelService gradeLevelService, ILeaveService leaveService, ILeaveTypeService leaveTypeService, IEmployeeService employeeService, IMapper mapper)
        {
            _leaveService = leaveService;
            _gradeLevelService = gradeLevelService;
            _leaveTypeService = leaveTypeService;
            _employeeService = employeeService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            var authData = JwtHelper.GetAuthData(Request);
            if (authData == null)
            {
                return RedirectToAction("Signout", "Employee");
            }

            LeaveViewModel leaveViewModel = new LeaveViewModel();
            var leaveType = await _leaveTypeService.GetAll();
            var employee = await _employeeService.GetByEmployerIdOrEmail(authData.Emp_No);
            leaveViewModel.LeaveType = _mapper.Map<IEnumerable<LeaveTypeModel>>(leaveType);
            leaveViewModel.Employee = _mapper.Map<EmployeeModel>(employee);

            return View(leaveViewModel);
        }

        [Route("ApplyLeave")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ApplyLeave(Guid leaveTypeId, DateTime dateFrom, DateTime dateTo, DateTime resumptionDate )
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