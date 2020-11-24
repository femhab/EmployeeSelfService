using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IRoleService
    {
        Task<BaseResponse> Create(Role model);
        Task<BaseResponse> Refresh();
        Task<BaseResponse> Edit(Guid id, string description);
        Task<IEnumerable<Role>> GetAll();
        Task<BaseResponse> Delete(Guid id);
    }
}
