using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class NotificationService: INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateNotification(string title, string message, Guid? employeeId, bool isGeneral = false, bool sendExtraNotification = false)
        {
            var notification = new Notification()
            {
                EmployeeId = employeeId,
                Title = title,
                Message = message,
                IsGeneral = isGeneral,
                Id = Guid.NewGuid(),
                CreatedDate = DateTime.Now
            };

            _unitOfWork.GetRepository<Notification>().Insert(notification);
            await _unitOfWork.SaveChangesAsync();
            if (sendExtraNotification)
            {
                //send email
                //send sms
            }
        }

        public async Task<IEnumerable<Notification>> GetByEmployee(Guid employeeId)
        {
            var empNotification = await _unitOfWork.GetRepository<Notification>().GetAllAsync(predicate: x => x.EmployeeId == employeeId || x.IsGeneral, source => source.OrderByDescending(c => c.CreatedDate));
            return empNotification;
        }

        public async Task<BaseResponse> Delete(Guid id)
        {
            var model = await GetById(id);
            if (model != null)
            {
                _unitOfWork.GetRepository<Notification>().Delete(model);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse() { Status = true, Message = ResponseMessage.DeletedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public async Task<Notification> GetById(Guid id)
        {
            var model = await _unitOfWork.GetRepository<Notification>().GetFirstOrDefaultAsync(predicate: c => c.Id == id);
            return model;
        }
    }
}
