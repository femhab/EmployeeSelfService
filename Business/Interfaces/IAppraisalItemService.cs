using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IAppraisalItemService
    {
        Task<BaseResponse> Create(List<AppraisalItem> model);
        Task<IEnumerable<AppraisalItem>> GetByEmployee(Guid employeeId);
        Task<IEnumerable<AppraisalItem>> GetAll();
    }
}
