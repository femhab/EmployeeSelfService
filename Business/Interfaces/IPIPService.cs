using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IPIPService
    {
        Task<BaseResponse> CreatePIP(PIP model);
        Task<BaseResponse> CreatePIPItem(PIPItem model);
        Task<IEnumerable<PIP>> GetByEmployee(string employeeNo);
        Task<IEnumerable<PIPItem>> GetByPIP(Guid pipId);
        Task<PIP> GetById(Guid id);
        Task<BaseResponse> ClosePIP(Guid pipId);
    }
}
