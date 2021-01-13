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
using ViewModel.ResponseModel;
using Web.Helper.JWT;

namespace Web.Controllers
{
    public class LeaveController : BaseController
    {
        private readonly IGradeLevelService _gradeLevelService;
        private readonly ILeaveService _leaveService;
        private readonly ILeaveRecallService _leaveRecallService;
        private readonly ILeaveTypeService _leaveTypeService;
        private readonly IEmployeeService _employeeService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IMapper _mapper;

        public LeaveController(IGradeLevelService gradeLevelService, ILeaveService leaveService, ILeaveTypeService leaveTypeService, IEmployeeService employeeService, IMapper mapper, ILeaveRecallService leaveRecallService, IApprovalBoardService approvalBoardService, IEmployeeApprovalConfigService employeeApprovalConfigService)
        {
            _leaveService = leaveService;
            _leaveRecallService = leaveRecallService;
            _gradeLevelService = gradeLevelService;
            _leaveTypeService = leaveTypeService;
            _employeeService = employeeService;
            _approvalBoardService = approvalBoardService;
            _employeeApprovalConfigService = employeeApprovalConfigService;
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
            var eligibility = await _leaveService.CheckEligibility(authData.Id);

            leaveViewModel = _mapper.Map<LeaveViewModel>(authData);
            leaveViewModel.LeaveType = _mapper.Map<IEnumerable<LeaveTypeModel>>(leaveType);
            leaveViewModel.Employee = _mapper.Map<EmployeeModel>(employee);
            leaveViewModel.LeaveTaken = _mapper.Map<IEnumerable<LeaveModel>>(leaveTaken);
            leaveViewModel.Eligiblity = _mapper.Map<LeaveResponseModel>(eligibility);


            return View(leaveViewModel);
        }

        [Route("ApplyLeave")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ApplyLeave(Guid leaveTypeId, string dateFrom, string dateTo, string resumptionDate, int noOfDays, bool isAllowanceAdded, int remainingDays)
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

                    if(noOfDays < 5 && isAllowanceAdded == true)
                    {
                        return Json(new
                        {
                            status = false,
                            message = "“You are not qualified for Leave Allowance.The total number of leave days is less than 5”"
                        });
                    }

                    var approverConfig = await _employeeApprovalConfigService.GetByServiceLevel(authData.Id, "leave", Level.FirstLevel);

                    if(approverConfig != null)
                    {
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
                            RemainingDays = remainingDays,
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
                        message = "You don't have approval configuration for leave yet, reach out to the admin."
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
        public async Task<ActionResult> ApplyRecallLeave(Guid leaveId, string recallDate, string dateFrom, string dateTo, string resumptionDate, int recallNoOfDays)
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

                    var recallStartDate = DateTime.ParseExact(recallDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var startDate = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var endDate = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var resumeDate = DateTime.ParseExact(resumptionDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var model = new LeaveRecall()
                    {
                        LeaveId = leaveId,
                        RecallDate = recallStartDate,
                        DateFrom = startDate,                      
                        DateTo = endDate,
                        NoOfDays = recallNoOfDays,
                        ResumptionDate = resumeDate,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now
                    };

                    var response = await _leaveRecallService.Create(model);
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
        public async Task<ActionResult> getLeaveById(Guid leaveId)
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

                    var board = await _approvalBoardService.GetById(leaveId);

                    var response = await _leaveService.GetById(board.ServiceId);
                    return Json(new
                    {
                        status = (response != null) ? true : false,
                        data = new { resumptionDate = response.ResumptionDate?.ToString("dddd, dd MMMM yyyy"), dateFrom = response.DateFrom.ToString("dddd, dd MMMM yyyy"), dateTo = response.DateTo.ToString("dddd, dd MMMM yyyy"), noOfDays = response.NoOfDays, leaveType = response.LeaveType, isAllowance = response.IsAllowanceRequested? "Yes":"No", serviceId = response.Id, level = board.ApprovalLevel }
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