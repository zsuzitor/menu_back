using BO.Models.TaskManagementApp.DAL.Domain;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApp.Models.DTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

        public async Task<List<WorkTask>> GetTasksAsync(GetTasksByFilter filters)
        {
            if (filters.PageNumber > 0)
            {
                filters.PageNumber--;
            }
            else
            {
                filters.PageNumber = 0;
            }

            var skipCount = filters.PageNumber * filters.PageSize;
            return await _db.TaskManagementTasks.AsNoTracking().Include(x => x.Sprints).Where(x => x.ProjectId == filters.ProjectId
                && (filters.CreatorId == null || x.CreatorId == filters.CreatorId)
                && (filters.ExecutorId == null || x.ExecutorId == filters.ExecutorId)
                && (filters.StatusId == null || x.StatusId == filters.StatusId)
                && (filters.SprintId == null || x.Sprints.Any(s => s.SprintId == filters.SprintId))
            //&& (string.IsNullOrWhiteSpace(filters.Name) || EF.Functions.Like(x.Name, $"%{filters.Name}%"))).OrderBy(x => x.Id)
                && (string.IsNullOrWhiteSpace(filters.Name) || x.Name.Contains(filters.Name))).OrderBy(x => x.Id)
                .Skip(skipCount).Take(filters.PageSize).ToListAsync();
        }

        public async Task<long> GetTasksCountAsync(GetTasksCountByFilter filters)
        {
            return await _db.TaskManagementTasks.Include(x => x.Sprints).Where(x => x.ProjectId == filters.ProjectId
                && (filters.CreatorId == null || x.CreatorId == filters.CreatorId)
                && (filters.ExecutorId == null || x.ExecutorId == filters.ExecutorId)
                && (filters.StatusId == null || x.StatusId == filters.StatusId)
                && (filters.SprintId == null || x.Sprints.Any(s => s.SprintId == filters.SprintId))
                //&& (string.IsNullOrWhiteSpace(filters.Name) || EF.Functions.Like(x.Name, $"%{filters.Name}%"))
                && (string.IsNullOrWhiteSpace(filters.Name) || x.Name.Contains(filters.Name))
                ).CountAsync();
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

        public async Task<bool> HaveAccessAsync(long taskId, long mainAppUserId)
        {
            return (await GetAccessedIQuer(taskId, mainAppUserId).Select(x => x.Id).FirstOrDefaultAsync()) > 0;
            //return (await _db.TaskManagementTasks.AsNoTracking().Include(x => x.Project).ThenInclude(x => x.Users)
            //    .Where(x => x.Id == taskId && x.Project.Users.Any(pu => pu.MainAppUserId == mainAppUserId))
            //    .Select(x => x.Id).FirstOrDefaultAsync()) > 0;
        }

        public async Task<WorkTask> GetAccessAsync(long taskId, long mainAppUserId)
        {
            return (await GetAccessedIQuer(taskId, mainAppUserId).Select(x => x).FirstOrDefaultAsync());
        }

        private IQueryable<WorkTask> GetAccessedIQuer(long taskId, long mainAppUserId)
        {
            return _db.TaskManagementTasks.AsNoTracking().Include(x => x.Project).ThenInclude(x => x.Users)
                .Where(x => x.Id == taskId && x.Project.Users.Any(pu => pu.MainAppUserId == mainAppUserId))
                ;//.Select(x => x);
        }

        public async Task<long> GetUserIdAccessAsync(long taskId, long mainAppUserId)
        {
            var res = (await GetAccessedIQuer(taskId, mainAppUserId)
                .Select(x => new { x.Id, User = x.Project.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId) }).FirstOrDefaultAsync());
            if (res.Id > 0)
            {
                return res.User?.Id ?? 0;
            }

            return 0;
        }

        public async Task<WorkTask> GetTaskFullAsync(long id)
        {
            return await _db.TaskManagementTasks.AsNoTracking()
                .Include(x => x.Comments).Include(x => x.Sprints).Include(x => x.Labels)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WorkTask> GetWithSprintRelationAsync(long id)
        {
            return await _db.TaskManagementTasks.Where(x => x.Id == id).Include(x => x.Sprints).FirstOrDefaultAsync();
        }

        public async Task<WorkTask> GetWithLabelRelationAsync(long id)
        {
            return await _db.TaskManagementTasks.Where(x => x.Id == id).Include(x => x.Labels).FirstOrDefaultAsync();
        }
    }
}
