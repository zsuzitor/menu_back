using BL.Models.Services.Interfaces;
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace TaskManagementApp.Models.DAL.Repositories
{
    public class TaskStatusCachedRepository : TaskStatusRepository, ITaskStatusCachedRepository
    {
        public TaskStatusCachedRepository(MenuDbContext db, IGeneralRepositoryStrategy repo, ICacheService cache) : base(db, repo, cache)
        {
        }

        public override async Task<List<WorkTaskStatus>> GetForProjectAsync(long projectId)
        {

            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.TaskStatusesByProjectId + projectId,
            async () =>
            {
                return await base.GetForProjectAsync(projectId);
            },
            Consts.CacheKeys.CacheTime);
            return result;
        }
    }



    public class TaskStatusRepository : GeneralRepository<WorkTaskStatus, long>, ITaskStatusRepository
    {
        protected readonly ICacheService _cache;
        public TaskStatusRepository(MenuDbContext db, IGeneralRepositoryStrategy repo, ICacheService cache) : base(db, repo)
        {
            _cache = cache;
        }

        public virtual async Task<List<WorkTaskStatus>> GetForProjectAsync(long projectId)
        {
            return await _db.TaskManagementTaskStatus.AsNoTracking().Where(x => x.ProjectId == projectId).ToListAsync();
        }


        public override async Task<WorkTaskStatus> AddAsync(WorkTaskStatus newRecord)
        {
            var result = await base.AddAsync(newRecord);
            await _cache.RemoveAsync(Consts.CacheKeys.TaskStatusesByProjectId + result.ProjectId);
            return result;
        }

        public override async Task<IEnumerable<WorkTaskStatus>> AddAsync(IEnumerable<WorkTaskStatus> newRecords)
        {
            var result = await base.AddAsync(newRecords);
            foreach (var record in result.Select(x => x.ProjectId).Distinct())
            {
                await _cache.RemoveAsync(Consts.CacheKeys.TaskStatusesByProjectId + record);
            }
            return result;
        }

        public override async Task<WorkTaskStatus> UpdateAsync(WorkTaskStatus record)
        {
            var result = await base.UpdateAsync(record);
            await _cache.RemoveAsync(Consts.CacheKeys.TaskStatusesByProjectId + result.ProjectId);
            return result;
        }

        public override async Task<IEnumerable<WorkTaskStatus>> UpdateAsync(IEnumerable<WorkTaskStatus> records)
        {
            var result = await base.UpdateAsync(records);
            foreach (var record in result.Select(x => x.ProjectId).Distinct())
            {
                await _cache.RemoveAsync(Consts.CacheKeys.TaskStatusesByProjectId + record);
            }
            return result;
        }


        public override async Task<WorkTaskStatus> DeleteAsync(WorkTaskStatus record)
        {
            var result = await base.DeleteAsync(record);

            if (result != null)
            {
                await _cache.RemoveAsync(Consts.CacheKeys.TaskStatusesByProjectId + result.ProjectId);
                await _cache.RemoveAsync(Consts.CacheKeys.PresetsByProjectId + result.ProjectId);
            }
            return result;
        }

        public override async Task<IEnumerable<WorkTaskStatus>> DeleteAsync(IEnumerable<WorkTaskStatus> records)
        {
            var result = await base.DeleteAsync(records);
            foreach (var record in result.Select(x => x.ProjectId).Distinct())
            {
                await _cache.RemoveAsync(Consts.CacheKeys.TaskStatusesByProjectId + record);
                await _cache.RemoveAsync(Consts.CacheKeys.PresetsByProjectId + record);
            }
            return result;
        }

        public override async Task<WorkTaskStatus> DeleteAsync(long recordId)
        {
            var result = await base.DeleteAsync(recordId);
            if(result!= null)
            {
                await _cache.RemoveAsync(Consts.CacheKeys.TaskStatusesByProjectId + result.ProjectId);
                await _cache.RemoveAsync(Consts.CacheKeys.PresetsByProjectId + result.ProjectId);
            }
            return result;
        }
    }
}
