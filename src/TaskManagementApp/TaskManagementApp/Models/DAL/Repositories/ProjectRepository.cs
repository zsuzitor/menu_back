
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
    public sealed class ProjectCachedRepository : ProjectRepository, IProjectCachedRepository
    {
        private readonly ICacheService _cache;
        private readonly ITasksManagmentAuthCachedRepository _auth;
        public ProjectCachedRepository(MenuDbContext db,
            IGeneralRepositoryStrategy repo,
            ICacheService cache,
            ITasksManagmentAuthCachedRepository auth) : base(db, repo, cache, auth)
        {
            //_projectRepository = projectRepository;
            _cache = cache;
            _auth = auth;
        }

        //public override async Task<Project> AddAsync(Project newRecord)
        //{
        //    var result = await _projectRepository.AddAsync(newRecord);
        //    //_cache.Set(Consts.CacheKeys.Project + result.Id,
        //    //    result,
        //    //    Consts.CacheKeys.CacheTime
        //    //);
        //    return result;
        //}

        //public async Task<IEnumerable<Project>> AddAsync(IEnumerable<Project> newRecords)
        //{
        //    var result = await _projectRepository.AddAsync(newRecords);
        //    //foreach (var r in result)
        //    //{
        //    //    _cache.Set(Consts.CacheKeys.Project + r.Id,
        //    //        r,
        //    //        Consts.CacheKeys.CacheTime
        //    //    );
        //    //}
        //    return result;

        //}

        //public async Task<Project> CreateAsync(string name, ProjectUser user)
        //{
        //    var record = await _projectRepository.CreateAsync(name, user);

        //    //_cache.Set(Consts.CacheKeys.Project + record.Id,
        //    //    record,
        //    //    Consts.CacheKeys.CacheTime
        //    //);
        //    return record;
        //}

        //public async Task<IEnumerable<Project>> DeleteAsync(IEnumerable<Project> records)
        //{
        //    var result = await _projectRepository.DeleteAsync(records);

        //    //foreach (var r in records)
        //    //{
        //    //    _cache.Remove(Consts.CacheKeys.Project + r.Id);
        //    //}

        //    return result;
        //}

        //public async Task<Project> DeleteAsync(Project record)
        //{
        //    var result = await _projectRepository.DeleteAsync(record);
        //    //_cache.Remove(Consts.CacheKeys.Project + record.Id);
        //    return result;
        //}

        //public async Task<Project> DeleteAsync(long recordId)
        //{
        //    var record = await _projectRepository.DeleteAsync(recordId);
        //    //_cache.Remove(Consts.CacheKeys.Project + recordId);
        //    return record;
        //}

        public override async Task<bool> ExistAsync(long id)
        {
            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await base.GetNoTrackAsync(id);
            },
            Consts.CacheKeys.CacheTime);
            return result != null;
            //return await _projectRepository.ExistAsync(id);
        }

        public override async Task<bool> ExistIfAccessAdminAsync(long id, long mainAppUserId)
        {
            //var users = await _cache.GetOrSetAsync(Consts.CacheKeys.UsersByProjectId + id,
            //async () =>
            //{
            //    return await base.GetProjectUsersAsync(id);
            //},
            //Consts.CacheKeys.CacheTime);

            //if (!users.Item2.Exists(u => u.MainAppUserId == mainAppUserId && u.Role == UserRoleEnum.Admin))
            //    return false;
            if(!await _auth.CanAdminEditProject(id,mainAppUserId))
                return false;

            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await base.GetNoTrackAsync(id);
            },
            Consts.CacheKeys.CacheTime);

            if (result == null || result.IsDeleted)
            {
                return false;
            }

            return true;

            //return result.Item2.Users.Exists(u => u.MainAppUserId == mainAppUserId && u.Role == UserRoleEnum.Admin);

            //return await _projectRepository.ExistIfAccessAdminAsync(id, mainAppUserId);
        }

        //public override async Task<(bool access, bool isAdmin)> ExistIfAccessAsync(long id, long mainAppUserId)
        //{
        //    //var users = await _cache.GetOrSetAsync(Consts.CacheKeys.UsersByProjectId + id,
        //    //async () =>
        //    //{
        //    //    return await base.GetProjectUsersAsync(id);
        //    //},
        //    //Consts.CacheKeys.CacheTime);

        //    //var user = users.Item2.FirstOrDefault(u => u.MainAppUserId == mainAppUserId && u.Role != UserRoleEnum.Deactivated);
        //    //var result = (user != null, user.Role == UserRoleEnum.Admin);
        //    //if (!result.Item1)
        //    //    return result;
        //    var result = await _auth.CanAccessProject(id, mainAppUserId);
        //    if (!result.access)
        //        return result;

        //    var (_, project) = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
        //    async () =>
        //    {
        //        return await base.GetNoTrackAsync(id);
        //    },
        //    Consts.CacheKeys.CacheTime);

        //    if (project == null || project.IsDeleted)
        //    {
        //        return (false, false);
        //    }

        //    return result;

        //}

        public override async Task<Project> GetAsync(long id)
        {
            //return await _projectRepository.GetAsync(id);

            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await base.GetNoTrackAsync(id);
            },
            Consts.CacheKeys.CacheTime);
            return result;
        }

        public override async Task<List<Project>> GetAsync(List<long> ids)
        {
            return await base.GetAsync(ids);
        }

        public override async Task<Project> GetByIdIfAccessAdminAsync(long id, long mainAppUserId)
        {

            //var users = await _cache.GetOrSetAsync(Consts.CacheKeys.UsersByProjectId + id,
            //async () =>
            //{
            //    return await base.GetProjectUsersAsync(id);
            //},
            //Consts.CacheKeys.CacheTime);

            //if (!users.Item2.Exists(u => u.MainAppUserId == mainAppUserId && u.Role == UserRoleEnum.Admin))
            //    return null;
            if (!await _auth.CanAdminEditProject(id, mainAppUserId))
                return null;

            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await base.GetNoTrackAsync(id);
            },
            Consts.CacheKeys.CacheTime);

            if (result == null || result.IsDeleted)
            {
                return null;
            }

            return result;
            //return await _projectRepository.GetByIdIfAccessAdminAsync(id, mainAppUserId);
        }

        public override async Task<Project> GetByIdIfAccessAsync(long id, long mainAppUserId)
        {

            //var users = await _cache.GetOrSetAsync(Consts.CacheKeys.UsersByProjectId + id,
            //async () =>
            //{
            //    return await base.GetProjectUsersAsync(id);
            //},
            //Consts.CacheKeys.CacheTime);

            //var user = users.Item2.FirstOrDefault(u => u.MainAppUserId == mainAppUserId && u.Role != UserRoleEnum.Deactivated);
            //if (user == null)
            //{
            //    return null;
            //}
            if (!(await _auth.CanAccessProject(id, mainAppUserId)))
                return null;

            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await base.GetNoTrackAsync(id);
            },
            Consts.CacheKeys.CacheTime);

            if (result == null || result.IsDeleted)
            {
                return null;
            }


            return result;
        }

        public override async Task<Project> GetNoTrackAsync(long id)
        {

            var result = await _cache.GetOrSetAsync(Consts.CacheKeys.Project + id,
            async () =>
            {
                return await base.GetNoTrackAsync(id);
            },
            Consts.CacheKeys.CacheTime);
            return result;
        }

        //public async Task<List<Project>> GetNoTrackAsync(List<long> ids)
        //{
        //    return await _projectRepository.GetNoTrackAsync(ids);
        //}

        //public override async Task<Project> GetNoTrackForCacheAsync(long id)
        //{
        //    return await _projectRepository.GetNoTrackForCacheAsync(id);
        //}

        //public async Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId)
        //{
        //    return await _projectRepository.GetProjectsByMainAppUserIdAsync(userId);
        //}

        //public override async Task<Project> UpdateAsync(Project record)
        //{
        //    var result = await _projectRepository.UpdateAsync(record);

        //    _cache.Set(Consts.CacheKeys.Project + result.Id,
        //        result,
        //        Consts.CacheKeys.CacheTime
        //    );
        //    return result;
        //}

        //public async Task<IEnumerable<Project>> UpdateAsync(IEnumerable<Project> records)
        //{
        //    return await _projectRepository.UpdateAsync(records);
        //}
    }

    public class ProjectRepository : GeneralRepository<Project, long>, IProjectRepository
    {
        private readonly ICacheService _cache;
        private readonly ITasksManagmentAuthRepository _auth;

        public ProjectRepository(MenuDbContext db, IGeneralRepositoryStrategy repo,
            ICacheService cache, ITasksManagmentAuthRepository auth) : base(db, repo)
        {
            _cache = cache;
            _auth = auth;
        }

        public virtual async Task<Project> CreateAsync(string name, ProjectUser user)
        {
            var newProject = new Project() { Name = name, Users = new List<ProjectUser>() { user } };

            await base.AddAsync(newProject);
            return newProject;

        }

        //public virtual async Task<(bool access, bool isAdmin)> ExistIfAccessAsync(long id, long mainAppUserId)
        //{
        //    var projExist = await _db.TaskManagementTaskProject.AnyAsync(x => x.Id == id && !x.IsDeleted);
        //    if (!projExist)
        //    {
        //        return (false, false);
        //    }

        //    var result = await _auth.CanAccessProject(id, mainAppUserId);
        //        return result;

            
        //    //var user = await _db.TaskManagementProjectUsers.AsNoTracking().Include(x => x.Project)
        //    //    .Where(u => u.MainAppUserId == mainAppUserId && u.Role != UserRoleEnum.Deactivated
        //    //    && u.Project.Id == id && !u.Project.IsDeleted)
        //    //    .Select(x => new { x.Id, isAdmin = (x.Role == UserRoleEnum.Admin) }).FirstOrDefaultAsync();

        //    ////var user = await _db.TaskManagementTaskProject.AsNoTracking().Include(x => x.Users)
        //    ////    .Where(x => x.Id == id && !x.IsDeleted
        //    ////        && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId && !u.Deactivated) != null)
        //    ////    .Select(x => x.Users.FirstOrDefault()).FirstOrDefaultAsync();
        //    ////user.IsAdmin;
        //    ////.FirstOrDefaultAsync() != null;
        //    //if (user == null || user.Id == 0)
        //    //{
        //    //    return (false, false);
        //    //}

        //    //return (true, user.isAdmin);
        //}

        public virtual async Task<Project> GetByIdIfAccessAsync(long id, long mainAppUserId)
        {
            var result = await _auth.CanAccessProject(id, mainAppUserId);
            if (result)
            {
                return await _db.TaskManagementTaskProject.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            }

            return null;


            //return await _db.TaskManagementProjectUsers.AsNoTracking().Include(x => x.Project)
            //    .Where(u => u.MainAppUserId == mainAppUserId && u.Role != UserRoleEnum.Deactivated
            //    && u.Project.Id == id && !u.Project.IsDeleted)
            //    .Select(x => x.Project).FirstOrDefaultAsync();

            //todo загрузит скорее всего с пользаками
            //return await _db.TaskManagementTaskProject.AsNoTracking().Include(x => x.Users)
            //                .Where(x => x.Id == id && !x.IsDeleted
            //                    && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId && !u.Deactivated) != null).FirstOrDefaultAsync();
        }

        public virtual async Task<bool> ExistIfAccessAdminAsync(long id, long mainAppUserId)
        {
            if (!await _auth.CanAdminEditProject(id, mainAppUserId))
                return false;
            return await _db.TaskManagementTaskProject.AnyAsync(x => x.Id == id);
            //return (await ExistIfAccessAsync(id, mainAppUserId)).isAdmin;
            //return await _db.TaskManagementTaskProject.AsNoTracking().Include(x => x.Users)
            //    .Where(x => x.Id == id && !x.IsDeleted
            //        && x.Users.FirstOrDefault(u => u.MainAppUserId == mainAppUserId
            //        && u.IsAdmin && !u.Deactivated) != null).Select(x => x.Name).FirstOrDefaultAsync() != null;
        }

        public virtual async Task<Project> GetByIdIfAccessAdminAsync(long id, long mainAppUserId)
        {

            var result = await _auth.CanAdminEditProject(id, mainAppUserId);
            if (result)
            {
                return await _db.TaskManagementTaskProject.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            }

            return null;

            //return await _db.TaskManagementProjectUsers.AsNoTracking().Include(x => x.Project)
            //    .Where(u => u.MainAppUserId == mainAppUserId && u.Role == UserRoleEnum.Admin
            //    && u.Project.Id == id && !u.Project.IsDeleted)
            //    .Select(x => x.Project).FirstOrDefaultAsync();
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
            Consts.CacheKeys.ClearForProject(_cache, project.Id);
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
            foreach (var r in records)
            {
                Consts.CacheKeys.ClearForProject(_cache, r.Id);
            }

            return records;
        }

        public override async Task<Project> DeleteAsync(long recordId)
        {
            var project = await GetAsync(recordId);
            if (project != null)
            {
                project.IsDeleted = false;
                await _db.SaveChangesAsync();
                Consts.CacheKeys.ClearForProject(_cache, project.Id);
            }

            return project;
        }


        public virtual async Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId)
        {
            var access = _auth.IsAccess(userId);
            return await _db.TaskManagementProjectUsers.AsNoTracking()
                .Where(access)
                .Include(x => x.Project).Select(x => x.Project).Where(x => !x.IsDeleted).ToListAsync();


        }

        //public virtual async Task<Project> GetNoTrackForCacheAsync(long id)
        //{
        //    return await _db.TaskManagementTaskProject
        //        .Include(x => x.Sprints).Include(x => x.Users).Include(x => x.TaskStatuses)
        //        .Include(x => x.Presets)
        //        .AsNoTracking().FirstOrDefaultAsync();
        //}

        public override async Task<Project> AddAsync(Project newRecord)
        {
            var result = await base.AddAsync(newRecord);
            //_cache.Remove(Consts.CacheKeys.Project + result.Id);
            return result;
        }

        public override async Task<IEnumerable<Project>> AddAsync(IEnumerable<Project> newRecords)
        {
            var result = await base.AddAsync(newRecords);
            //foreach (var record in result)
            //{
            //    _cache.Remove(Consts.CacheKeys.Project + record.Id);
            //}
            return result;
        }

        public override async Task<Project> UpdateAsync(Project record)
        {
            var result = await base.UpdateAsync(record);
            Consts.CacheKeys.ClearForProject(_cache, result.Id);
            return result;
        }

        public override async Task<IEnumerable<Project>> UpdateAsync(IEnumerable<Project> records)
        {
            var result = await base.UpdateAsync(records);
            foreach (var record in result)
            {
                Consts.CacheKeys.ClearForProject(_cache, record.Id);
            }
            return result;
        }

        protected async Task<List<ProjectUser>> GetProjectUsersAsync(long projectId)
        {
            return await _db.TaskManagementProjectUsers.AsNoTracking().Where(x => x.ProjectId == projectId).ToListAsync();
        }
    }
}
