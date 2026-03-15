using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using BO.Models.DAL.Domain;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using BO.Models.TaskManagementApp.DAL;

namespace TaskManagementApp.Models.Services
{
    public sealed class ProjectUserService : IProjectUserService
    {
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectCachedRepository _projectCacheRepository;

        public ProjectUserService(IProjectUserRepository projectUserRepository, IProjectRepository projectRepository
            , IProjectCachedRepository projectCacheRepository)
        {
            _projectUserRepository = projectUserRepository;
            _projectRepository = projectRepository;
            _projectCacheRepository = projectCacheRepository;
        }

        public async Task<ProjectUser> CreateAsync(ProjectUser user)
        {
            if (user?.MainAppUserId != null)
            {
                var exist = await _projectUserRepository.ExistByMainIdAsync(user.ProjectId, user.MainAppUserId.Value);
                if (exist)
                {
                    throw new SomeCustomException(Consts.ErrorConsts.UserAlreadyAdded);
                }
            }

            return await _projectUserRepository.AddAsync(user);
        }

        public async Task<List<ProjectUser>> GetProjectUsersAccessAsync(long projectId, long userId)
        {

            await ThrowIfNotAccessToProject(projectId, userId, false);

            return await _projectUserRepository.GetProjectUsersAsync(projectId);
        }

        public async Task<List<ProjectUser>> GetProjectUsersAsync(long projectId, long userId)
        {
            return await _projectUserRepository.GetProjectUsersAsync(projectId);
        }


        public async Task<bool> ExistAsync(long projectId, long userId)
        {
            return await _projectUserRepository.ExistAsync(projectId, userId);
        }

        public async Task<ProjectUser> ChangeAsync(long userIdForChange, string name, string email, bool isAdmin, bool deactivated, long userId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new SomeCustomException(Consts.ErrorConsts.EmptyUserName);
            }

            var user = await _projectUserRepository.GetAsync(userIdForChange);
            if (user == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectUserNotFound);
            }

            if (user.MainAppUserId == userId)
            {
                //если редачим себя проверяем можем ли мы проставить флаг isAdmin
                await ThrowIfNotAccessToProject(userId, user.ProjectId, isAdmin);
            }
            else
            {
                await ThrowIfNotAccessToProject(userId, user.ProjectId, true);
            }

            //var userCurrent = await _projectUserRepository.GetByMainAppUserIdAsync(userInfo.UserId, user.ProjectId);
            //if (userCurrent == null || !userCurrent.IsAdmin || userCurrent.Deactivated)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.HaveNoAccessToEditProject);
            //}

            user.UserName = name;
            user.NotifyEmail = email;
            user.Role = UserRoleEnum.Editor;
            if (isAdmin)
                user.Role = UserRoleEnum.Admin;
            if (deactivated)
                user.Role = UserRoleEnum.Deactivated;
            await _projectUserRepository.UpdateAsync(user);
            return user;

        }

        public async Task<ProjectUser> DeleteAsync(long userIdForDel, long userId)
        {
            var user = await _projectUserRepository.GetAsync(userIdForDel);
            if (user == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectUserNotFound);
            }

            if (user.Role == UserRoleEnum.Deactivated)
            {
                return user;
            }

            //var userCurrent = await _projectUserRepository.GetByMainAppUserIdAsync(userInfo.UserId, user.ProjectId);
            //if (userCurrent == null || !userCurrent.IsAdmin || userCurrent.Deactivated)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.HaveNoAccessToEditProject);
            //}
            await ThrowIfNotAccessToProject(userId, user.ProjectId, true);

            await _projectUserRepository.DeleteAsync(user);
            return user;
        }

        public async Task<ProjectUser> GetByMainAppIdAsync(long userId, long projectId)
        {
            var user = await _projectUserRepository.GetByMainAppUserIdAsync(userId, projectId);
            if (user.Role == UserRoleEnum.Deactivated)
            {
                return null;
            }

            return user;
        }


        public async Task<ProjectUser> GetAdminByMainAppIdAsync(long userId, long projectId)
        {
            var user = await _projectUserRepository.GetByMainAppUserIdAsync(userId, projectId);
            if (user.Role != UserRoleEnum.Admin)
            {
                return null;
            }

            return user;
        }

        public async Task<long?> GetIdByMainAppIdAsync(long userId, long projectId)
        {
            return await _projectUserRepository.GetIdByMainAppIdAsync(userId, projectId);
        }

        public async Task<string> GetNotificationEmailAsync(long userId)
        {
            return await _projectUserRepository.GetNotificationEmailAsync(userId);
        }

        public async Task<(string email, long? mainAppId)> GetNotificationEmailWithMainAppIdAsync(long userId)
        {
            return await _projectUserRepository.GetNotificationEmailWithMainAppIdAsync(userId);
        }



        private async Task ThrowIfNotAccessToProject(long mainAppUserId, long projectId, bool isAdmin)
        {
            var res = await _projectCacheRepository.ExistIfAccessAsync(projectId, mainAppUserId);
            //var userCurrent = await _projectUserRepository.GetByMainAppUserIdAsync(mainAppUserId, projectId);
            //if (userCurrent == null || !userCurrent.IsAdmin || userCurrent.Deactivated)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.HaveNoAccessToEditProject);
            //}
            if (!res.access || (isAdmin && !res.isAdmin))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }
        }

        public async Task<List<ProjectUser>> GetProjectUserAsync(long projectId, List<long> usersId)
        {
            return await _projectUserRepository.GetProjectUserAsync(projectId, usersId);
        }
    }
}
