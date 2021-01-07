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
    public class PayrollController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IPaymentAdvanceService _paymentAdvanceService;
        private readonly IPayslipService _payslipService;
        private readonly IEmployeeService _employeeService;

        public PayrollController(IMapper mapper, IPaymentAdvanceService paymentAdvanceService, IPayslipService payslipService, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _paymentAdvanceService = paymentAdvanceService;
            _payslipService = payslipService;
            _employeeService = employeeService;
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
                //await _payslipService.GeneratePayslipData();

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

        public async Task<ActionResult> GeneratePayslip()
        {
            try
            {
                var authData = JwtHelper.GetAuthData(Request);
                if (authData == null)
                {
                    return RedirectToAction("Signout", "Employee");
                }

                PayrollViewModel payrollViewModel = new PayrollViewModel();
                var payslipData = await _payslipService.GeneratePayslipData(authData.Id);
                var employee = await _employeeService.GetById(authData.Id);

                payrollViewModel = _mapper.Map<PayrollViewModel>(authData);
                payrollViewModel.PayslipResponse = _mapper.Map<PayslipResponseModel>(payslipData);
                payrollViewModel.Employee = _mapper.Map<EmployeeModel>(employee);

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

                    var targetDate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    var model = new PaymentAdvance()
                    {
                        EmployeeId = authData.Id,
                        Emp_No = authData.Emp_No,
                        Amount = amount,
                        TargetDate = targetDate,
                        Status = ApprovalStatus.Pending,
                        Id = Guid.NewGuid(),
                        CreatedDate = DateTime.Now
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
