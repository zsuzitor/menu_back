using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL.Models.Services.Interfaces
{
    /// <summary>
    /// интерфейс под каждый тип рассылки. 
    /// </summary>
    public interface IEmailService
    {
        Task<long> SendEmailAsync(string email, string subject, string message);
        Task<long> QueueEmailAsync(string email, string subject, string message);
        Task<List<long>> QueueEmailAsync(List<string> email, string subject, string message);
        /// <summary>
        /// отправляет то что накопилось
        /// </summary>
        /// <returns></returns>
        Task SendQueueAsync();
        void SendQueue();
    }
}
