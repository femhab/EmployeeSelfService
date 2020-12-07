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

        public LeaveRecallService(IUnitOfWork unitOfWork, ILeaveService leaveService)
        {
            _unitOfWork = unitOfWork;
            _leaveService = leaveService;
        }

        public async Task<BaseResponse> Create(LeaveRecall model)
        {
            var check = await _unitOfWork.GetRepository<Leave>().GetFirstOrDefaultAsync(predicate: x => x.Id == model.LeaveId);

            if(check != null)
            {
                _unitOfWork.GetRepository<LeaveRecall>().Insert(model);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.LeaveExecuted };
        }
    }
}
