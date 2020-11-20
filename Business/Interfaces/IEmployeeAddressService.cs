using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IEmployeeAddressService
    {
        Task<BaseResponse> Create(EmployeeAddress model);
        Task<BaseResponse> Edit(Guid id, string streetAddress, string state, string city, string country, string stateOfOrigin, string lGOfOrigin);
        Task<EmployeeAddress> GetByEmployee(Guid employeeId);
        Task<IEnumerable<EmployeeAddress>> GetAll(Expression<Func<EmployeeAddress, bool>> predicate, string include = null, bool includeDeleted = false);
        Task<IEnumerable<EmployeeAddress>> GetByDepartment(Guid departmentId);
    }
}
