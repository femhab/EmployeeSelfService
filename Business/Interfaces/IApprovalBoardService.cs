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
        Task<IPagedList<ApprovalBoard>> GetByProcessor(Guid processorId, int pageIndex = 0, int pageSize = 20);
        Task<IPagedList<ApprovalBoard>> GetApprovalUpdate(Guid serviceId, Guid approvalWorkItemId);
    }
}
