using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Business.Providers
{
    public class SendGridProvider : ISendGridProvider
    {
        private readonly EmailConfig _emailConfig;
        private readonly IConfiguration _configuration;

        public SendGridProvider(IOptions<EmailConfig> emailConfig, IConfiguration configuration)
        {
            _emailConfig = emailConfig.Value;
            _configuration = configuration;
            _configuration.GetSection("EmailConfig").Bind(_emailConfig);
        }

        public async Task SendEmail(string recipient, string subject, string message, string templateString = null, Stream attachment = null, string attachmentName = null)
        {
            try
            {
                SendGridClient client = new SendGridClient(_emailConfig.Smtp.Pass);
                EmailAddress mailFrom = new EmailAddress(_emailConfig.Sender, _emailConfig.Alias);
                EmailAddress mailTo = new EmailAddress(recipient, "");

                //string compose = EmailHelper.ComposeMessage(subject, message);
                SendGridMessage msg = MailHelper.CreateSingleEmail(mailFrom, mailTo, subject, message, string.Empty);

                Response response = await client.SendEmailAsync(msg);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
