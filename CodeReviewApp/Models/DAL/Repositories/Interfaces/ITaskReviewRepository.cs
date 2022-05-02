
using BO.Models.CodeReviewApp.DAL.Domain;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories.Interfaces
{
    public interface ITaskReviewRepository
    {
        Task<TaskReview> CreateAsync(TaskReview task);
    }
}
