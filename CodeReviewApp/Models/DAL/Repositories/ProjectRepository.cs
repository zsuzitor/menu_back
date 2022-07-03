
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
    public sealed class ProjectRepository : GeneralRepository<Project, long>, IProjectRepository
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

        public async Task<(bool access, bool isAdmin)> ExistIfAccessAsync(long id, long mainAppUserId)
        {
            var user = await _db.ReviewProject.AsNoTracking().Include(x => x.Users)
                .Where(x => x.Id == id && !x.IsDeleted
                    && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId && !u.Deactivated) != null)
                .Select(x => x.Users.FirstOrDefault()).FirstOrDefaultAsync();
            //user.IsAdmin;
            //.FirstOrDefaultAsync() != null;
            if (user == null)
            {
                return (false, false);
            }

            return (true, user.IsAdmin);
        }

        public async Task<Project> GetByIdIfAccessAsync(long id, long mainAppUserId)
        {
            //todo загрузит скорее всего с пользаками
            return await _db.ReviewProject.AsNoTracking().Include(x => x.Users)
                            .Where(x => x.Id == id && !x.IsDeleted
                                && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId) != null).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistIfAccessAdminAsync(long id, long mainAppUserId)
        {
            return await _db.ReviewProject.AsNoTracking().Include(x => x.Users)
                .Where(x => x.Id == id && !x.IsDeleted
                    && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId
                    && u.IsAdmin && !u.Deactivated) != null).Select(x => x.Name).FirstOrDefaultAsync() != null;
        }

        public async Task<Project> GetByIdIfAccessAdminAsync(long id, long mainAppUserId)
        {
            //todo загрузит скорее всего с пользаками
            return await _db.ReviewProject.AsNoTracking().Include(x => x.Users)
                            .Where(x => x.Id == id && !x.IsDeleted
                                && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId
                                && u.IsAdmin && !u.Deactivated) != null).FirstOrDefaultAsync();
        }


        public override async Task<Project> DeleteAsync(Project project)
        {
            _db.ReviewProject.Attach(project);
            project.IsDeleted = true;
            await _db.SaveChangesAsync();
            return project;
        }

        public override async Task<List<Project>> DeleteAsync(List<Project> records)
        {
            _db.ReviewProject.AttachRange(records);
            foreach (var item in records)
            {
                item.IsDeleted = true;
            }

            await _db.SaveChangesAsync();
            return records;
        }

        public override async Task<Project> DeleteAsync(long recordId)
        {
            var project = await GetAsync(recordId);
            if (project != null)
            {
                project.IsDeleted = false;
                await _db.SaveChangesAsync();
            }

            return project;
        }


        

       

        public async Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId)
        {
            return await _db.ReviewProjectUsers.AsNoTracking().Where(x => x.MainAppUserId == userId)
                .Include(x => x.Project).Select(x => x.Project).Where(x => !x.IsDeleted).ToListAsync();

            //.Join(_db.ReviewProject,u=>u.ProjectId)

        }
    }
}
