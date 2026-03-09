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
    public class PresetCachedRepository : PresetRepository, IPresetCachedRepository
    {

        private readonly ICacheService _cache;
        public PresetCachedRepository(MenuDbContext db,
            IGeneralRepositoryStrategy repo,
            ICacheService cache) : base(db, repo, cache)
        {
            _cache = cache;
        }

        #region GeneralRepositoryCache

        public override async Task<Preset> GetAsync(long id)
        {
            //return await _projectRepository.GetAsync(id);

            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Preset + id,
            async () =>
            {
                return await base.GetNoTrackAsync(id);
            },
            Consts.CacheKeys.CacheTime);
            return result.Item2;
        }

        public override async Task<List<Preset>> GetAsync(List<long> ids)
        {
            return await base.GetAsync(ids);
        }


        public override async Task<Preset> GetNoTrackAsync(long id)
        {
            return await this.GetAsync(id);
        }

        public override async Task<List<Preset>> GetNoTrackAsync(List<long> ids)
        {
            return await base.GetNoTrackAsync(ids);
        }

        public override async Task<bool> ExistAsync(long id)
        {
            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Preset + id,
            async () =>
            {
                return await base.GetNoTrackAsync(id);
            },
            Consts.CacheKeys.CacheTime);
            return result.Item2 != null;
        }
        #endregion GeneralRepositoryCache


        public override async Task<List<Preset>> GetAllAsync(long projectId)
        {
            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.PresetsByProjectId + projectId,
            async () =>
            {
                return await base.GetAllAsync(projectId);
            },
            Consts.CacheKeys.CacheTime);
            return result.Item2;

        }

        public override async Task<Preset> GetWithLabelsAsync(long presetId)
        {
            return await this.GetWithLabelsAsync(presetId);
        }
        public override async Task<List<Preset>> GetWithLabelsForProjectsync(long projectId)
        {
            return await this.GetWithLabelsForProjectsync(projectId);

        }

    }

    public class PresetRepository : GeneralRepository<Preset, long>, IPresetRepository
    {
        private readonly ICacheService _cache;

        public PresetRepository(MenuDbContext db, IGeneralRepositoryStrategy repo
            , ICacheService cache) : base(db, repo)
        {
            _cache = cache;
        }

        public async Task<List<WorkTaskLabelPresetRelation>> DeleteAsync(List<WorkTaskLabelPresetRelation> list)
        {
            _db.RemoveRange(list);
            await _db.SaveChangesAsync();

            return list;
        }

        public virtual async Task<List<Preset>> GetAllAsync(long projectId)
        {
            return await _db.TaskManagementPreset.AsNoTracking().Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public virtual async Task<List<Preset>> GetWithLabelsForProjectsync(long projectId)
        {
            return await _db.TaskManagementPreset.AsNoTracking().Include(x => x.Labels).Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public virtual async Task<Preset> GetWithLabelsAsync(long presetId)
        {
            return await _db.TaskManagementPreset.Include(x => x.Labels).FirstOrDefaultAsync(x => x.Id == presetId);

        }


        public override async Task<Preset> AddAsync(Preset newRecord)
        {
            var result = await base.AddAsync(newRecord);
            _cache.Remove(Consts.CacheKeys.PresetsByProjectId + result.ProjectId);
            return result;
        }

        public override async Task<IEnumerable<Preset>> AddAsync(IEnumerable<Preset> newRecords)
        {
            var result = await base.AddAsync(newRecords);
            foreach (var record in result.Select(x => x.ProjectId).Distinct())
            {
                _cache.Remove(Consts.CacheKeys.PresetsByProjectId + record);
            }
            return result;
        }

        public override async Task<Preset> UpdateAsync(Preset record)
        {
            var result = await base.UpdateAsync(record);
            _cache.Remove(Consts.CacheKeys.PresetsByProjectId + result.ProjectId);
            _cache.Remove(Consts.CacheKeys.Preset + result.Id);
            return result;
        }

        public override async Task<IEnumerable<Preset>> UpdateAsync(IEnumerable<Preset> records)
        {
            var result = await base.UpdateAsync(records);
            foreach (var record in result.Select(x => x.ProjectId).Distinct())
            {
                _cache.Remove(Consts.CacheKeys.PresetsByProjectId + record);
            }

            foreach (var record in result)
            {
                _cache.Remove(Consts.CacheKeys.Preset + record.Id);
            }


            return result;
        }


        public override async Task<Preset> DeleteAsync(Preset record)
        {
            var result = await base.DeleteAsync(record);

            _cache.Remove(Consts.CacheKeys.PresetsByProjectId + result.ProjectId);
            _cache.Remove(Consts.CacheKeys.Preset + result.Id);
            return result;
        }

        public override async Task<IEnumerable<Preset>> DeleteAsync(IEnumerable<Preset> records)
        {
            var result = await base.DeleteAsync(records);
            foreach (var record in result.Select(x => x.ProjectId).Distinct())
            {
                _cache.Remove(Consts.CacheKeys.PresetsByProjectId + record);
            }

            foreach (var record in result)
            {
                _cache.Remove(Consts.CacheKeys.Preset + record.Id);
            }
            return result;
        }

        public override async Task<Preset> DeleteAsync(long recordId)
        {
            var result = await base.DeleteAsync(recordId);

            _cache.Remove(Consts.CacheKeys.PresetsByProjectId + result.ProjectId);
            _cache.Remove(Consts.CacheKeys.Preset + recordId);
            return result;
        }
    }
}
