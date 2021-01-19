using System;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class LeaveRecallService: ILeaveRecallService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILeaveService _leaveService;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IApprovalBoardActiveLevelService _approvalBoardActiveLevelService;

        public LeaveRecallService(IUnitOfWork unitOfWork, ILeaveService leaveService, IEmployeeApprovalConfigService employeeApprovalConfigService, IApprovalBoardService approvalBoardService, IApprovalBoardActiveLevelService approvalBoardActiveLevelService)
        {
            _unitOfWork = unitOfWork;
            _leaveService = leaveService;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _approvalBoardService = approvalBoardService;
            _approvalBoardActiveLevelService = approvalBoardActiveLevelService;
        }

        public async Task<BaseResponse> Create(LeaveRecall model)
        {
            var check = await _unitOfWork.GetRepository<Leave>().GetFirstOrDefaultAsync(predicate: x => x.Id == model.LeaveId);
            var checkRecall = await _unitOfWork.GetRepository<LeaveRecall>().GetFirstOrDefaultAsync(predicate: x => x.LeaveId == model.LeaveId);

            if(check != null && checkRecall == null)
            {
                _unitOfWork.GetRepository<LeaveRecall>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("leave"));
                var approvalProcessor = await _employeeApprovalConfigService.GetBy(x => x.EmployeeId == check.EmployeeId && x.ApprovalLevel == Level.FirstLevel && x.ApprovalWorkItemId == approvalWorkItem.Id);
                try
                {
                    var approvalWorkLeaveItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("recall"));

                    var enlistBoard = new ApprovalBoard()
                    {
                        EmployeeId = check.EmployeeId,
                        ApprovalLevel = Level.FirstLevel,
                        Emp_No = check.Emp_No,
                        ApprovalWorkItemId = approvalWorkLeaveItem.Id,
                        ApprovalProcessorId = approvalProcessor.ProcessorIId.Value,
                        ApprovalProcessor = approvalProcessor.Processor,
                        ServiceId = model.Id,
                        Status = ApprovalStatus.Pending,
                        CreatedDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                        CreatedBy = check.Emp_No
                    };
                    await _approvalBoardService.Create(enlistBoard);
                    await _approvalBoardActiveLevelService.CreateOrUpdate(approvalWorkLeaveItem.Id, model.Id, Level.FirstLevel);

                }
                catch (Exception ex)
                {
                    throw ex;
                }

                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.LeaveRecallExecuted };
        }

        public async Task<LeaveRecall> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<LeaveRecall>().GetFirstOrDefaultAsync(predicate: c => c.Id == id, null, include: e => e.Include(i => i.Leave).Include(i => i.Leave.LeaveType));
            return model;
        }
    }
}
