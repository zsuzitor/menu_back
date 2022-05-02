
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services.Interfaces;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{
    public class TaskReviewService : ITaskReviewService
    {
        private readonly ITaskReviewRepository _taskReviewRepository;
        public TaskReviewService(ITaskReviewRepository taskReviewRepository)
        {
            _taskReviewRepository = taskReviewRepository;
        }

        public async Task<TaskReview> CreateAsync(TaskReview task)
        {
            return await _taskReviewRepository.CreateAsync(task);
        }
    }
}
