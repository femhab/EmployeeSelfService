using Data.Entities;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IDisciplinaryActionService
    {
        Task<BaseResponse> CreateQuery(Guid employeeId, string empNo, string subject, string message, Guid targetEmployeeId, string targetEmployeeNo);
        Task<BaseResponse> ReplyQuery(Guid queryId, string reply);
        Task<BaseResponse> GiveAction(Guid queryId, string actionComment, QueryAction action);
        Task<IEnumerable<DisciplinaryAction>> GetByEmployee(Guid employeeId);
        Task<IEnumerable<DisciplinaryAction>> GetByTargetEmployee(Guid targetEmployeeId);
    }
}
