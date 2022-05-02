using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories
{
    public class TaskReviewRepository : GeneralRepository<TaskReview, long>, ITaskReviewRepository
    {
        public TaskReviewRepository(MenuDbContext db) : base(db)
        {
        }

        public async Task<TaskReview> CreateAsync(TaskReview task)
        {
            return await base.AddAsync(task);
        }
    }
}
