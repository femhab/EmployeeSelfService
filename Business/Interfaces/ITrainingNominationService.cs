using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface ITrainingNominationService
    {
        Task<BaseResponse> Refresh();
        Task<IEnumerable<TrainingNomination>> GetByEmployee(string empNo);
        Task<TrainingNomination> GetById(Guid id);
        Task<BaseResponse> UpdateStatus(Guid nominationId, bool isApplied);
    }
}
