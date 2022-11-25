
using BL.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services.Interfaces
{
    public interface IReviewAppEmailService : IEmailService
    {
        Task QueueNewCommentInReviewTaskAsync(string email, string taskName);
        Task QueueNewCommentInReviewTaskAsync(List<string> email, string taskName);
        Task QueueReviewerInReviewTaskAsync(string email, string taskName);
        Task QueueChangeStatusTaskAsync(string email, string taskName, string newStatus);

    }
}
