using Business.Interfaces;
using Data.Entities;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class ApprovalBoardActiveLevelService: IApprovalBoardActiveLevelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApprovalBoardActiveLevelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> CreateOrUpdate(Guid approvalWorkItemId, Guid serviceId, Level activeLevel)
        {
            var check = await _unitOfWork.GetRepository<ApprovalBoardActiveLevel>().GetFirstOrDefaultAsync(predicate: x => x.ApprovalWorkItemId == approvalWorkItemId && x.ServiceId == serviceId);
            if(check == null)
            {
                _unitOfWork.GetRepository<ApprovalBoardActiveLevel>().Insert(new ApprovalBoardActiveLevel()
                {
                    ApprovalWorkItemId = approvalWorkItemId,
                    ServiceId = serviceId,
                    ActiveLevel = activeLevel,
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now
                });

                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            else
            {
                check.ActiveLevel = activeLevel;
                _unitOfWork.GetRepository<ApprovalBoardActiveLevel>().Update(check);

                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful };
            }
        }
    }
}
