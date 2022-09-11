
using BO.Models;
using BO.Models.Configs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Models.Services.Interfaces
{
    /// <summary>
    /// непосредственно отправка
    /// </summary>
    public interface IEmailServiceSender
    {
        Task SendEmailAsync(string nameFrom, string emailFrom
            , string email, string subject, string message,
            string mailingHost, int mailingPort, string mailingLogin, string mailingPassword);
        Task SendEmailAsync(string email, string subject, string message,
            MailSendingInstanceConfig config);

        Task SendEmailAsync(OneMail oneMail,
            MailSendingInstanceConfig config);
        Task SendEmailAsync(List<OneMail> mails,
            MailSendingInstanceConfig config);

    }

    /// <summary>
    /// интерфейс под каждый тип рассылки. 
    /// </summary>
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task QueueEmailAsync(string email, string subject, string message);
        Task QueueEmailAsync(List<string> email, string subject, string message);
        Task SendQueueAsync();
    }
}
