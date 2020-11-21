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
        Task<BaseResponse> Create(Employee model);
        Task<BaseResponse> RequestBasicInfoChange(Guid id, string lastName, string firstName, string email);
        Task<Employee> GetById(Guid id);
        Task<IEnumerable<Employee>> GetAll();
        Task<IEnumerable<Employee>> GetByDepartment(Guid departmentId);
    }
}
