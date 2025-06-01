using BO.Models.TaskManagementApp.DAL.Domain;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories
{
    public sealed class WorkTaskRepository : GeneralRepository<WorkTask, long>, IWorkTaskRepository
    {
        public WorkTaskRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public async Task<WorkTask> CreateAsync(WorkTask task)
        {
            return await base.AddAsync(task);
        }

        public async Task<List<WorkTask>> GetTasksAsync(long projectId, string name, long? creatorId
            , long? executorId, long? statusId, int pageNumber, int pageSize)
        {
            if (pageNumber > 0)
            {
                pageNumber--;
            }

            var skipCount = pageNumber * pageSize;
            return await _db.TaskManagementTasks.AsNoTracking().Where(x => x.ProjectId == projectId
                && (creatorId == null || x.CreatorId == creatorId)
                && (executorId == null || x.ExecutorId == executorId)
                && (statusId == null || x.StatusId == statusId)
                && (string.IsNullOrWhiteSpace(name) || EF.Functions.Like(x.Name, $"%{name}%"))).OrderBy(x => x.Id)
                .Skip(skipCount).Take(pageSize).ToListAsync();
        }

        public async Task<long> GetTasksCountAsync(long projectId, string name, long? creatorId
            , long? executorId, long? statusId)
        {
            return await _db.TaskManagementTasks.Where(x => x.ProjectId == projectId
                && (creatorId == null || x.CreatorId == creatorId)
                && (executorId == null || x.ExecutorId == executorId)
                && (statusId == null || x.StatusId == statusId)
                && (string.IsNullOrWhiteSpace(name) || EF.Functions.Like(x.Name, $"%{name}%"))).CountAsync();
        }

        public async Task<List<WorkTask>> GetTasksByProjectIdAsync(long projectId)
        {
            return await _db.TaskManagementTasks.AsNoTracking().Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<WorkTask> GetTaskWithCommentsAsync(long id)
        {
            return await _db.TaskManagementTasks.AsNoTracking().Include(x => x.Comments).FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<WorkTask> GetAsync(long id, long projectId)
        {
            return await _db.TaskManagementTasks.AsNoTracking().FirstOrDefaultAsync(x =>
                x.Id == id
                && x.ProjectId == projectId);

        }

        public async Task<bool> ExistAsync(long projectId, long statusId)
        {
            return await _db.TaskManagementTasks.AsNoTracking().AnyAsync(x =>
                x.StatusId == statusId
                && x.ProjectId == projectId);
        }
    }
}
