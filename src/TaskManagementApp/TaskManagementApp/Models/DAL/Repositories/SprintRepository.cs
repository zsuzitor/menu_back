using BL.Models.Services.Interfaces;
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace TaskManagementApp.Models.DAL.Repositories
{
    public class SprintRepository : GeneralRepository<ProjectSprint, long>, ISprintRepository
    {
        private readonly ICacheService _cache;

        public SprintRepository(MenuDbContext db, IGeneralRepositoryStrategy repo
            , ICacheService cache) : base(db, repo)
        {
            _cache = cache;
        }

        public async Task<List<ProjectSprint>> GetForProject(long projectId)
        {
            return await _db.TaskManagementWorkTaskSprint.Where(x => x.ProjectId == projectId)
                .AsNoTracking().ToListAsync();
        }

        public async Task<ProjectSprint> GetWithTasks(long id)
        {
            return await _db.TaskManagementWorkTaskSprint
                .Where(x => x.Id == id).Include(x => x.Tasks).ThenInclude(x => x.Task).FirstOrDefaultAsync();
        }

        public override async Task<ProjectSprint> DeleteAsync(ProjectSprint record)
        {
            using (var t = await _db.Database.BeginTransactionAsync())
            {
                _db.RemoveRange(_db.TaskManagementWorkTaskSprintRelation.Where(x => x.SprintId == record.Id));
                _db.Remove(record);
                await _db.SaveChangesAsync();
                await t.CommitAsync();
            }
            _cache.Remove(Consts.CacheKeys.Project + record.ProjectId);
            return record;
        }

        public async Task<bool> ExistsAsync(long sprintId, long taskId)
        {
            return await _db.TaskManagementWorkTaskSprintRelation.Where(x => x.TaskId == taskId && x.SprintId == sprintId).AnyAsync();
        }

        public async Task<WorkTaskSprintRelation> CreateAsync(WorkTaskSprintRelation obj)
        {
            _db.Add(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> RemoveFromTaskIdExistAsync(long sprintId, long taskId)
        {
            var obj = await _db.TaskManagementWorkTaskSprintRelation.Where(x => x.TaskId == taskId && x.SprintId == sprintId).FirstOrDefaultAsync();
            if (obj == null)
            {
                return false;
            }

            _db.Remove(obj);
            await _db.SaveChangesAsync();
            return true;

        }


        public override async Task<ProjectSprint> AddAsync(ProjectSprint newRecord)
        {
            var result = await base.AddAsync(newRecord);
            _cache.Remove(Consts.CacheKeys.Project + result.ProjectId);
            return result;
        }

        public override async Task<IEnumerable<ProjectSprint>> AddAsync(IEnumerable<ProjectSprint> newRecords)
        {
            var result = await base.AddAsync(newRecords);
            foreach (var record in result.Select(x => x.ProjectId).Distinct())
            {
                _cache.Remove(Consts.CacheKeys.Project + record);
            }
            return result;
        }

        public override async Task<ProjectSprint> UpdateAsync(ProjectSprint record)
        {
            var result = await base.UpdateAsync(record);
            _cache.Remove(Consts.CacheKeys.Project + result.ProjectId);
            return result;
        }

        public override async Task<IEnumerable<ProjectSprint>> UpdateAsync(IEnumerable<ProjectSprint> records)
        {
            var result = await base.UpdateAsync(records);
            foreach (var record in result.Select(x => x.ProjectId).Distinct())
            {
                _cache.Remove(Consts.CacheKeys.Project + record);
            }
            return result;
        }


        public override async Task<IEnumerable<ProjectSprint>> DeleteAsync(IEnumerable<ProjectSprint> records)
        {
            //todo плохо
            foreach (var record in records)
            {
                await this.DeleteAsync(record);
            }
            return records;
        }

        public override async Task<ProjectSprint> DeleteAsync(long recordId)
        {
            ProjectSprint result = null;
            using (var t = await _db.Database.BeginTransactionAsync())
            {
                result = await _db.TaskManagementWorkTaskSprint.FirstOrDefaultAsync(x => x.Id == recordId);
                _db.RemoveRange(_db.TaskManagementWorkTaskSprintRelation.Where(x => x.SprintId == result.Id));
                _db.Remove(result);
                await _db.SaveChangesAsync();
                await t.CommitAsync();
            }
            _cache.Remove(Consts.CacheKeys.Project + result.ProjectId);
            return result;
        }
    }
}
