
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories
{
    public class ProjectRepository : GeneralRepository<Project, long>, IProjectRepository
    {

        public ProjectRepository(MenuDbContext db) : base(db)
        {
        }

        public async Task<Project> CreateAsync(string name, ProjectUser user)
        {
            var newProject = new Project() { Name = name, Users = new List<ProjectUser>() { user } };

            await base.AddAsync(newProject);
            return newProject;

        }

        public async Task<bool> ExistIfAccessAsync(long id, long mainAppUserId)
        {
            return await _db.ReviewProject.Include(x => x.Users)
                .Where(x => x.Id == id && !x.IsDeleted
                && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId) != null).FirstOrDefaultAsync() != null;
        }

        public async Task<bool> ExistIfAccessAdminAsync(long id, long mainAppUserId)
        {
            return await _db.ReviewProject.Include(x => x.Users)
                .Where(x => x.Id == id && !x.IsDeleted
                && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId
                    && u.IsAdmin) != null).FirstOrDefaultAsync() != null;
        }



        public override async Task<Project> DeleteAsync(Project project)
        {
            _db.ReviewProject.Attach(project);
            project.IsDeleted = true;
            await _db.SaveChangesAsync();
            return project;
        }

        public async Task<Project> GetByIdIfAccessAsync(long id, long mainAppUserId)
        {
            return await _db.ReviewProject.AsNoTracking().Include(x => x.Users)
                            .Where(x => x.Id == id && !x.IsDeleted
                            && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId) != null).FirstOrDefaultAsync();
        }

        public async Task<Project> GetByIdIfAccessAdminAsync(long id, long mainAppUserId)
        {
            return await _db.ReviewProject.AsNoTracking().Include(x => x.Users)
                            .Where(x => x.Id == id && !x.IsDeleted
                            && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId
                                && u.IsAdmin) != null).FirstOrDefaultAsync();
        }

        public async Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId)
        {
            return await _db.ReviewProjectUsers.AsNoTracking().Where(x => x.MainAppUserId == userId)
                .Include(x => x.Project).Select(x => x.Project).Where(x => !x.IsDeleted).ToListAsync();

            //.Join(_db.ReviewProject,u=>u.ProjectId)

        }
    }
}
