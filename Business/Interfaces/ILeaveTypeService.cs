using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface ILeaveTypeService
    {
        Task<BaseResponse> Refresh();
        Task<BaseResponse> Edit(Guid id, int availableDays);
        Task<IEnumerable<LeaveType>> GetAll();
    }
}
