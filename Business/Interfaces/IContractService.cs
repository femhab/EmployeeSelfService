using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IContractService
    {
        Task<BaseResponse> UpdateContract(Guid contractId, List<ContractItem> items);
        Task<BaseResponse> Create(ContractObjective model, List<ContractItem> items);
        Task<ContractObjective> GetByObjectiveId(Guid objectiveId);
        Task<IEnumerable<ContractItem>> GetItemByObjectiveId(Guid objectiveId);
        Task<IEnumerable<ContractObjective>> GetByLineManager(string lineManagerNo);
        Task<IEnumerable<ContractObjective>> GetByEmployee(Guid employeeId);
    }
}
