using Data.Entities;
using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IAppraisalPeriodService
    {
        Task<BaseResponse> Create(DateTime startDate, DateTime enddate);
       // Task<BaseResponse> UpdateDate(Guid id, DateTime? startDate, DateTime? enddate);
        Task<BaseResponse> UpdateStatus(Guid id, bool IsActive);
        Task<AppraisalPeriod> GetActivePeriod();
    }
}
