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
    public class WorkTaskLabelCachedRepository : WorkTaskLabelRepository, IWorkTaskLabelCachedRepository
    {
        public WorkTaskLabelCachedRepository(MenuDbContext db, IGeneralRepositoryStrategy repo, ICacheService cache) : base(db, repo, cache)
        {
        }


        public override async Task<bool> ExistsAsync(string name, long projectId)
        {
            var labels = await this.GetForProjectAsync(projectId);
            return labels.Any(x => x.Name == name);

        }


        public override async Task<List<WorkTaskLabel>> GetForProjectAsync(long projectId)
        {

            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.TaskLabelsByProjectId + projectId,
            async () =>
            {
                return await base.GetForProjectAsync(projectId);
            },
            Consts.CacheKeys.CacheTime);
            return result;
        }

    }



    public class WorkTaskLabelRepository : GeneralRepository<WorkTaskLabel, long>, IWorkTaskLabelRepository
    {
        protected readonly ICacheService _cache;
        public WorkTaskLabelRepository(MenuDbContext db, IGeneralRepositoryStrategy repo, ICacheService cache) : base(db, repo)
        {
            _cache = cache;
        }

        public async Task<WorkTaskLabelTaskRelation> CreateAsync(WorkTaskLabelTaskRelation obj)
        {
            _db.Add(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public virtual async Task<bool> ExistsAsync(long labelId, long taskId)
        {
            return await _db.TaskManagementWorkTaskLabelTaskRelation.Where(x => x.TaskId == taskId && x.LabelId == labelId).AnyAsync();

        }

        public virtual async Task<bool> ExistsAsync(string name, long projectId)
        {
            return await _db.TaskManagementWorkTaskLabel.Where(x => x.ProjectId == projectId && x.Name == name).AnyAsync();

        }

        public virtual async Task<List<WorkTaskLabel>> GetForProjectAsync(long projectId)
        {
            return await _db.TaskManagementWorkTaskLabel.Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<List<WorkTaskLabel>> GetForTaskAsync(long taskId)
        {
            return await _db.TaskManagementWorkTaskLabelTaskRelation.Include(x => x.Label).Where(x => x.TaskId == taskId).Select(x => x.Label).ToListAsync();
        }

        public async Task<bool> RemoveFromTaskIdExistAsync(long labelId, long taskId)
        {
            var obj =  await _db.TaskManagementWorkTaskLabelTaskRelation.Where(x => x.TaskId == taskId && x.LabelId == labelId).FirstOrDefaultAsync();
            if (obj == null)
            {
                return false;
            }

            _db.Remove(obj);
            await _db.SaveChangesAsync();
            return true;

        }

        public override async Task<WorkTaskLabel> DeleteAsync(WorkTaskLabel record)
        {
            using (var t = await _db.Database.BeginTransactionAsync())
            {
                _db.RemoveRange(_db.TaskManagementWorkTaskLabelTaskRelation.Where(x => x.LabelId == record.Id));
                _db.Remove(record);
                await _db.SaveChangesAsync();
                await t.CommitAsync();
                await _cache.RemoveAsync(Consts.CacheKeys.TaskLabelsByProjectId + record.ProjectId);
                return record;
            }
        }


        public override async Task<IEnumerable<WorkTaskLabel>> DeleteAsync(IEnumerable<WorkTaskLabel> records)
        {
            //todo плохо
            foreach (var record in records)
            {
                await this.DeleteAsync(record);
            }
            return records;
        }

        public override async Task<WorkTaskLabel> DeleteAsync(long recordId)
        {
            var result = await base.DeleteAsync(recordId);

            if (result != null)
            {
                await _cache.RemoveAsync(Consts.CacheKeys.TaskLabelsByProjectId + result.ProjectId);
            }
            return result;
        }


        public async Task<List<WorkTaskLabelTaskRelation>> DeleteAsync(List<WorkTaskLabelTaskRelation> list)
        {
            _db.RemoveRange(list);
            await _db.SaveChangesAsync();
            return list;
        }



        public override async Task<WorkTaskLabel> AddAsync(WorkTaskLabel newRecord)
        {
            var result = await base.AddAsync(newRecord);
            await _cache.RemoveAsync(Consts.CacheKeys.TaskLabelsByProjectId + result.ProjectId);
            return result;
        }

        public override async Task<IEnumerable<WorkTaskLabel>> AddAsync(IEnumerable<WorkTaskLabel> newRecords)
        {
            var result = await base.AddAsync(newRecords);
            foreach (var record in result.Select(x => x.ProjectId).Distinct())
            {
                await _cache.RemoveAsync(Consts.CacheKeys.TaskLabelsByProjectId + record);
            }
            return result;
        }

        public override async Task<WorkTaskLabel> UpdateAsync(WorkTaskLabel record)
        {
            var result = await base.UpdateAsync(record);
            await _cache.RemoveAsync(Consts.CacheKeys.TaskLabelsByProjectId + result.ProjectId);
            return result;
        }

        public override async Task<IEnumerable<WorkTaskLabel>> UpdateAsync(IEnumerable<WorkTaskLabel> records)
        {
            var result = await base.UpdateAsync(records);
            foreach (var record in result.Select(x => x.ProjectId).Distinct())
            {
                await _cache.RemoveAsync(Consts.CacheKeys.TaskLabelsByProjectId + record);
            }


            return result;
        }
    }
}
