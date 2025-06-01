
using BL.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BL.Models.Services.EmailServiceBase;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface ITaskManagementAppEmailService : IEmailService
    {
        Task QueueNewCommentInTaskAsync(List<string> email, string taskName, string taskUrl);
        Task QueueExecutorInTaskAsync(List<string> email, string taskName, string taskUrl);
        Task QueueChangeStatusTaskAsync(List<string> email, string taskName, string newStatus, string taskUrl);
        Task QueueChangeTaskAsync(List<string> email, string taskName, List<Changes> changes, string taskUrl);

    }
}
