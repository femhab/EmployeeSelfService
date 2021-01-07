using System.Threading.Tasks;

namespace Business.Providers
{
    public interface IBulkSmsProvider
    {
        Task SendSms(string recipient, string message);
    }
}
