using BO.Models.Auth;
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{
    public class ProjectUserService : IProjectUserService
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

        public async Task<ProjectUser> ChangeAsync(long userId, string name, string email, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new SomeCustomException("empty_user_name");
            }

            var user = await _projectUserRepository.GetAsync(userId);
            if (user == null)
            {
                throw new SomeCustomException("project_user_not_founded");
            }

            var userCurrent = await _projectUserRepository.GetByMainAppUserIdAsync(user.ProjectId, userInfo.UserId);
            if (userCurrent == null || !userCurrent.IsAdmin)
            {
                throw new SomeCustomException("have_no_access_to_edit_project");
            }

            user.UserName = name;
            user.NotifyEmail = email;
            await _projectUserRepository.UpdateAsync(user);
            return user;

        }

        public async Task<ProjectUser> DeleteAsync(long userId, UserInfo userInfo)
        {
            var user = await _projectUserRepository.GetAsync(userId);
            if (user == null)
            {
                throw new SomeCustomException("project_user_not_founded");
            }

            var userCurrent = await _projectUserRepository.GetByMainAppUserIdAsync(user.ProjectId, userInfo.UserId);
            if (userCurrent == null || !userCurrent.IsAdmin)
            {
                throw new SomeCustomException("have_no_access_to_edit_project");
            }

            await _projectUserRepository.DeleteAsync(user);
            return user;
        }

        public async Task<ProjectUser> GetByMainAppIdAsync(UserInfo userInfo)
        {
            return await _projectUserRepository.GetByMainAppIdAsync(userInfo.UserId);
        }
    }
}
