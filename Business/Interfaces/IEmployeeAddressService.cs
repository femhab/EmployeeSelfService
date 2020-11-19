using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IEmployeeAddressService
    {
        Task<BaseResponse> Create(EmployeeAddress model);
        Task<BaseResponse> Edit(EmployeeAddress model);
        Task<EmployeeAddress> GetByEmployee(Guid employeeId);
        Task<IEnumerable<EmployeeAddress>> GetAll();
        Task<IEnumerable<EmployeeAddress>> GetByDepartment(Guid departmentId);
    }
}
