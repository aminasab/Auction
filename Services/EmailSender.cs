using OnlineAuction.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace OnlineAuction.Services
{
    public class EmailSender
    {
        private readonly EmailConfig _emailConfig;

        public EmailSender(IOptions<EmailConfig> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }

        public async Task SendEmailAsync(string recipientEmail, string message)
        {
            try
            {
                using (var smtpClient = new SmtpClient(_emailConfig.SmtpServer, _emailConfig.SmtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(_emailConfig.Email, _emailConfig.AppPassword);
                    smtpClient.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_emailConfig.Email),
                        Body = message,
                        IsBodyHtml = true
                    };
                    mailMessage.To.Add(recipientEmail);
                    await smtpClient.SendMailAsync(mailMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отправке сообщения: {ex.Message}");
            }
        }
    }

}
