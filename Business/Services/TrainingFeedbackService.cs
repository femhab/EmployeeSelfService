using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class TrainingFeedbackService: ITrainingFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public TrainingFeedbackService(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<BaseResponse> Create(TrainingFeedback model)
        {
            _unitOfWork.GetRepository<TrainingFeedback>().Insert(model);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.CreateNotification(NotificationAction.FeedbackCreateTitle, NotificationAction.RoleCreateMessage, null, true, false);

            return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
        }

        public async Task<IEnumerable<TrainingFeedback>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<IEnumerable<TrainingFeedback>> GetAll(Expression<Func<TrainingFeedback, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<TrainingFeedback>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<IEnumerable<TrainingFeedback>> GetByEmployee(Guid trainingId, Guid employeeId)
        {
            var model = await GetAll(predicate: c => c.TrainingId == trainingId && c.EmployeeId == employeeId);
            return model;
        }

        public async Task<TrainingFeedback> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<TrainingFeedback>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
