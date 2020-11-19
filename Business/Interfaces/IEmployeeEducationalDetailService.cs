using Data.Entities;
using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IEmployeeEducationalDetailService
    {
        Task<BaseResponse> Create(EmployeeEducationDetail model);
        Task<BaseResponse> Edit(EmployeeEducationDetail model);
        Task<EmployeeEducationDetail> GetByEmployee(Guid employeeId);
    }
}
