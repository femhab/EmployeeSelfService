using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface ILoanTypeService
    {
        Task<BaseResponse> Create(LoanType model);
        Task<IEnumerable<LoanType>> GetAll();
        Task<BaseResponse> Edit(Guid id, string name);
        Task<BaseResponse> Delete(Guid id);
    }
}
