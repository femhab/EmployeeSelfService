using Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface ILeaveService
    {
        Task<BaseResponse> Create(Leave model);
        Task<IEnumerable<Leave>> GetByEmployee(Guid employeeId);
        Task<IEnumerable<Leave>> GetByDepartment(Guid departmentId);
        Task<IEnumerable<Leave>> GetEmployeeOnLeave(); //onleave
        Task<IEnumerable<Leave>> GetApprovedLeave(); //approved but not started
        Task<BaseResponse> Edit(Guid id, DateTime startDate, int noOfDays);
        Task<BaseResponse> Delete(Guid id);
        Task<LeaveResponseModel> CheckEligibility(Guid employeeId);
        Task EmployeeLeavePreset(Guid employeeId, string empNo, Guid gradeLevelId);
        Task ResetEmployeeLeave();
        Task<IEnumerable<Leave>> GetAll(Expression<Func<Leave, bool>> predicate, string include = null, bool includeDeleted = false);
        Task<Leave> GetById(Guid id);
    }
}
