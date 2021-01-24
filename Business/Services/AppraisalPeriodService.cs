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
    public class AppraisalPeriodService: IAppraisalPeriodService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmployeeService _employeeService;
        private readonly INotificationService _notificationService;

        public AppraisalPeriodService(IUnitOfWork unitOfWork, IEmployeeService employeeService, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _employeeService = employeeService;
            _notificationService = notificationService;
        }

        public async Task<BaseResponse> Create(DateTime startDate, DateTime enddate)
        {
            var check = await _unitOfWork.GetRepository<AppraisalPeriod>().GetFirstOrDefaultAsync(predicate: x => x.StartDate.Year == startDate.Year);
            if(check == null)
            {
                var otherUpdates = await _unitOfWork.GetRepository<AppraisalPeriod>().GetAllAsync(x => x.IsActive);
                if(otherUpdates != null)
                {
                    foreach(var item in otherUpdates)
                    {
                        item.IsActive = false;
                        _unitOfWork.GetRepository<AppraisalPeriod>().Update(item);
                    }
                }
                var appraisalPeriod = new AppraisalPeriod() { StartDate = startDate, EndDate = enddate, IsActive = true, Id =Guid.NewGuid(), CreatedDate = DateTime.Now };
                _unitOfWork.GetRepository<AppraisalPeriod>().Insert(appraisalPeriod);
                await _unitOfWork.SaveChangesAsync();

                var employees = await _employeeService.GetAll();
                foreach(var item in employees)
                {
                    if (string.IsNullOrEmpty(item.EmailAddress))
                    {
                        await _notificationService.CreateNotification(NotificationAction.AppraisalPeriodCreateTitle, NotificationAction.AppraisalPeriodCreateMessage + startDate.ToString("dd MMMM yyyy") + "to " + enddate.ToString("dd MMMM yyyy"), item.Id, false, true);
                    }
                }

                return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }

        public Task<BaseResponse> UpdateStatus(Guid id, bool IsActive)
        {
            throw new NotImplementedException();
        }

        public async Task<AppraisalPeriod> GetActivePeriod()
        {
            var data = await _unitOfWork.GetRepository<AppraisalPeriod>().GetFirstOrDefaultAsync(predicate: x => x.IsActive);
            return data;
        }

        public async Task<IEnumerable<AppraisalPeriod>> GetAll()
        {
            var data = await _unitOfWork.GetRepository<AppraisalPeriod>().GetAllAsync();
            return data;
        }

        public async Task<BaseResponse> UpdateDate(Guid id, bool status, DateTime? startDate, DateTime? enddate)
        {
            var data = await _unitOfWork.GetRepository<AppraisalPeriod>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            if(data != null)
            {
                if(status == true)
                {
                    var otherUpdates = await _unitOfWork.GetRepository<AppraisalPeriod>().GetAllAsync(x => x.IsActive);
                    if (otherUpdates != null)
                    {
                        foreach (var item in otherUpdates.Where(x => x.Id != id))
                        {
                            item.IsActive = false;
                            _unitOfWork.GetRepository<AppraisalPeriod>().Update(item);
                        }
                    }
                }

                data.StartDate = startDate.Value;
                data.EndDate = enddate.Value;
                data.IsActive = status;

                _unitOfWork.GetRepository<AppraisalPeriod>().Update(data);
                await _unitOfWork.SaveChangesAsync();

                var employees = await _employeeService.GetAll();
                foreach (var item in employees)
                {
                    if (string.IsNullOrEmpty(item.EmailAddress))
                    {
                        await _notificationService.CreateNotification(NotificationAction.AppraisalPeriodEditTitle, NotificationAction.AppraisalPeriodEditMessage + startDate?.ToString("dd MMMM yyyy") + "to " + enddate?.ToString("dd MMMM yyyy"), item.Id, false, true);
                    }
                }

                return new BaseResponse() { Status = true, Message = ResponseMessage.OperationSuccessful };
            }
            return new BaseResponse() { Status = false, Message = ResponseMessage.OperationFailed };
        }
    }
}
