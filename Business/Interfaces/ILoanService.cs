using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface ILoanService
    {
        Task<BaseResponse> Create(Loan model);
        Task<BaseResponse> Edit(Loan request);
        Task<IEnumerable<Loan>> GetByEmployee(Guid employeeId);
        Task<LoanEligibilityResponseModel> CheckEligibility(string Emp_No);
    }
}
