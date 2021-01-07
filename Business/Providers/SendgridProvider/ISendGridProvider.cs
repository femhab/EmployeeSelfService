using System.IO;
using System.Threading.Tasks;

namespace Business.Providers
{
    public interface ISendGridProvider
    {
        Task SendEmail(string recipient, string subject, string message, string templateString = null, Stream attachment = null, string attachmentName = null);
    }
}
