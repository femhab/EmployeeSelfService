using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IApprovalWorkItemService
    {
        Task<BaseResponse> Create(ApprovalWorkItem model);
        Task<IEnumerable<ApprovalWorkItem>> GetAll(Expression<Func<ApprovalWorkItem, bool>> predicate, string include = null, bool includeDeleted = false);
        Task<BaseResponse> Edit(Guid id, string name, string description);
        Task<BaseResponse> Delete(Guid Id);
    }
}
