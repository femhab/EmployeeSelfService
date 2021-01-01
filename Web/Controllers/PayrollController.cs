using AutoMapper;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        public PayrollController(IMapper mapper, IPaymentAdvanceService paymentAdvanceService)
        {
            _mapper = mapper;
            _paymentAdvanceService = paymentAdvanceService;
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
    }
}
