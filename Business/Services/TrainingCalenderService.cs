using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Services
{
    public class TrainingCalenderService: ITrainingCalenderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly ITrainingService _trainingService;
        private readonly ServiceContext _hrContext; 
        private readonly IConfiguration _configuration;

        public TrainingCalenderService(IUnitOfWork unitOfWork, ITrainingService trainingService, ServiceContext hrContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
            _trainingService = trainingService;
            _hrContext = hrContext;
        }

        public async Task<BaseResponse> Refresh()
        {
            var topicResponse = await _trainingService.RefreshTopics();
            var resource = _hrContext.Set<New_TrainingCalendar>().FromSql($"select * from New_TrainingCalendar").ToList();

            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<TrainingCalender>().GetFirstOrDefaultAsync(predicate: x => x.HRTrainingCalenderID == item.New_TrainingCalendarID);

                    var topic = await _unitOfWork.GetRepository<TrainingTopics>().GetFirstOrDefaultAsync(predicate: x => x.Code.ToLower() == item.Topic.ToLower() ) ?? null;

                    if (check == null && topic != null)
                    {
                        try
                        {
                            var calender = new TrainingCalender() { HRTrainingCalenderID = item.New_TrainingCalendarID, TopicId = topic.Id, TrainingYear = item.TrainingYear.Value, AmtPerHead = item.AmtPerHead, StartDate = item.StartDate.Value, EndDate = item.EndDate.Value, Organiser = item.Organiser, TrainingRoomID = item.New_TrainingRoomID ?? 0, Venue = item.Venue, HoursPerDay = item.HoursPerDay.Value, CreatedDate = DateTime.Now, Id = Guid.NewGuid(), InternalFlag = item.InternalFlag, IsInternational = item.IsInternational ?? 0 }; 
                            _unitOfWork.GetRepository<TrainingCalender>().Insert(calender);
                        }
                       catch(Exception ex)
                        {
                            throw ex;
                        }
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<TrainingCalender> GetByTopicId(Guid topicId)
        {
            var data = (await GetAll(x => x.TopicId == topicId)).FirstOrDefault();
            return data;
        }

        public async Task<IEnumerable<TrainingCalender>> GetAll(Expression<Func<TrainingCalender, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<TrainingCalender>().GetAllAsync(predicate, orderBy: source => source.OrderByDescending(c => c.CreatedDate), include);
            return model;
        }

        public async Task<IEnumerable<TrainingCalender>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }
    }
}
