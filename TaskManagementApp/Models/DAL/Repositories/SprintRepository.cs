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
                .Where(x => x.Id == id).Include(x => x.Tasks).FirstOrDefaultAsync();
        }
    }
}
