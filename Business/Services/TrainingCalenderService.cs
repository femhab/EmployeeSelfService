using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
        public TrainingCalenderService(IUnitOfWork unitOfWork, ITrainingService trainingService)
        {
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(HRDbConfig.ConnectionStringUrl);
            _trainingService = trainingService;
        }

        public async Task<BaseResponse> Refresh()
        {
            var topicResponse = await _trainingService.RefreshTopics();

            var sql = "select * from New_TrainingCalendar";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<New_TrainingCalendar> resource = new List<New_TrainingCalendar>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    New_TrainingCalendar requester = new New_TrainingCalendar()
                    {
                        New_TrainingCalendarID = reader.GetOrdinal("New_TrainingCalendarID"),
                        Topic = reader["Topic"].ToString(),
                        TrainingYear = reader.GetOrdinal("TrainingYear"),
                        StartDate = reader["StartDate"].ToString(),
                        EndDate = reader["EndDate"].ToString(),
                        Organiser = reader["Organiser"].ToString(),
                        Venue = reader["Venue"].ToString(),
                        AmtPerHead = reader.GetDecimal(reader.GetOrdinal("AmtPerHead")),
                        //InternalFlag = reader.GetBoolean("InternalFlag"),
                        New_TrainingRoomID = reader.GetOrdinal("New_TrainingRoomID"),
                        HoursPerDay = reader.GetOrdinal("HoursPerDay"),
                        IsInternational = reader.GetOrdinal("IsInternational"),
                        Emp_no = reader["Emp_no"].ToString(),
                        train_cat = reader["train_cat"].ToString(),
                        //attended = reader.GetBoolean("attended"),
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<TrainingCalender>().GetFirstOrDefaultAsync(predicate: x => x.HRTrainingCalenderID == item.New_TrainingCalendarID);

                    var topic = await _unitOfWork.GetRepository<TrainingTopics>().GetFirstOrDefaultAsync(predicate: x => x.Title.ToLower() == item.Topic.ToLower()) ?? null;

                    if (check == null)
                    {
                        var calender = new TrainingCalender() { HRTrainingCalenderID = item.New_TrainingCalendarID, TopicId = topic.Id, TrainingYear = item.TrainingYear, AmtPerHead = item.AmtPerHead, Attended = item.attended, Emp_no = item.Emp_no, StartDate = item.StartDate, EndDate = item.EndDate, HoursPerDay = item.HoursPerDay, InternalFlag = item.InternalFlag, IsInternational = item.IsInternational, Organiser = item.Organiser, TrainingCategory = item.train_cat, TrainingRoomID = item.New_TrainingRoomID, Venue = item.Venue, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };

                        _unitOfWork.GetRepository<TrainingCalender>().Insert(calender);
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
            var model = await _unitOfWork.GetRepository<TrainingCalender>().GetAllAsync(predicate, orderBy: source => source.OrderByDescending(c => c.CreatedDate));
            return model;
        }

        public async Task<IEnumerable<TrainingCalender>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }
    }
}
