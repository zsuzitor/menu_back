using BO.Models.Auth;
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{
    public sealed class ProjectUserService : IProjectUserService
    {
        private readonly IProjectUserRepository _projectUserRepository;
        public ProjectUserService(IProjectUserRepository projectUserRepository)
        {
            _projectUserRepository = projectUserRepository;
        }

        public async Task<ProjectUser> CreateAsync(ProjectUser user)
        {
            return await _projectUserRepository.CreateAsync(user);
        }

        public async Task<List<ProjectUser>> GetProjectUsersAsync(long projectId)
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
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.EmptyUserName);
            }

            var user = await _projectUserRepository.GetAsync(userId);
            if (user == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectUserNotFound);
            }

            var userCurrent = await _projectUserRepository.GetByMainAppUserIdAsync(userInfo.UserId, user.ProjectId);
            if (userCurrent == null || !userCurrent.IsAdmin || userCurrent.Deactivated)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.HaveNoAccessToEditProject);
            }

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
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectUserNotFound);
            }

            if (user.Deactivated)
            {
                return user;
            }

            var userCurrent = await _projectUserRepository.GetByMainAppUserIdAsync(userInfo.UserId, user.ProjectId);
            if (userCurrent == null || !userCurrent.IsAdmin || userCurrent.Deactivated)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.HaveNoAccessToEditProject);
            }

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

        public async Task<long?> GetIdByMainAppIdAsync(UserInfo userInfo, long projectId)
        {
            return await _projectUserRepository.GetIdByMainAppIdAsync(userInfo.UserId, projectId);
        }

        public async Task<string> GetNotificationEmailAsync(long userId)
        {
            return await _projectUserRepository.GetNotificationEmailAsync(userId);
        }
    }
}
