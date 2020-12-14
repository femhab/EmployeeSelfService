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

        public EmployeeAppraisalService(IUnitOfWork unitOfWork, IEmployeeApprovalConfigService employeeApprovalConfigService, IApprovalBoardService approvalBoardService, IApprovalBoardActiveLevelService approvalBoardActiveLevelService)
        {
            _unitOfWork = unitOfWork;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _approvalBoardService = approvalBoardService;
            _approvalBoardActiveLevelService = approvalBoardActiveLevelService;
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
                    CreatedBy = model.Emp_No
                };
                await _approvalBoardService.Create(enlistBoard);
                await _approvalBoardActiveLevelService.CreateOrUpdate(approvalWorkItem.Id, model.Id, Level.FirstLevel);

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
    }
}
