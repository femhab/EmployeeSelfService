using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Business.Providers
{
    public class EmailHelper
    {
        /// <summary>
        /// Send the email
        /// </summary>
        /// <param name="message">Message to be sent using BuildMessage()</param>
        /// <param name="isHtml">Set to true if message body is HTML</param>
        public static void Send(MailMessage message, bool isHtml = false, bool enableSsl = false)
        {
            message.IsBodyHtml = isHtml;
            SmtpClient smtp = new SmtpClient();
            smtp.EnableSsl = enableSsl;
            smtp.Send(message);
            smtp.Dispose();
            message.Dispose();
        }

        /// <summary>
        /// Send the email
        /// </summary>
        /// <param name="config">An <see cref="EmailConfig"/> settings</param>
        /// <param name="message">Message to be sent using BuildMessage()</param>
        /// <param name="isHtml">Set to true if message body is HTML</param>
        public static void Send(EmailConfig config, MailMessage message, bool isHtml = false, bool enableSsl = false)
        {
            message.IsBodyHtml = isHtml;
            SmtpClient smtp = new SmtpClient(config.Smtp.Host, config.Smtp.Port);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(config.Smtp.User, config.Smtp.Pass);
            smtp.EnableSsl = enableSsl;
            smtp.Send(message);
            smtp.Dispose();
            message.Dispose();
        }

        /// <summary>
        /// Send the email using async await
        /// </summary>
        /// <param name="message">Message to be sent using BuildMessage()</param>
        /// <param name="isHtml">Set to true if message body is HTML</param>
        public async static Task SendAsync(MailMessage message, bool isHtml = false, bool enableSsl = false)
        {
            message.IsBodyHtml = isHtml;
            message.Priority = MailPriority.High;
            using (var smtp = new SmtpClient())
            {
                smtp.EnableSsl = enableSsl;
                await smtp.SendMailAsync(message);
            }
            message.Dispose();
        }

        /// <summary>
        /// Send the email using async await
        /// </summary>
        /// <param name="config">An <see cref="EmailConfig"/> settings</param>
        /// <param name="message">Message to be sent using BuildMessage()</param>
        /// <param name="isHtml">Set to true if message body is HTML</param>
        /// <returns></returns>
        public async static Task SendAsync(EmailConfig config, MailMessage message, bool isHtml = false, bool enableSsl = false)
        {
            message.IsBodyHtml = isHtml;
            message.Priority = MailPriority.High;
            using (var smtp = new SmtpClient(config.Smtp.Host, config.Smtp.Port))
            {
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(config.Smtp.User, config.Smtp.Pass);
                smtp.EnableSsl = enableSsl;
                try
                {
                    await smtp.SendMailAsync(message);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
                message.Dispose();
            }
        }

        /// <summary>
        /// Build the email message using template, view data, and other email parameter
        /// </summary>
        /// <param name="sender">Email from</param>
        /// <param name="subject">Subject of the mail</param>
        /// <param name="recipient">recipient email address</param>
        /// <param name="messageBody">Main Body of the message if available</param>
        /// <param name="template">Template file to read email from. (.html or .txt)</param>
        /// <param name="viewData">Dynamic view data to be binded to the template</param>
        /// <param name="Cc">Carbon copy email addresses. Separate multiple by comma(",")</param>
        /// <param name="Bcc">Blind copy email addresses. Separate multiple by comma(",")</param>
        /// <returns>returns MailMessage object ready to be sent</returns>
        public static MailMessage BuildMessage(string sender, string subject, string recipient, string messageBody, string template = null, string[] viewData = null, string Cc = null, string Bcc = null)
        {
            MailMessage message = new MailMessage(sender, recipient, subject, ComposeMessage(subject, messageBody, template, viewData));

            //Email carbon copy
            if (!string.IsNullOrEmpty(Cc))
            {
                string[] copy = Cc.ToLower().Split(',');
                foreach (var item in copy)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        message.CC.Add(item);
                    }
                }
            }

            //Email blind copy
            if (!string.IsNullOrEmpty(Bcc))
            {
                string[] blindCopy = Bcc.ToLower().Split(',');
                foreach (var item in blindCopy)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        message.Bcc.Add(item);
                    }
                }
            }
            return message;
        }

        /// <summary>
        /// Compose the email message using template and view data
        /// </summary>
        /// <param name="subject">Subject of the mail</param>
        /// <param name="message">Email Message if template is not available</param>
        /// <param name="template">Template file to read email from. (.html or .txt)</param>
        /// <param name="viewData">Dynamic view data to be binded to the template</param>
        /// <returns>Returns the composed message string</returns>
        public static string ComposeMessage(string subject, string message, string template = null, string[] viewData = null)
        {
            //use message as body by default
            string body = message;
            //use template if available
            if (!string.IsNullOrEmpty(template))
            {
                //var sr = new StreamReader(string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, template))
                using (var sr = new StreamReader(template))
                {
                    body = sr.ReadToEnd();
                }
            }
            //replace static content
            body = body.Replace("{{Subject}}", subject);
            body = body.Replace("{{Message}}", message);
            body = body.Replace("{{Date}}", DateTime.Now.ToShortDateString());
            body = body.Replace("{{Year}}", DateTime.Now.Year.ToString());

            //asign view data to the template
            if (viewData != null && viewData.Length > 0)
            {
                foreach (var item in viewData)
                {
                    string[] data = item.Split(new char[] { ':' }, 2);
                    if (!string.IsNullOrEmpty(data[0]))
                    {
                        body = body.Replace("{{" + data[0] + "}}", data[1]);
                    }
                }
            }
            return body;
        }
    }
}
