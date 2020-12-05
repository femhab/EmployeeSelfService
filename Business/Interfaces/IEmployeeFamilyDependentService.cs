using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IEmployeeFamilyDependentService
    {
        Task<BaseResponse> Create(EmployeeFamilyDependent model);
        Task<BaseResponse> Delete(Guid id);
        Task<BaseResponse> Edit(Guid id, string firstName, string lastName, string phoneNumber, DateTime? dob, string address, Guid relationshipId);
        Task<IEnumerable<EmployeeFamilyDependent>> GetByEmployee(Guid employeeId);
        Task<IEnumerable<EmployeeFamilyDependent>> GetAll();
    }
}
