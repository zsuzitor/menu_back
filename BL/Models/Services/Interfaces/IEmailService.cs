
using BO.Models.Configs;
using System.Threading.Tasks;

namespace BL.Models.Services.Interfaces
{
    public interface IEmailServiceSender
    {
        Task SendEmailAsync(string nameFrom, string emailFrom
            , string email, string subject, string message,
            string mailingHost, int mailingPort, string mailingLogin, string mailingPassword);
        Task SendEmailAsync(string email, string subject, string message,
            MailSendingInstanceConfig config);
        
    }

    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
