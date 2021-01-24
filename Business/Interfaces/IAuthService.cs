using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModel.ResponseModel;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<(bool status, string message, string token, string refreshToken)> Register(string email, string password, string lastName, string firstName, string userName, string empNo, List<Guid> roleId, string createdBy = null);
        Task<(bool status, string message, string token, string refreshToken)> Login(string email, string password);
        Task<string> Logout();
        Task DeleteUser(Guid empId);
        Task<BaseResponse> ChangePassword(string empNo, string oldPassword, string newPassword);
    }
}
