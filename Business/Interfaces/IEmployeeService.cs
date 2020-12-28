using Data.Entities;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<HRUsers>> GetUnregisteredBaseEmployee();
        Task<Employee> GetByEmployerIdOrEmail(string employeeIdOrEmail);
        Task<EmployeeResponseModel> Create(Employee model);
        Task<BaseResponse> RequestBasicInfoChange(Guid id, string lastName, string firstName, string email);
        Task<Employee> GetById(Guid id);
        Task<IEnumerable<Employee>> GetAll();
        Task<IEnumerable<Employee>> GetByDepartment(Guid departmentId);
        Task<IEnumerable<Employee>> GetAllLowGradeEmployee(Guid employeeId);
        Task<BaseResponse> RequestTransfer(Guid id, Guid divisionId, Guid departmentId, Guid sectionId, Guid? unitId);
        Task<BaseResponse> UpdateAccessType(Guid employeeId, AccessType accessType);
        Task<IEnumerable<Employee>> GetAll(Expression<Func<Employee, bool>> predicate, string include = null, bool includeDeleted = false);
    }
}
