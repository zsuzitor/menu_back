using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Migrations;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace TaskManagementApp.Models.DAL.Repositories
{
    public class PresetRepository : GeneralRepository<Preset, long>, IPresetRepository
    {
        public PresetRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public async Task<List<WorkTaskLabelPresetRelation>> DeleteAsync(List<WorkTaskLabelPresetRelation> list)
        {
            _db.RemoveRange(list);
            await _db.SaveChangesAsync();
            return list;
        }

        public async Task<List<Preset>> GetAllAsync(long projectId)
        {
            return await _db.TaskManagementPreset.AsNoTracking().Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<List<Preset>> GetWithLabelsForProjectsync(long projectId)
        {
            return await _db.TaskManagementPreset.AsNoTracking().Include(x => x.Labels).Where(x => x.ProjectId == projectId).ToListAsync();
        }

        public async Task<Preset> GetWithLabelsAsync(long presetId)
        {
            return await _db.TaskManagementPreset.Include(x => x.Labels).FirstOrDefaultAsync(x => x.Id == presetId);

        }
    }
}
