using BO.Models.TaskManagementApp.DAL.Domain;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories
{
    public class TaskStatusRepository : GeneralRepository<WorkTaskStatus, long>, ITaskStatusRepository
    {
        public TaskStatusRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public async Task<List<WorkTaskStatus>> GetForProjectAsync(long projectId)
        {
            return await _db.TaskManagementTaskStatus.AsNoTracking().Where(x => x.ProjectId == projectId).ToListAsync();
        }
    }
}
