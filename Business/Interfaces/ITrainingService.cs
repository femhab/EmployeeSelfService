using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface ITrainingService
    {
        Task<BaseResponse> Create(Training model, Guid? nominationId);
        Task<IEnumerable<Training>> GetByEmployee(Guid employeeId);
        Task<BaseResponse> RefreshTopics();
        Task<IEnumerable<TrainingTopics>> GetAll();
        Task<Training> GetById(Guid id);
    }
}
