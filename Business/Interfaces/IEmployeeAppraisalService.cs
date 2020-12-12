using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IEmployeeAppraisalService
    {
        Task<BaseResponse> Create(EmployeeAppraisal model);
        Task<IEnumerable<EmployeeAppraisal>> GetByEmployee(Guid employeeId);
        Task<IEnumerable<EmployeeAppraisal>> GetByProcessor(Guid processorId);//to use activelevel for trace
    }
}
