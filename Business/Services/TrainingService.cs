using Business.Interfaces;
using Data.Entities;
using Data.Enums;
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
    public class TrainingService: ITrainingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeApprovalConfigService _employeeApprovalConfigService;
        private readonly IApprovalBoardService _approvalBoardService;
        private readonly IApprovalBoardActiveLevelService _approvalBoardActiveLevelService;
        private readonly INotificationService _notificationService;
        private readonly SqlConnection _sqlConnection;
        private readonly IConfiguration _configuration;


        public TrainingService(IUnitOfWork unitOfWork, IEmployeeApprovalConfigService employeeApprovalConfigService, IApprovalBoardService approvalBoardService, IApprovalBoardActiveLevelService approvalBoardActiveLevelService, INotificationService notificationService, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _employeeApprovalConfigService = employeeApprovalConfigService;
            _approvalBoardService = approvalBoardService;
            _approvalBoardActiveLevelService = approvalBoardActiveLevelService;
            _notificationService = notificationService;
            _sqlConnection = new SqlConnection(_configuration["ConnectionStrings:HRServerConnection"]);
        }

        public async Task<BaseResponse> Create(Training model, Guid? nominationId)
        {
            _unitOfWork.GetRepository<Training>().Insert(model);
            await _unitOfWork.SaveChangesAsync();

            //submit for approval
            var approvalWorkItem = await _unitOfWork.GetRepository<ApprovalWorkItem>().GetFirstOrDefaultAsync(predicate: x => x.Name.ToLower().Contains("training"));
            var approvalProcessor = await _employeeApprovalConfigService.GetBy(x => x.EmployeeId == model.EmployeeId && x.ApprovalLevel == Level.FirstLevel && x.ApprovalWorkItemId == approvalWorkItem.Id);
            try
            {
                var enlistBoard = new ApprovalBoard()
                {
                    EmployeeId = model.EmployeeId,
                    ApprovalLevel = Level.FirstLevel,
                    Emp_No = model.Emp_No,
                    ApprovalWorkItemId = approvalWorkItem.Id,
                    ApprovalProcessorId = approvalProcessor.ProcessorIId.Value,
                    ApprovalProcessor = approvalProcessor.Processor,
                    ServiceId = model.Id,
                    Status = ApprovalStatus.Pending,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    CreatedBy = model.Emp_No
                };
                await _approvalBoardService.Create(enlistBoard);
                await _approvalBoardActiveLevelService.CreateOrUpdate(approvalWorkItem.Id, model.Id, Level.FirstLevel);
                await _notificationService.CreateNotification(NotificationAction.TrainingCreateTitle, NotificationAction.TrainingCreateMessage, model.EmployeeId, false, false);
                if (approvalProcessor != null)
                {
                    await _notificationService.CreateNotification(NotificationAction.NewApprovalCreateTitle, NotificationAction.ApprovalCreateMessage, approvalProcessor.ProcessorIId.Value, false, false);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            await _notificationService.CreateNotification(NotificationAction.TrainingCreateTitle, NotificationAction.TrainingCreateMessage, model.EmployeeId, false, false);

            return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
        }

        public async Task<BaseResponse> RefreshTopics()
        {
            var sql = "select * from HRTopics";
            SqlCommand query = new SqlCommand(sql, _sqlConnection);
            List<HRTopics> resource = new List<HRTopics>();
            _sqlConnection.Open();
            using (SqlDataReader reader = query.ExecuteReader())
            {
                while (reader.Read())
                {
                    HRTopics requester = new HRTopics()
                    {
                        SkillCode = reader["SkillCode"].ToString(),
                        descc = reader["descc"].ToString()
                    };
                    resource.Add(requester);
                }
                _sqlConnection.Close();
            }
            if (resource != null)
            {
                foreach (var item in resource)
                {
                    var check = await _unitOfWork.GetRepository<TrainingTopics>().GetFirstOrDefaultAsync(predicate: x => x.Code.ToLower() == item.SkillCode.ToLower());

                    if (check == null)
                    {
                        var topics = new TrainingTopics() { Code = item.SkillCode, Title = item.descc, CreatedDate = DateTime.Now, Id = Guid.NewGuid()};

                        _unitOfWork.GetRepository<TrainingTopics>().Insert(topics);
                    }

                }
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<Training>> GetByEmployee(Guid employeeId)
        {
            var data = await GetAll(x => x.EmployeeId == employeeId, "Employee");
            return data;
        }

        public async Task<IEnumerable<Training>> GetAll(Expression<Func<Training, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Training>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id), include);
            return model;
        }

        public async Task<IEnumerable<TrainingTopics>> GetAllTopics(Expression<Func<TrainingTopics, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<TrainingTopics>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<IEnumerable<TrainingTopics>> GetAll()
        {
            var data = await GetAllTopics(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<Training> GetById(Guid id)
        {
            var data = await _unitOfWork.GetRepository<Training>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            return data;
        }
    }
}
