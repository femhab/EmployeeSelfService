using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IApprovalWorkItemService
    {
        Task<BaseResponse> Create(ApprovalWorkItem model);
        Task<IEnumerable<ApprovalWorkItem>> GetAll();
        Task<BaseResponse> Edit(ApprovalWorkItem model);
        Task<BaseResponse> Delete(Guid Id);
    }
}
