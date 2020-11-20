using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class ApprovalBoardService: IApprovalBoardService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApprovalBoardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> ApprovalAction(Guid ProcessorId, ApprovalStatus status, Level approvalLevel, Guid serviceId, Guid approvalWorkItemId)
        {
            var data = await _unitOfWork.GetRepository<ApprovalBoard>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalWorkItemId == approvalWorkItemId && x.ServiceId == serviceId && x.ApprovalLevel == approvalLevel);

            if(data != null && data.ApprovalProcessorId == ProcessorId)
            {
                if (data.Status != status)
                    data.Status = status;
                else
                    return new BaseResponse() { Status = false, Message = ResponseMessage.AlreadyApproved};

                _unitOfWork.GetRepository<ApprovalBoard>().Update(data);
                await _unitOfWork.SaveChangesAsync();
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Create(ApprovalBoard model)
        {
            _unitOfWork.GetRepository<ApprovalBoard>().Insert(model);
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
        }

        public async  Task<IPagedList<ApprovalBoard>> GetApprovalUpdate(Guid serviceId, Guid approvalWorkItemId)
        {
            var model = await GetPagedList(c => c.ServiceId == serviceId && c.ApprovalWorkItemId == approvalWorkItemId, include: e => e.Include(i => i.Employee).Include(x => x.ApprovalProcessor).Include(i => i.ApprovalWorkItem));
            return model;
        }
         
        public async Task<IPagedList<ApprovalBoard>> GetByProcessor(Guid processorId, int pageIndex = 0, int pageSize = 20)
        {
            var model = await GetPagedList(c => c.ApprovalProcessorId == processorId, include: e => e.Include(i => i.Employee).Include(x => x.ApprovalProcessor).Include(i => i.ApprovalWorkItem), pageIndex: pageIndex, pageSize: pageSize);

            return model;
        }

        public async Task<IPagedList<ApprovalBoard>> GetPagedList(Expression<Func<ApprovalBoard, bool>> predicate = null, Func<IQueryable<ApprovalBoard>, IIncludableQueryable<ApprovalBoard, object>> include = null, int pageIndex = 0, int pageSize = 20)
        {

            var model = await _unitOfWork.GetRepository<ApprovalBoard>().GetPagedListAsync(predicate, source => source.OrderBy(c => c.CreatedDate), include: e => e.Include(i => i.Employee).Include(x => x.ApprovalWorkItem).Include(i => i.ApprovalProcessor), pageIndex: pageIndex, pageSize: pageSize);
            return model;
        }
    }
}
