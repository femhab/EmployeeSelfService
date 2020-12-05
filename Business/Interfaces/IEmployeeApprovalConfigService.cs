using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IEmployeeApprovalConfigService
    {
        Task<BaseResponse> CreateUpdate(List<EmployeeApprovalConfig> model);
        Task<BaseResponse> SetApprovalCount(EmployeeApprovalCount model);
        Task<int> GetApprovalCount(Guid employeeId, Guid approvalWorkItemId);
    }
}
