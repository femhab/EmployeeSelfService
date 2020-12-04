using Data.Entities;
using Data.Enums;
using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IExitProcessService
    {
        Task<BaseResponse> Create(ExitProcess model);
        Task<BaseResponse> Update(Guid exitProcessId, ExitProcessStatus status);
    }
}
