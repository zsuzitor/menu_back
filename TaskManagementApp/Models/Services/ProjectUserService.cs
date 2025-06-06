﻿using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using BO.Models.DAL.Domain;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services
{
    public sealed class ProjectUserService : IProjectUserService
    {
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly IProjectRepository _projectRepository;
        public ProjectUserService(IProjectUserRepository projectUserRepository, IProjectRepository projectRepository)
        {
            _projectUserRepository = projectUserRepository;
            _projectRepository = projectRepository;
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

            return await _projectUserRepository.CreateAsync(user);
        }

        public async Task<List<ProjectUser>> GetProjectUsersAccessAsync(long projectId, UserInfo userInfo)
        {

            await ThrowIfNotAccessToProject(projectId, userInfo.UserId, false);

            return await _projectUserRepository.GetProjectUsersAsync(projectId);
        }

        public async Task<List<ProjectUser>> GetProjectUsersAsync(long projectId, UserInfo userInfo)
        {
            return await _projectUserRepository.GetProjectUsersAsync(projectId);
        }


        public async Task<bool> ExistAsync(long projectId, long userId)
        {
            return await _projectUserRepository.ExistAsync(projectId, userId);
        }

        public async Task<ProjectUser> ChangeAsync(long userId, string name, string email, bool isAdmin, bool deactivated, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new SomeCustomException(Consts.ErrorConsts.EmptyUserName);
            }

            var user = await _projectUserRepository.GetAsync(userId);
            if (user == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectUserNotFound);
            }

            if(user.MainAppUserId == userInfo.UserId)
            {
                if (isAdmin)
                {
                    await ThrowIfNotAccessToProject(userInfo.UserId, user.ProjectId, true);
                }
                else
                {
                    await ThrowIfNotAccessToProject(userInfo.UserId, user.ProjectId, false);
                }
            }
            else
            {
                await ThrowIfNotAccessToProject(userInfo.UserId, user.ProjectId, true);
            }

            //var userCurrent = await _projectUserRepository.GetByMainAppUserIdAsync(userInfo.UserId, user.ProjectId);
            //if (userCurrent == null || !userCurrent.IsAdmin || userCurrent.Deactivated)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.HaveNoAccessToEditProject);
            //}

            user.UserName = name;
            user.NotifyEmail = email;
            user.IsAdmin = isAdmin;
            user.Deactivated = deactivated;
            await _projectUserRepository.UpdateAsync(user);
            return user;

        }

        public async Task<ProjectUser> DeleteAsync(long userId, UserInfo userInfo)
        {
            var user = await _projectUserRepository.GetAsync(userId);
            if (user == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectUserNotFound);
            }

            if (user.Deactivated)
            {
                return user;
            }

            //var userCurrent = await _projectUserRepository.GetByMainAppUserIdAsync(userInfo.UserId, user.ProjectId);
            //if (userCurrent == null || !userCurrent.IsAdmin || userCurrent.Deactivated)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.HaveNoAccessToEditProject);
            //}
            await ThrowIfNotAccessToProject(userInfo.UserId, user.ProjectId, true);

            await _projectUserRepository.DeleteAsync(user);
            return user;
        }

        public async Task<ProjectUser> GetByMainAppIdAsync(UserInfo userInfo, long projectId)
        {
            var user = await _projectUserRepository.GetByMainAppUserIdAsync(userInfo.UserId, projectId);
            if (user.Deactivated)
            {
                return null;
            }

            return user;
        }


        public async Task<ProjectUser> GetAdminByMainAppIdAsync(UserInfo userInfo, long projectId)
        {
            var user = await _projectUserRepository.GetByMainAppUserIdAsync(userInfo.UserId, projectId);
            if (user.Deactivated || !user.IsAdmin)
            {
                return null;
            }

            return user;
        }

        public async Task<long?> GetIdByMainAppIdAsync(UserInfo userInfo, long projectId)
        {
            return await _projectUserRepository.GetIdByMainAppIdAsync(userInfo.UserId, projectId);
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
            var res = await _projectRepository.ExistIfAccessAsync(projectId, mainAppUserId);
            //var userCurrent = await _projectUserRepository.GetByMainAppUserIdAsync(mainAppUserId, projectId);
            //if (userCurrent == null || !userCurrent.IsAdmin || userCurrent.Deactivated)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.HaveNoAccessToEditProject);
            //}
            if (!res.access || (isAdmin && !res.isAdmin))
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }
        }

        public async Task<List<ProjectUser>> GetProjectUserAsync(long projectId, List<long> usersId)
        {
            return await _projectUserRepository.GetProjectUserAsync(projectId, usersId);
        }
    }
}
