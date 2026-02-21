
using BL.Models.Services.Interfaces;
using BO.Models.TaskManagementApp.DAL;
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
    public sealed class ProjectCachedRepository : GeneralRepository<Project, long>, IProjectCachedRepository
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ICacheService _cache;
        public ProjectCachedRepository(IProjectRepository projectRepository, MenuDbContext db, IGeneralRepositoryStrategy repo,
            ICacheService cache) : base(db, repo)
        {
            _projectRepository = projectRepository;
            _cache = cache;
        }

        public async override Task<Project> AddAsync(Project newRecord)
        {
            var result = await _projectRepository.AddAsync(newRecord);
            _cache.Set(Consts.CacheKeys.Project + result.Id,
                result,
                Consts.CacheKeys.CacheTime
            );
            return result;
        }

        public async override Task<IEnumerable<Project>> AddAsync(IEnumerable<Project> newRecords)
        {
            var result = await _projectRepository.AddAsync(newRecords);
            foreach (var r in result)
            {
                _cache.Set(Consts.CacheKeys.Project + r.Id,
                    r,
                    Consts.CacheKeys.CacheTime
                );
            }
            return result;

        }

        public async Task<Project> CreateAsync(string name, ProjectUser user)
        {
            var record = await _projectRepository.CreateAsync(name, user);

            _cache.Set(Consts.CacheKeys.Project + record.Id,
                record,
                Consts.CacheKeys.CacheTime
            );
            return record;
        }

        public async override Task<IEnumerable<Project>> DeleteAsync(IEnumerable<Project> records)
        {
            var result = await _projectRepository.DeleteAsync(records);

            foreach (var r in records)
            {
                _cache.Remove(Consts.CacheKeys.Project + r.Id);
            }

            return result;
        }

        public async override Task<Project> DeleteAsync(Project record)
        {
            var result = await _projectRepository.DeleteAsync(record);
            _cache.Remove(Consts.CacheKeys.Project + record.Id);
            return result;
        }

        public async override Task<Project> DeleteAsync(long recordId)
        {
            var record = await _projectRepository.DeleteAsync(recordId);
            _cache.Remove(Consts.CacheKeys.Project + recordId);
            return record;
        }

        public async override Task<bool> ExistAsync(long id)
        {
            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await _projectRepository.GetNoTrackForCacheAsync(id);
            },
            Consts.CacheKeys.CacheTime);
            return result.Item2 != null;
            //return await _projectRepository.ExistAsync(id);
        }

        public async Task<bool> ExistIfAccessAdminAsync(long id, long mainAppUserId)
        {

            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await _projectRepository.GetNoTrackForCacheAsync(id);
            },
            Consts.CacheKeys.CacheTime);

            if (result.Item2 == null || result.Item2.IsDeleted)
            {
                return false;
            }

            return result.Item2.Users.Exists(u => u.MainAppUserId == mainAppUserId && u.Role == UserRoleEnum.Admin);

            //return await _projectRepository.ExistIfAccessAdminAsync(id, mainAppUserId);
        }

        public async Task<(bool access, bool isAdmin)> ExistIfAccessAsync(long id, long mainAppUserId)
        {
            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await _projectRepository.GetNoTrackForCacheAsync(id);
            },
            Consts.CacheKeys.CacheTime);

            if (result.Item2 == null || result.Item2.IsDeleted)
            {
                return (false, false);
            }

            var user = result.Item2.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId && u.Role != UserRoleEnum.Deactivated);
            if (user != null)
            {
                if (user.Role == UserRoleEnum.Admin)
                {
                    return (true, true);

                }

                return (true, false);
            }
            return (false, false);

        }

        public async override Task<Project> GetAsync(long id)
        {
            //return await _projectRepository.GetAsync(id);

            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await _projectRepository.GetNoTrackForCacheAsync(id);
            },
            Consts.CacheKeys.CacheTime);
            return result.Item2;
        }

        public async override Task<List<Project>> GetAsync(List<long> ids)
        {
            return await _projectRepository.GetAsync(ids);
        }

        public async Task<Project> GetByIdIfAccessAdminAsync(long id, long mainAppUserId)
        {
            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await _projectRepository.GetNoTrackForCacheAsync(id);
            },
            Consts.CacheKeys.CacheTime);

            if (result.Item2 == null || result.Item2.IsDeleted)
            {
                return null;
            }

            if (result.Item2.Users.Exists(u => u.MainAppUserId == mainAppUserId && u.Role == UserRoleEnum.Admin))
            {
                return result.Item2;
            }

            return null;
            //return await _projectRepository.GetByIdIfAccessAdminAsync(id, mainAppUserId);
        }

        public async Task<Project> GetByIdIfAccessAsync(long id, long mainAppUserId)
        {
            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await _projectRepository.GetNoTrackForCacheAsync(id);
            },
            Consts.CacheKeys.CacheTime);

            if (result.Item2 == null || result.Item2.IsDeleted)
            {
                return null;
            }

            if (result.Item2.Users.Exists(u => u.MainAppUserId == mainAppUserId && u.Role != UserRoleEnum.Deactivated))
            {
                return result.Item2;
            }

            return null;
        }

        public async override Task<Project> GetNoTrackAsync(long id)
        {

            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await _projectRepository.GetNoTrackForCacheAsync(id);
            },
            Consts.CacheKeys.CacheTime);
            return result.Item2;
        }

        public async override Task<List<Project>> GetNoTrackAsync(List<long> ids)
        {
            return await _projectRepository.GetNoTrackAsync(ids);
        }

        public async Task<Project> GetNoTrackForCacheAsync(long id)
        {
            return await _projectRepository.GetNoTrackForCacheAsync(id);
        }

        public async Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId)
        {
            return await _projectRepository.GetProjectsByMainAppUserIdAsync(userId);
        }

        public async override Task<Project> UpdateAsync(Project record)
        {
            var result = await _projectRepository.UpdateAsync(record);

            _cache.Set(Consts.CacheKeys.Project + result.Id,
                result,
                Consts.CacheKeys.CacheTime
            );
            return result;
        }

        public async override Task<IEnumerable<Project>> UpdateAsync(IEnumerable<Project> records)
        {
            return await _projectRepository.UpdateAsync(records);
        }
    }

    public sealed class ProjectRepository : GeneralRepository<Project, long>, IProjectRepository
    {
        private readonly ICacheService _cache;

        public ProjectRepository(MenuDbContext db, IGeneralRepositoryStrategy repo,
            ICacheService cache) : base(db, repo)
        {
            _cache = cache;
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
                .Where(u => u.MainAppUserId == mainAppUserId && u.Role != UserRoleEnum.Deactivated
                && u.Project.Id == id && !u.Project.IsDeleted)
                .Select(x => new { x.Id, isAdmin = (x.Role == UserRoleEnum.Admin) }).FirstOrDefaultAsync();

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

            return (true, user.isAdmin);
        }

        public async Task<Project> GetByIdIfAccessAsync(long id, long mainAppUserId)
        {

            return await _db.TaskManagementProjectUsers.AsNoTracking().Include(x => x.Project)
                .Where(u => u.MainAppUserId == mainAppUserId && u.Role != UserRoleEnum.Deactivated
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
                .Where(u => u.MainAppUserId == mainAppUserId && u.Role == UserRoleEnum.Admin
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
                .Where(x => x.MainAppUserId == userId && x.Role != UserRoleEnum.Deactivated)
                .Include(x => x.Project).Select(x => x.Project).Where(x => !x.IsDeleted).ToListAsync();


        }

        public async Task<Project> GetNoTrackForCacheAsync(long id)
        {
            return await _db.TaskManagementTaskProject
                .Include(x => x.Sprints).Include(x => x.Users).Include(x => x.TaskStatuses)
                .AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
