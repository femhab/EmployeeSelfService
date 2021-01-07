using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using ViewModel.ResponseModel;
using ViewModel.ServiceModel;

namespace Business.Services
{
    public class RoleService: IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public RoleService(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<BaseResponse> Create(Role model)
        {
            _unitOfWork.GetRepository<Role>().Insert(model);
            await _unitOfWork.SaveChangesAsync();

            await _notificationService.CreateNotification(NotificationAction.RoleCreateTitle, NotificationAction.RoleCreateMessage, null, true, false);

            return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var model = await GetById(id);
            if (model != null)
            {
                _unitOfWork.GetRepository<Role>().Delete(model);
                await _unitOfWork.SaveChangesAsync();
                await _notificationService.CreateNotification(NotificationAction.RoleDeletedTitle, NotificationAction.RoleDeleteMessage, null, true, false);
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<BaseResponse> Edit(Guid id, string description)
        {
            var model = await GetById(id);
            if (model != null)
            {
                model.Description = description;
                model.UpdatedDate = DateTime.Now;

                _unitOfWork.GetRepository<Role>().Update(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful }; ;
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            var data = await GetAll(x => !string.IsNullOrEmpty(x.Id.ToString()));
            return data;
        }

        public async Task<IEnumerable<Role>> GetAll(Expression<Func<Role, bool>> predicate, string include = null, bool includeDeleted = false)
        {
            var model = await _unitOfWork.GetRepository<Role>().GetAllAsync(predicate, orderBy: source => source.OrderBy(c => c.Id));
            return model;
        }

        public async Task<Role> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<Role>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
