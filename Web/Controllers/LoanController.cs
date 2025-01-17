﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
    public class LoanController : BaseController
    {
        private readonly ILoanService _loanService;
        private readonly ILoanTypeService _loanTypeService;
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IMapper _mapper;

        public LoanController(ILoanService loanService, ILoanTypeService loanTypeService, IEmployeeService employeeService,  IMapper mapper, IEmployeeApprovalConfigService employeeApprovalConfigService, IApprovalBoardService approvalBoardService)
        {
            _loanService = loanService;
            _loanTypeService = loanTypeService;
            _employeeService = employeeService;
            _mapper = mapper;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _approvalBoardService = approvalBoardService;
        }

        public async Task<ActionResult> Index()
        {
            var authData = JwtHelper.GetAuthData(Request);
            if (authData == null)
            {
                return RedirectToAction("Signout", "Employee");
            }

            LoanViewModel loanViewModel = new LoanViewModel();
            var loanType = await _loanTypeService.GetAll();
            var employee = await _employeeService.GetByEmployerIdOrEmail(authData.Emp_No);
            var loanTaken = await _loanService.GetByEmployee(authData.Id);

            loanViewModel = _mapper.Map<LoanViewModel>(authData);
            loanViewModel.LoanType = _mapper.Map<IEnumerable<LoanTypeModel>>(loanType);
            loanViewModel.Employee = _mapper.Map<EmployeeModel>(employee);
            loanViewModel.LoanTaken = _mapper.Map<IEnumerable<LoanModel>>(loanTaken);

            return View(loanViewModel);
        }

        [Route("RequestLoan")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ApplyLoan(Guid loanTypeId, string dateFrom, string dateTo, decimal amountRequested, decimal interestRate, int noOfInstallment, string reason, decimal frequencyAmount)
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

                    var approverConfig = await _employeeApprovalConfigService.GetByServiceLevel(authData.Id, "loan", Level.FirstLevel);

                    if(approverConfig != null)
                    {
                        var startDate = DateTime.ParseExact(dateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        var endDate = DateTime.ParseExact(dateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        var model = new Loan()
                        {
                            EmployeeId = authData.Id,
                            Emp_No = authData.Emp_No,
                            StartDate = startDate,
                            EndDate = endDate,
                            LoanTypeId = loanTypeId,
                            Id = Guid.NewGuid(),
                            CreatedDate = DateTime.Now,
                            NoOfInstallment = noOfInstallment,
                            InstallmentAmount = frequencyAmount,
                            AmountRequested = amountRequested,
                            AmountApproved = 0,
                            InterestRate = interestRate,
                            LoanStatus = LoanStatus.Pending,
                            Status = ApprovalStatus.Pending,
                            LastApprover = authData.Emp_No,
                            Reason = reason
                        };

                        var response = await _loanService.Create(model);
                        return Json(new
                        {
                            status = response.Status,
                            message = response.Message
                        });
                    }

                    return Json(new
                    {
                        status = false,
                        message = "You don't have approval configuration for loan yet, reach out to the admin."
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
        public async Task<ActionResult> GetLoanById(Guid loanId)
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

                    var board = await _approvalBoardService.GetById(loanId);

                    var response = await _loanService.GetById(board.ServiceId);
                    return Json(new
                    {
                        status = (response != null) ? true : false,
                        data = new { dateFrom = response.StartDate.ToString("dddd, dd MMMM yyyy"), dateTo = response.EndDate.ToString("dddd, dd MMMM yyyy"), loanType = response.LoanType.Name, loanAmount = response.AmountRequested, noOfInstallment = response.NoOfInstallment, installmentAmount = response.InstallmentAmount, serviceId = response.Id, level = board.ApprovalLevel }
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