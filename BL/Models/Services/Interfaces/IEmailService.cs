
using System.Threading.Tasks;

namespace BL.Models.Services.Interfaces
{
    public interface IEmailServiceSender
    {
        Task SendEmailAsync(string nameFrom, string emailFrom
            , string email, string subject, string message,
            string mailingHost, int mailingPort, string mailingLogin, string mailingPassword);
    }

    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
