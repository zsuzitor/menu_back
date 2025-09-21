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
    public class SprintRepository : GeneralRepository<ProjectSprint, long>, ISprintRepository
    {
        public SprintRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
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
                return record;
            }
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
    }
}
