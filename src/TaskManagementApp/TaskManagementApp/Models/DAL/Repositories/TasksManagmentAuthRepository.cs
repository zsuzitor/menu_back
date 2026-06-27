using BL.Models.Services.Interfaces;
using BO.Models.TaskManagementApp.DAL;
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace TaskManagementApp.Models.DAL.Repositories
{
    public class TasksManagmentAuthCachedRepository : TasksManagmentAuthRepository, ITasksManagmentAuthCachedRepository
    {

        public TasksManagmentAuthCachedRepository(MenuDbContext db,
            IGeneralRepositoryStrategy repo,
            ICacheService cache) : base(db, cache)
        {
        }

        public override async Task<bool> CanEditProject(long projectId, long userId)
        {

            var users = await _cache.GetAsync<List<ProjectUser>>(Consts.CacheKeys.UsersByProjectId + projectId);
            if (users.Item1)
            {
                if (!users.Item2.Exists(u => u.MainAppUserId == userId && (u.Role == UserRoleEnum.Admin || u.Role == UserRoleEnum.Editor)))
                    return false;
            }

            return await base.CanEditProject(projectId, userId);
        }

        public override async Task<bool> CanAdminEditProject(long projectId, long userId)
        {

            var users = await _cache.GetAsync<List<ProjectUser>>(Consts.CacheKeys.UsersByProjectId + projectId);
            if (users.Item1)
            {
                if (!users.Item2.Exists(u => u.MainAppUserId == userId && u.Role == UserRoleEnum.Admin))
                    return false;
            }

            return await base.CanEditProject(projectId, userId);
        }


        public override async Task<bool> CanAccessProject(long projectId, long userId)
        {

            var users = await _cache.GetAsync<List<ProjectUser>>(Consts.CacheKeys.UsersByProjectId + projectId);
            ProjectUser user = null;
            if (users.Item1)
            {
                user = users.Item2.FirstOrDefault(u => u.MainAppUserId == userId);

            }
            else
            {
                user = await _db.TaskManagementProjectUsers.FirstOrDefaultAsync(u => u.MainAppUserId == userId && u.ProjectId == projectId);
            }

            return CanAccessUser(user);

        }

    }

    public class TasksManagmentAuthRepository : ITasksManagmentAuthRepository
    {

        protected readonly ICacheService _cache;
        protected readonly MenuDbContext _db;

        public TasksManagmentAuthRepository(MenuDbContext db
            , ICacheService cache) 
        {
            _cache = cache;
            _db = db;
        }

        public virtual async Task<bool> CanAccessProject(long projectId, long userId)
        {
            var user = await _db.TaskManagementProjectUsers.FirstOrDefaultAsync(u => u.MainAppUserId == userId && u.ProjectId == projectId);
            return CanAccessUser(user);

        }


        public virtual async Task<bool> CanEditProject(long projectId, long userId)
        {
            //было бы конечно хорошо тут засовывать в кеш, но тогда тут будет дублирование логики из репозитория, либо репо как зависимость подключать что все сильно условжняет
            return await _db.TaskManagementProjectUsers.AnyAsync(u => u.MainAppUserId == userId && (u.Role == UserRoleEnum.Admin || u.Role == UserRoleEnum.Editor) && u.ProjectId == projectId);

        }

        public virtual async Task<bool> CanAdminEditProject(long projectId, long userId)
        {
            //было бы конечно хорошо тут засовывать в кеш, но тогда тут будет дублирование логики из репозитория, либо репо как зависимость подключать что все сильно условжняет
            return await _db.TaskManagementProjectUsers.AnyAsync(u => u.MainAppUserId == userId && u.Role == UserRoleEnum.Admin && u.ProjectId == projectId);
        }

        public async Task<bool> CanViewProject(long projectId, long userId)
        {
            return await _db.TaskManagementProjectUsers.AnyAsync(u => u.MainAppUserId == userId && u.Role != UserRoleEnum.Deactivated && u.ProjectId == projectId);
        }

        public virtual async Task<bool> CanEditTask(long taskId, long userId)
        {
            var task = await _db.TaskManagementTasks.Where(x => x.Id == taskId).Select(x => x.ProjectId).FirstOrDefaultAsync();
            if (task == default)
            {
                return false;
            }
            return await this.CanEditProject(task, userId);
        }

        public virtual async Task<bool> CanAccessTask(long taskId, long userId)
        {

            var task = await _db.TaskManagementTasks.Where(x=> x.Id == taskId).Select(x=>x.ProjectId).FirstOrDefaultAsync();
            if (task == default)
            {
                return false;
            }
            return await this.CanAccessProject(task, userId);
        }

        public async Task<bool> CanViewTask(long taskId, long userId)
        {
            var task = await _db.TaskManagementTasks.FirstOrDefaultAsync(x => x.Id == taskId);
            if (task == null)
            {
                return false;
            }
            return await this.CanViewProject(task.ProjectId, userId);
        }

        public Expression<Func<ProjectUser, bool>> IsAccess(long userId)
        {
            return user => user.MainAppUserId == userId && user.Role != UserRoleEnum.Deactivated;
        }

        public Expression<Func<ProjectUser, bool>> IsAccess(long userId, long projectId)
        {
            return user => user.MainAppUserId == userId && user.Role != UserRoleEnum.Deactivated && user.ProjectId == projectId;
        }

        public Expression<Func<ProjectUser, bool>> IsAdmin(long userId)
        {
            return user => user.MainAppUserId == userId && user.Role == UserRoleEnum.Admin;
        }

        protected bool CanAccessUser(ProjectUser user)
        {

            //if (user?.Role == UserRoleEnum.Admin)
            //{
            //    return (true, true);
            //}

            if (user != null && user.Role != UserRoleEnum.Deactivated)
            {
                return true;// (true, false);
            }

            return false;// (false, false);
        }


    }
}
