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
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IApprovalBoardActiveLevelService _approvalBoardActiveLevelService;

        public ApprovalBoardService(IUnitOfWork unitOfWork, IEmployeeApprovalConfigService employeeApprovalConfigService, IApprovalBoardActiveLevelService approvalBoardActiveLevelService)
        {
            _unitOfWork = unitOfWork;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _approvalBoardActiveLevelService = approvalBoardActiveLevelService;
        }

        public async Task<BaseResponse> ApprovalAction(Guid ProcessorId, ApprovalStatus status, Level approvalLevel, Guid serviceId, Guid approvalWorkItemId)
        {
            var data = await _unitOfWork.GetRepository<ApprovalBoard>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalWorkItemId == approvalWorkItemId && x.ServiceId == serviceId && x.ApprovalLevel == approvalLevel);

            if(data != null && data.ApprovalProcessorId == ProcessorId)
            {
                if (data.Status != status)
                {
                    data.Status = status;
                    data.UpdatedDate = DateTime.Now;
                }
                else
                {
                    return new BaseResponse() { Status = false, Message = ResponseMessage.AlreadyApproved };
                }

                _unitOfWork.GetRepository<ApprovalBoard>().Update(data);

                if (status == ApprovalStatus.Approved)
                {
                    //look for new approver for next level and assign the neccessary
                   var newApprovalLevel = approvalLevel + 1;
                    var approvalProcessor = await _employeeApprovalConfigService.GetBy(x => x.EmployeeId == data.EmployeeId && x.ApprovalLevel == newApprovalLevel && x.ApprovalWorkItemId == data.ApprovalWorkItemId);

                    var enlistBoard = new ApprovalBoard()
                    {
                        EmployeeId = data.EmployeeId,
                        ApprovalLevel = (approvalProcessor != null) ? newApprovalLevel: Level.HR,
                        Emp_No = data.Emp_No,
                        ApprovalWorkItemId = data.ApprovalWorkItemId,
                        ApprovalProcessorId = (approvalProcessor != null) ? approvalProcessor.ProcessorIId.Value: Guid.Empty,
                        ApprovalProcessor = (approvalProcessor != null) ? approvalProcessor.Processor : null,
                        ServiceId = data.ServiceId,
                        Status = ApprovalStatus.Pending,
                        CreatedDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        CreatedBy = data.Emp_No
                    };

                    await Create(enlistBoard);
                    try
                    {
                        await _approvalBoardActiveLevelService.CreateOrUpdate(data.ApprovalWorkItemId, data.ServiceId, (approvalProcessor != null) ? newApprovalLevel : Level.HR);
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                    
                }

                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.ApprovedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Create(ApprovalBoard model)
        {
            var check = await _unitOfWork.GetRepository<ApprovalBoard>().GetFirstOrDefaultAsync(predicate: x => x.ServiceId == model.ServiceId && x.ApprovalWorkItemId == model.ApprovalWorkItemId && x.EmployeeId == model.EmployeeId && x.ApprovalLevel == model.ApprovalLevel);
            if (check == null)
            {
                try
                {
                    _unitOfWork.GetRepository<ApprovalBoard>().Insert(model);
                    await _unitOfWork.SaveChangesAsync();
                    return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.RecordExist };
        }

        public async  Task<IEnumerable<ApprovalBoard>> GetApprovalUpdate(Guid serviceId, Guid approvalWorkItemId)
        {
            var model = await GetAll(x => x.ServiceId == serviceId && x.ApprovalWorkItemId == approvalWorkItemId, "Employee,ApprovalWorkItem");
            return model;
        }
         
        public async Task<IEnumerable<ApprovalBoard>> GetByProcessor(Guid processorId)
        {
            var model = await GetAll(x => x.ApprovalProcessorId == processorId && x.Status == ApprovalStatus.Pending, "Employee,ApprovalWorkItem");
            return model;
        }

        public async Task<IEnumerable<ApprovalBoard>> GetAll(Expression<Func<ApprovalBoard, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<ApprovalBoard>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<IPagedList<ApprovalBoard>> GetPagedList(Expression<Func<ApprovalBoard, bool>> predicate = null, Func<IQueryable<ApprovalBoard>, IIncludableQueryable<ApprovalBoard, object>> include = null, int pageIndex = 0, int pageSize = 20)
        {

            var model = await _unitOfWork.GetRepository<ApprovalBoard>().GetPagedListAsync(predicate, source => source.OrderBy(c => c.CreatedDate), include: e => e.Include(i => i.Employee).Include(x => x.ApprovalWorkItem).Include(i => i.ApprovalProcessor), pageIndex: pageIndex, pageSize: pageSize);
            return model;
        }

        public async Task<ApprovalBoard> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<ApprovalBoard>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
