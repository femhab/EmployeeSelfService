using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IApprovalBoardService
    {
        Task<BaseResponse> Create(ApprovalBoard model);
        Task<BaseResponse> ApprovalAction(Guid ProcessorId, ApprovalStatus status, Level approvalLevel, Guid serviceId, Guid approvalWorkItemId);
        Task<IEnumerable<ApprovalBoard>> GetByProcessor(Guid processorId);
        Task<IEnumerable<ApprovalBoard>> GetApprovalUpdate(Guid serviceId, Guid approvalWorkItemId);
        Task<ApprovalBoard> GetUnsignedAppraisal(Guid serviceId);
        Task<BaseResponse> SignOffAppraisal(Guid appraisalId);
        Task<ApprovalBoard> GetById(Guid id);
    }
}
