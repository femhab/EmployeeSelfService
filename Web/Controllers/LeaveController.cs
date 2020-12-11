using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            var leaveTaken = await _leaveService.GetByEmployee(authData.Id);

            leaveViewModel = _mapper.Map<LeaveViewModel>(authData);
            leaveViewModel.LeaveType = _mapper.Map<IEnumerable<LeaveTypeModel>>(leaveType);
            leaveViewModel.Employee = _mapper.Map<EmployeeModel>(employee);
            leaveViewModel.LeaveTaken = _mapper.Map<IEnumerable<LeaveModel>>(leaveTaken);

            return View(leaveViewModel);
        }

        [Route("ApplyLeave")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ApplyLeave(Guid leaveTypeId, string dateFrom, string dateTo, string resumptionDate, int noOfDays, bool isAllowanceAdded )
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

                    var startDate = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var endDate = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var resumeDate = DateTime.ParseExact(resumptionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var model = new Leave()
                    {
                        EmployeeId = authData.Id,
                        Emp_No = authData.Emp_No,
                        DateFrom = startDate,
                        DateTo = endDate,
                        LeaveTypeId = leaveTypeId,
                        IsAllowanceRequested = isAllowanceAdded,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now,
                        NoOfDays = noOfDays,
                        DaysUsed = 0,
                        ResumptionDate = resumeDate,
                        LeaveStatus = LeaveStatus.Pending,
                        Status = ApprovalStatus.Pending,
                        LastProccessedBy = authData.Emp_No
                    };

                    var response = await _leaveService.Create(model);
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