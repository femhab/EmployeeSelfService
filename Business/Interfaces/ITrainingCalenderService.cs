using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface ITrainingCalenderService
    {
        Task<BaseResponse> Refresh();
        Task<TrainingCalender> GetByTopicId(Guid topicId);
        //Task<IEnumerable<TrainingCalender>> GetByEmployee(string empNo);
    }
}
