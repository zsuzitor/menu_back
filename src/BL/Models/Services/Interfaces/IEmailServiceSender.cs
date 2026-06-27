
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
        Task<bool> SendEmailAsync(string nameFrom, string emailFrom
            , string email, string subject, string message,
            string mailingHost, int mailingPort, string mailingLogin, string mailingPassword);
        Task<bool> SendEmailAsync(string email, string subject, string message,
            MailSendingInstanceConfig config);

        Task<bool> SendEmailAsync(OneMail oneMail,
            MailSendingInstanceConfig config);

        /// <summary>
        /// отправить письма
        /// </summary>
        /// <param name="mails"></param>
        /// <param name="config"></param>
        /// <returns>Письма отправка которых завершилась с ошибкой</returns>
        Task<List<long>> SendEmailAsync(List<OneMail> mails,
            MailSendingInstanceConfig config);

    }

    /// <summary>
    /// интерфейс под каждый тип рассылки. 
    /// </summary>
    public interface IEmailService
    {
        Task<long> SendEmailAsync(string email, string subject, string message);
        Task ReSendEmailAsync(long notificationId);
        Task<long> QueueEmailAsync(string email, string subject, string message);
        Task<List<long>> QueueEmailAsync(List<string> email, string subject, string message);
        /// <summary>
        /// отправляет то что накопилось
        /// </summary>
        /// <returns></returns>
        Task SendQueueAsync();
    }
}
