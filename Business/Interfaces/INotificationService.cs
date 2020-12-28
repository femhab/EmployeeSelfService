using Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface INotificationService
    {
        Task CreateNotification(string title, string message, Guid? employeeId = null, bool isGeneral = false, bool sendExtraNotification = false);
        Task<IEnumerable<Notification>> GetByEmployee(Guid employeeId);
        Task<BaseResponse> Delete(Guid id);
    }
}
