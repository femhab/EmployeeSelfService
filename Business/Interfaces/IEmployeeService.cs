using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<HRUsers>> GetUnregisteredBaseEmployee();
        Task<BaseResponse> SynDataToESS(Guid employeeId);
        Task<BaseResponse> Create(Employee model);
        Task<BaseResponse> Edit(Employee model);
        Task<Employee> GetById(Guid id);
        Task<Employee> GetAll();
        Task<Employee> GetByDepartment(Guid departmentId);
        Task<Employee> GetByRole(Guid roleId);
    }
}
