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

        public LeaveRecallService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(LeaveRecall model)
        {
            var check = await _unitOfWork.GetRepository<Leave>().GetFirstOrDefaultAsync(predicate: x => x.Id == model.LeaveId && x.LeaveStatus == LeaveStatus.Recall);
            if(check != null)
            {
                var interval = Convert.ToInt32(check.ActualEndDate - check.DateFrom);
                if(check.DaysUsed < interval)
                {
                    _unitOfWork.GetRepository<LeaveRecall>().Insert(model);
                    await _unitOfWork.SaveChangesAsync();

                    return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
                }
                return new BaseResponse() { Status = false, Message = ResponseMessage.MaximumLeaveReached };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.LeaveExecuted };
        }
    }
}
