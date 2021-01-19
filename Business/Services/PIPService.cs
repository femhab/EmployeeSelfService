using Business.Interfaces;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Services
{
    public class PIPService: IPIPService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;

        public PIPService(IUnitOfWork unitOfWork, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task<BaseResponse> CreatePIP(PIP model)
        {
            _unitOfWork.GetRepository<PIP>().Insert(model);
            await _unitOfWork.SaveChangesAsync();
            await _notificationService.CreateNotification(NotificationAction.PIPCreateTitle, NotificationAction.PIPCreateMessage, model.EmployeeId, false, false);
            
            return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
        }

        public async Task<BaseResponse> CreatePIPItem(PIPItem model)
        {
            _unitOfWork.GetRepository<PIPItem>().Insert(model);
            await _unitOfWork.SaveChangesAsync();
            return new BaseResponse() { Status = true, Message = ResponseMessage.CreatedSuccessful };
        }

        public async Task<IEnumerable<PIP>> GetByEmployee(string employeeNo)
        {
            var data = await _unitOfWork.GetRepository<PIP>().GetAllAsync(predicate: x => x.Emp_No.ToLower() == employeeNo.ToLower() || x.LineManager == employeeNo.ToLower(), null, "Employee");
            return data;
        }

        public async Task<IEnumerable<PIPItem>> GetByPIP(Guid pipId)
        {
            var data = await _unitOfWork.GetRepository<PIPItem>().GetAllAsync(predicate: x => x.PIPId == pipId);
            return data;
        }

        public async Task<PIP> GetById(Guid id)
        {
            var data = await _unitOfWork.GetRepository<PIP>().GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            return data;
        }

        public async Task<BaseResponse> ClosePIP(Guid pipId)
        {
            var data = await _unitOfWork.GetRepository<PIP>().GetFirstOrDefaultAsync(predicate: x => x.Id == pipId);
            if(data != null)
            {
                data.IsClosed = true;
                _unitOfWork.GetRepository<PIP>().Update(data);
                await _unitOfWork.SaveChangesAsync();
            }
            return new BaseResponse { Status = true, Message = ResponseMessage.UpdatedSuccessful };
        }
    }
}
