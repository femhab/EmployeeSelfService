using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IApprovalSettingService
    {
        Task<BaseResponse> Create(ApprovalSetting model);
        Task<IEnumerable<ApprovalSetting>> GetAll();
        Task<BaseResponse> Edit(ApprovalSetting model);
        Task<BaseResponse> Delete(Guid Id);
    }
}
