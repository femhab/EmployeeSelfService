using Data.Entities;
using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IExitProcessPriorityItemService
    {
        Task<BaseResponse> Create(Guid exitProcessId, Guid clearanceDepartmentId, string clearanceOfficer, string comment);
    }
}
