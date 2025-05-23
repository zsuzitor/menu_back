using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories
{
    public class TaskStatusRepository : GeneralRepository<TaskReviewStatus, long>, ITaskStatusRepository
    {
        public TaskStatusRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public async Task<List<TaskReviewStatus>> GetForProjectAsync(long projectId)
        {
            return await _db.TaskReviewStatus.AsNoTracking().Where(x => x.ProjectId == projectId).ToListAsync();
        }
    }
}
