using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<TaskReview>> GetTasksAsync(long projectId, string name, long? creatorId
            , long? reviewerId, CodeReviewTaskStatus? status, int pageNumber, int pageSize)
        {
            if (pageNumber > 0)
            {
                pageNumber--;
            }

            var skipCount = pageNumber * pageSize;
            return await _db.ReviewTasks.Where(x => x.ProjectId == projectId
                && (creatorId == null || x.CreatorId == creatorId)
                && (reviewerId == null || x.ReviewerId == reviewerId)
                && (status == null || x.Status == status)
                && EF.Functions.Like(x.Name, $"%{name}%")).OrderBy(x => x.Id)
                .Skip(skipCount).Take(pageSize).ToListAsync();
        }

        public async Task<List<TaskReview>> GetTasksByProjectIdAsync(long projectId)
        {
            return await _db.ReviewTasks.Where(x => x.ProjectId == projectId).ToListAsync();
        }


    }
}
