using Data.Entities;
using System;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface ITrainingCalenderService
    {
        Task<BaseResponse> Refresh();
        Task<TrainingCalender> GetByTopicId(Guid topicId);
    }
}
