using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using GenX.TaskEasyTool.Service.Interface;

namespace GenX.TaskEasyTool.Service.Service
{
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public void Send(string toEmail, string subject, string body)
        {

            var fromEmail = _config["Email:FromEmail"];
            var password = _config["Email:Password"];
            var smtpHost = _config["Email:Host"];
            var port = int.Parse(_config["Email:Port"]);
            var enableSsl = bool.Parse(_config["Email:EnableSsl"]);

            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = port,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = enableSsl
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            mailMessage.To.Add(toEmail);

            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (SmtpException ex)
            {
                throw new Exception("Email sending failed. Check SMTP credentials or internet access.", ex);
            }
        }
    }
}
