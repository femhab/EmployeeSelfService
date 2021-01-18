using Business.Interfaces;
using Business.Providers;
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
        private readonly ISendGridProvider _sendGridProvider;
        private readonly IBulkSmsProvider _bulkSmsProvider;

        public NotificationService(IUnitOfWork unitOfWork, ISendGridProvider sendGridProvider, IBulkSmsProvider bulkSmsProvider)
        {
            _unitOfWork = unitOfWork;
            _sendGridProvider = sendGridProvider;
            _bulkSmsProvider = bulkSmsProvider;
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
            if (sendExtraNotification || !sendExtraNotification)
            {
                var employee = await _unitOfWork.GetRepository<Employee>().GetFirstOrDefaultAsync(predicate: x => x.Id == employeeId);
                if (!string.IsNullOrEmpty(employee.EmailAddress))
                {
                    //send email
                    await _sendGridProvider.SendEmail(employee.EmailAddress, title, message);
                }
                if (!string.IsNullOrEmpty(employee.PhoneNumber))
                {
                    //send sms
                    await _bulkSmsProvider.SendSms(employee.PhoneNumber, message);
                }
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
