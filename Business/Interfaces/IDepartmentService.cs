using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IDepartmentService
    {
        Task<BaseResponse> Refresh();
        Task<IEnumerable<Department>> GetAll();
        Task<IEnumerable<Department>> GetByExitApproval();
    }
}
