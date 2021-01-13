using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IPaymentAdvanceService
    {
        Task<BaseResponse> Create(PaymentAdvance model);
        Task<IEnumerable<PaymentAdvance>> GetAll();
        Task<IEnumerable<PaymentAdvance>> GetByEmployee(Guid employeeId);
        Task<PaymentAdvanceResponseModel> CheckEligibility(string emp_No);
        Task<PaymentAdvance> GetById(Guid id);
    }
}
