using AutoMapper;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.Model;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;
using Web.Helper.JWT;

namespace Web.Controllers
{
    public class PayrollController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IPaymentAdvanceService _paymentAdvanceService;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IPayslipService _payslipService;
        private readonly IEmployeeService _employeeService;

        public PayrollController(IMapper mapper, IPaymentAdvanceService paymentAdvanceService, IPayslipService payslipService, IEmployeeService employeeService, IEmployeeApprovalConfigService employeeApprovalConfigService, IApprovalBoardService approvalBoardService)
        {
            _mapper = mapper;
            _paymentAdvanceService = paymentAdvanceService;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _payslipService = payslipService;
            _employeeService = employeeService;
            _approvalBoardService = approvalBoardService;
        }

        public async Task<ActionResult> PaymentAdvance()
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                PayrollViewModel payrollViewModel = new PayrollViewModel();
                var paymentAdvance = await _paymentAdvanceService.GetByEmployee(authData.Id);
                var eligibility = await _paymentAdvanceService.CheckEligibility(authData.Emp_No);
                
                payrollViewModel = _mapper.Map<PayrollViewModel>(authData);
                payrollViewModel.Eligibility = _mapper.Map<PaymentAdvanceResponseModel>(eligibility);
                payrollViewModel.PaymentAdvance = _mapper.Map<IEnumerable<PaymentAdvanceModel>>(paymentAdvance);

                return View(payrollViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        public async Task<ActionResult> GeneratePayslip(DateTime? payPeriod = null )
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                PayrollViewModel payrollViewModel = new PayrollViewModel();
                var calender = await _payslipService.PayrollCalender();
                var lastCalender = calender.OrderByDescending(c => c.PayPeriod).FirstOrDefault();
                var payslipData = await _payslipService.GeneratePayslipData(authData.Id, (payPeriod != null) ? payPeriod.Value : lastCalender.PayPeriod);
                var employee = await _employeeService.GetById(authData.Id);

                payrollViewModel = _mapper.Map<PayrollViewModel>(authData);
                payrollViewModel.PayslipResponse = _mapper.Map<PayslipResponseModel>(payslipData);
                payrollViewModel.Employee = _mapper.Map<EmployeeModel>(employee);
                payrollViewModel.PayrollCalender = _mapper.Map<List<PayrollCalender>>(calender);

                return View(payrollViewModel);
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> RequestPaymentAdvance(decimal amount, string date)
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

                    var approverConfig = await _employeeApprovalConfigService.GetByServiceLevel(authData.Id, "payment advance", Level.FirstLevel);

                    if(approverConfig  != null)
                    {
                        var targetDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        var model = new PaymentAdvance()
                        {
                            EmployeeId = authData.Id,
                            Emp_No = authData.Emp_No,
                            Amount = amount,
                            TargetDate = targetDate,
                            Status = ApprovalStatus.Pending,
                            Id = Guid.NewGuid(),
                            CreatedDate = DateTime.Now,
                            LastProcessor = approverConfig.Processor
                        };

                        var response = await _paymentAdvanceService.Create(model);

                        return Json(new
                        {
                            status = response.Status,
                            message = response.Message
                        });
                    }

                    return Json(new
                    {
                        status = false,
                        message = "You don't have approval configuration for payment advance yet, reach out to the admin."
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
        public async Task<ActionResult> GetAdvanceById(Guid advanceId)
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

                    var board = await _approvalBoardService.GetById(advanceId);

                    var response = await _paymentAdvanceService.GetById(board.ServiceId);
                    return Json(new
                    {
                        status = (response != null) ? true : false,
                        data = new { targetDate = response.TargetDate.ToString("dddd, dd MMMM yyyy"), amount = response.Amount, serviceId = response.Id, level = board.ApprovalLevel }
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

        public async Task<ActionResult> GeneratePreviousMonthsPayslip(DateTime payPeriod)
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                var payslipData = await _payslipService.GeneratePayslipData(authData.Id, payPeriod);

                return Json(new
                {
                    status = true,
                    data = payslipData
                });
            }
            catch (Exception ex)
            {
                return ErrorPage(ex);
            }
        }
    }
}
