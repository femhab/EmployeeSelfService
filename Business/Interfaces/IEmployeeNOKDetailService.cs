using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IEmployeeNOKDetailService
    {
        Task<BaseResponse> Create(EmployeeNOKDetail model);
        Task<BaseResponse> Delete(Guid id);
        Task<BaseResponse> Edit(Guid id, string firstName, string lastName, string email, string phoneNumber, DateTime? dob, string address, Guid relationshipId);
        Task<EmployeeNOKDetail> GetByEmployee(Guid employeeId);
        Task<IEnumerable<EmployeeNOKDetail>> GetAll();
    }
}
