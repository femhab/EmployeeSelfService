using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<(bool status, string message, string token, string refreshToken)> Register(string email, string password, string lastName, string firstName, string userName, string empNo, string createdBy = null);
        Task<(bool status, string message, string token, string refreshToken)> Login(string email, string password);
        Task<string> Logout();
    }
}
