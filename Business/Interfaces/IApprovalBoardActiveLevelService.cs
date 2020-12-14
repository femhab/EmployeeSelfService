using Data.Enums;
using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IApprovalBoardActiveLevelService
    {
        Task<BaseResponse> CreateOrUpdate(Guid approvalWorkItemId, Guid serviceId, Level activeLevel);
    }
}
