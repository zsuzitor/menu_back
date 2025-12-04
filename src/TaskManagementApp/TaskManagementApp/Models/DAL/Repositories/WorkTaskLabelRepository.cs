using BO.Models.TaskManagementApp.DAL.Domain;
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
    internal class WorkTaskLabelRepository : GeneralRepository<WorkTaskLabel, long>, IWorkTaskLabelRepository
    {
        public WorkTaskLabelRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public async Task<WorkTaskLabelTaskRelation> CreateAsync(WorkTaskLabelTaskRelation obj)
        {
            _db.Add(obj);
            await _db.SaveChangesAsync();
            return obj;
        }

        public async Task<bool> ExistsAsync(long labelId, long taskId)
        {
            return await _db.TaskManagementWorkTaskLabelTaskRelation.Where(x => x.TaskId == taskId && x.LabelId == labelId).AnyAsync();

        }

        public async Task<bool> ExistsAsync(string name, long projectId)
        {
            return await _db.TaskManagementWorkTaskLabel.Where(x => x.ProjectId == projectId && x.Name == name).AnyAsync();

        }

        public async Task<List<WorkTaskLabel>> GetForProjectAsync(long projectId)
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
                return record;
            }
        }

        public async Task<List<WorkTaskLabelTaskRelation>> DeleteAsync(List<WorkTaskLabelTaskRelation> list)
        {
            _db.RemoveRange(list);
            await _db.SaveChangesAsync();
            return list;
        }
    }
}
