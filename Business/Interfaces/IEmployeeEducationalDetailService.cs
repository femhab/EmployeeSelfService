using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IEmployeeEducationalDetailService
    {
        Task<BaseResponse> Create(EmployeeEducationDetail model);
        Task<BaseResponse> Edit(Guid id, Guid levelId, Guid qualificationId, Guid gradeId);
        Task<IEnumerable<EmployeeEducationDetail>> GetByEmployee(Guid employeeId);
    }
}
