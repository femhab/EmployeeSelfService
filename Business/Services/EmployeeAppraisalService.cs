using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class EmployeeAppraisalService: IEmployeeAppraisalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IApprovalBoardActiveLevelService _approvalBoardActiveLevelService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly INotificationService _notificationService;

        public EmployeeAppraisalService(IUnitOfWork unitOfWork, IEmployeeApprovalConfigService employeeApprovalConfigService, IApprovalBoardService approvalBoardService, IApprovalBoardActiveLevelService approvalBoardActiveLevelService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _approvalBoardService = approvalBoardService;
            _approvalBoardActiveLevelService = approvalBoardActiveLevelService;
            _notificationService = notificationService;
        }

        public async Task<BaseResponse> Create(EmployeeAppraisal model)
        {
            var check = await _unitOfWork.GetRepository<EmployeeAppraisal>().GetFirstOrDefaultAsync(predicate: x => x.EmployeeId == model.EmployeeId && x.AppraisalPeriodId == model.AppraisalPeriodId);
            if(check == null)
            {
                _unitOfWork.GetRepository<EmployeeAppraisal>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                //submit for approval
                var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("appraisal"));
                var approvalProcessor = await _employeeApprovalConfigService.GetBy(x => x.EmployeeId == model.EmployeeId && x.ApprovalLevel == Level.FirstLevel && x.ApprovalWorkItemId == approvalWorkItem.Id);
                var enlistBoard = new ApprovalBoard()
                {
                    EmployeeId = model.EmployeeId,
                    ApprovalLevel = Level.FirstLevel,
                    Emp_No = model.Emp_No,
                    ApprovalWorkItemId = approvalWorkItem.Id,
                    ApprovalProcessorId = approvalProcessor.ProcessorIId.Value,
                    ApprovalProcessor = approvalProcessor.Processor,
                    ServiceId = model.Id,
                    Status = ApprovalStatus.Pending,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    CreatedBy = model.Emp_No,
                    SignOff = true
                };
                await _approvalBoardService.Create(enlistBoard);
                await _approvalBoardActiveLevelService.CreateOrUpdate(approvalWorkItem.Id, model.Id, Level.FirstLevel);
                await _notificationService.CreateNotification(NotificationAction.AppraisalCreateTitle, NotificationAction.AppraisalCreateMessage, model.EmployeeId, false, false);
                if (approvalProcessor != null)
                {
                    await _notificationService.CreateNotification(NotificationAction.NewApprovalCreateTitle, NotificationAction.ApprovalCreateMessage, approvalProcessor.ProcessorIId.Value, false, false);
                }

                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.AppraisalExist };
        }

        public async Task<IEnumerable<EmployeeAppraisal>> GetByEmployee(Guid employeeId)
        {
            var data = await _unitOfWork.GetRepository<EmployeeAppraisal>().GetAllAsync(predicate: x => x.EmployeeId == employeeId, null, "Employee,AppraisalPeriod");
            return data;
        }

        public async Task<IEnumerable<EmployeeAppraisal>> GetByProcessor(string processorId)
        {
            var data = await _unitOfWork.GetRepository<EmployeeAppraisal>().GetAllAsync(predicate: x => x.LastRatingManagerId == processorId, null, "Employee,AppraisalPeriod");
            return data;
        }

        public async Task<EmployeeAppraisal> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<EmployeeAppraisal>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }

        public async Task<BaseResponse> Update(EmployeeAppraisal model)
        {
            _unitOfWork.GetRepository<EmployeeAppraisal>().Update(model);
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
        }
    }
}
