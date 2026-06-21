using BL.Models.Services.Interfaces;
using BO.Models.DAL.Domain;
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
                if (!users.Item2.Exists(u => u.MainAppUserId == userId && u.Role == UserRoleEnum.Admin))
                    return false;
            }

            //было бы конечно хорошо тут засовывать в кеш, но тогда тут будет дублирование логики из репозитория, либо репо как зависимость подключать что все сильно условжняет
            return await _db.TaskManagementProjectUsers.AnyAsync(u => u.MainAppUserId == userId && u.Role == UserRoleEnum.Admin);
        }

        public override async Task<bool> CanEditTask(long taskId, long userId)
        {

        }

        public override async Task<(bool access, bool isAdmin)> CanAccessProject(long projectId, long userId)
        {

            var users = await _cache.GetAsync<List<ProjectUser>>(Consts.CacheKeys.UsersByProjectId + projectId);
            ProjectUser user = null;
            if (users.Item1)
            {
                user = users.Item2.FirstOrDefault(u => u.MainAppUserId == userId);

            }
            else
            {
                user = await _db.TaskManagementProjectUsers.FirstOrDefaultAsync(u => u.MainAppUserId == userId);
            }

            if (user?.Role == UserRoleEnum.Admin)
            {
                return (true, true);
            }

            if (user != null && user.Role != UserRoleEnum.Deactivated)
            {
                return (true, false);
            }

            return (false, false);

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

        public virtual async Task<(bool access, bool isAdmin)> CanAccessProject(long projectId, long userId)
        {

        }

        public virtual async Task<bool> CanEditProject(long projectId, long userId)
        {

        }

        public virtual async Task<bool> CanEditTask(long taskId, long userId)
        {

        }

        public Expression<Func<ProjectUser, bool>> IsAccess(long userId)
        {
            return user => user.MainAppUserId == userId && user.Role != UserRoleEnum.Deactivated;
        }

        public Expression<Func<ProjectUser, bool>> IsAdmin(long userId)
        {
            return user => user.MainAppUserId == userId && user.Role == UserRoleEnum.Admin;
        }
    }
}
