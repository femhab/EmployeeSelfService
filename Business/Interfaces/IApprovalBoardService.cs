using Data.Entities;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IApprovalBoardService
    {
        Task<BaseResponse> Create(ApprovalBoard model);
        Task<BaseResponse> ApprovalAction(Guid ProcessorId, ApprovalStatus status, Guid ApprovalSettingId);
        Task<IEnumerable<ApprovalBoard>> GetByProcessor(Guid ProcessorId);
        Task<IEnumerable<ProcessedWorkItem>> GetApprovalUpdate(Guid approvalBoardId);
    }
}
