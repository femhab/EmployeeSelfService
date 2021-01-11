using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    public class TrainingNominationService: ITrainingNominationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SqlConnection _sqlConnection;
        private readonly ITrainingCalenderService _trainingCalenderService;
        public readonly IConfiguration _configuration;
        public readonly IEmployeeService _employeeService;
        public readonly ServiceContext _hrContext;
        public TrainingNominationService(IUnitOfWork unitOfWork, ITrainingCalenderService trainingCalenderService, IConfiguration configuration, IEmployeeService employeeService, ServiceContext hrContext)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
            _trainingCalenderService = trainingCalenderService;
            _employeeService = employeeService;
            _hrContext = hrContext;
        }

        public async Task<BaseResponse> Refresh()
        {
            var topicCalenderResponse = await _trainingCalenderService.Refresh();
            var resource = _hrContext.Set<New_TrainingNomination>().FromSql($"select * from New_TrainingNomination").ToList();
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<TrainingNomination>().GetFirstOrDefaultAsync(predicate: x => x.HRTrainingNominationID == item.New_TrainingNominationID);

                    var calender = await _unitOfWork.GetRepository<TrainingCalender>().GetFirstOrDefaultAsync(predicate: x => x.HRTrainingCalenderID == item.New_TrainingCalendarID) ?? null;

                    var employee = await _employeeService.GetByEmployerIdOrEmail(item.Emp_No);

                    if (check == null && employee != null)
                    {
                        var nomination = new TrainingNomination() { HRTrainingNominationID = item.New_TrainingNominationID, TrainingCalenderId = calender.Id, HRTrainingCalendarID = item.New_TrainingCalendarID, Emp_No = item.Emp_No, EmployeeId = employee.Id, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };

                        _unitOfWork.GetRepository<TrainingNomination>().Insert(nomination);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }


        public async Task<IEnumerable<TrainingNomination>> GetByEmployee(string empNo)
        {
            var data = await GetAll(x => x.Emp_No.ToLower() == empNo.ToLower(), "Employee,TrainingCalender.Topic");
            return data;
        }

        public async Task<TrainingNomination> GetById(Guid id)
        {
            var data = await _unitOfWork.GetRepository<TrainingNomination>().GetFirstOrDefaultAsync(predicate: x => x.Id == id, null, include: c => c.Include(i => i.TrainingCalender).Include(i => i.TrainingCalender.Topic));
            return data;
        }

        public async Task<IEnumerable<TrainingNomination>> GetAll(Expression<Func<TrainingNomination, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<TrainingNomination>().GetAllAsync(predicate, orderBy: source => source.OrderByDescending(c => c.CreatedDate), include);
            return model;
        }

        public async Task<IEnumerable<TrainingNomination>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<BaseResponse> UpdateStatus(Guid nominationId, bool isApplied)
        {
            var nomination = await GetById(nominationId);
            if(nomination != null)
            {
                nomination.IsApplied = isApplied;
                _unitOfWork.GetRepository<TrainingNomination>().Update(nomination);
                await _unitOfWork.SaveChangesAsync();
            }
            return new BaseResponse() { Status = true, Message = ResponseMessage.UpdatedSuccessful};
        }
    }
}
