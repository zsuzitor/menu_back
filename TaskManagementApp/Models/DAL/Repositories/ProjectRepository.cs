
using BO.Models.TaskManagementApp.DAL.Domain;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories
{
    public sealed class ProjectRepository : GeneralRepository<Project, long>, IProjectRepository
    {

        public ProjectRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
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
            var user = await _db.TaskManagementProjectUsers.AsNoTracking().Include(x => x.Project)
                .Where(u => u.MainAppUserId == mainAppUserId && !u.Deactivated
                && u.Project.Id == id && !u.Project.IsDeleted)
                .Select(x => new { x.Id, x.IsAdmin }).FirstOrDefaultAsync();

            //var user = await _db.TaskManagementTaskProject.AsNoTracking().Include(x => x.Users)
            //    .Where(x => x.Id == id && !x.IsDeleted
            //        && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId && !u.Deactivated) != null)
            //    .Select(x => x.Users.FirstOrDefault()).FirstOrDefaultAsync();
            //user.IsAdmin;
            //.FirstOrDefaultAsync() != null;
            if (user == null || user.Id == 0)
            {
                return (false, false);
            }

            return (true, user.IsAdmin);
        }

        public async Task<Project> GetByIdIfAccessAsync(long id, long mainAppUserId)
        {

            return await _db.TaskManagementProjectUsers.AsNoTracking().Include(x => x.Project)
                .Where(u => u.MainAppUserId == mainAppUserId && !u.Deactivated
                && u.Project.Id == id && !u.Project.IsDeleted)
                .Select(x => x.Project).FirstOrDefaultAsync();

            //todo загрузит скорее всего с пользаками
            //return await _db.TaskManagementTaskProject.AsNoTracking().Include(x => x.Users)
            //                .Where(x => x.Id == id && !x.IsDeleted
            //                    && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId && !u.Deactivated) != null).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistIfAccessAdminAsync(long id, long mainAppUserId)
        {
            return (await ExistIfAccessAsync(id, mainAppUserId)).isAdmin;
            //return await _db.TaskManagementTaskProject.AsNoTracking().Include(x => x.Users)
            //    .Where(x => x.Id == id && !x.IsDeleted
            //        && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId
            //        && u.IsAdmin && !u.Deactivated) != null).Select(x => x.Name).FirstOrDefaultAsync() != null;
        }

        public async Task<Project> GetByIdIfAccessAdminAsync(long id, long mainAppUserId)
        {
            return await _db.TaskManagementProjectUsers.AsNoTracking().Include(x => x.Project)
                .Where(u => u.MainAppUserId == mainAppUserId && !u.Deactivated && u.IsAdmin
                && u.Project.Id == id && !u.Project.IsDeleted)
                .Select(x => x.Project).FirstOrDefaultAsync();
            //todo загрузит скорее всего с пользаками
            //return await _db.TaskManagementTaskProject.AsNoTracking().Include(x => x.Users)
            //                .Where(x => x.Id == id && !x.IsDeleted
            //                    && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId
            //                    && u.IsAdmin && !u.Deactivated) != null).FirstOrDefaultAsync();
        }


        public override async Task<Project> DeleteAsync(Project project)
        {
            _db.TaskManagementTaskProject.Attach(project);
            project.IsDeleted = true;
            await _db.SaveChangesAsync();
            return project;
        }

        public override async Task<IEnumerable<Project>> DeleteAsync(IEnumerable<Project> records)
        {
            _db.TaskManagementTaskProject.AttachRange(records);
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
            return await _db.TaskManagementProjectUsers.AsNoTracking()
                .Where(x => x.MainAppUserId == userId && !x.Deactivated)
                .Include(x => x.Project).Select(x => x.Project).Where(x => !x.IsDeleted).ToListAsync();


        }
    }
}
