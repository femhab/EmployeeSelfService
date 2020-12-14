using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class AppraisalPeriodService: IAppraisalPeriodService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AppraisalPeriodService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse> Create(DateTime startDate, DateTime enddate)
        {
            var check = await _unitOfWork.GetRepository<AppraisalPeriod>().GetFirstOrDefaultAsync(predicate: x => x.StartDate.Year == startDate.Year);
            if(check == null)
            {
                var otherUpdates = await _unitOfWork.GetRepository<AppraisalPeriod>().GetAllAsync(x => x.IsActive);
                if(otherUpdates != null)
                {
                    foreach(var item in otherUpdates)
                    {
                        item.IsActive = false;
                        _unitOfWork.GetRepository<AppraisalPeriod>().Update(item);
                    }
                }
                var appraisalPeriod = new AppraisalPeriod() { StartDate = startDate, EndDate = enddate, IsActive = true, Id =Guid.NewGuid(), CreatedDate = DateTime.Now };
                _unitOfWork.GetRepository<AppraisalPeriod>().Insert(appraisalPeriod);

                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public Task<BaseResponse> UpdateStatus(Guid id, bool IsActive)
        {
            throw new NotImplementedException();
        }

        public async Task<AppraisalPeriod> GetActivePeriod()
        {
            var data = await _unitOfWork.GetRepository<AppraisalPeriod>().GetFirstOrDefaultAsync(predicate: x => x.IsActive);
            return data;
        }
    }
}
