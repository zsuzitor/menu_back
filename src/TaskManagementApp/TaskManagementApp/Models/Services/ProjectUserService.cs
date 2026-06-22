using BO.Models.TaskManagementApp.DAL;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models.Exceptions;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;

namespace TaskManagementApp.Models.Services
{
    public sealed class ProjectUserService : IProjectUserService
    {
        private readonly IProjectUserCahcedRepository _projectUserRepository;
        private readonly IProjectCachedRepository _projectCacheRepository;
        private readonly IUserRepository _userRepo;

        public ProjectUserService(IProjectUserCahcedRepository projectUserRepository
            , IProjectCachedRepository projectCacheRepository, IUserRepository userRepo)
        {
            _projectUserRepository = projectUserRepository;
            _projectCacheRepository = projectCacheRepository;
            _userRepo = userRepo;
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

            return await _projectUserRepository.GetProjectUsersWithMainAppUserAsync(projectId);
        }

        public async Task<List<ProjectUser>> GetProjectUsersAsync(long projectId)
        {
            return await _projectUserRepository.GetProjectUsersWithMainAppUserAsync(projectId);
        }



        public async Task<bool> ExistByMainAppUserIdAsync(long projectId, long mainAppUserId)
        {
            return await _projectUserRepository.ExistByMainIdAsync(projectId, mainAppUserId);
        }

        public async Task<ProjectUser> ChangeAsync(long userIdForChange, long projectId,  bool isAdmin, bool deactivated, long userId)
        {
            await ThrowIfNotAccessToProject(userId, projectId, true);

            var user = await _projectUserRepository.GetByMainAppUserIdAsync(userIdForChange, projectId);
            if (user == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectUserNotFound);
            }

            //if (user.MainAppUserId == userId)
            //{
            //    //если редачим себя проверяем можем ли мы проставить флаг isAdmin
            //    await ThrowIfNotAccessToProject(userId, user.ProjectId, isAdmin);
            //}
            //else
            //{
            //    await ThrowIfNotAccessToProject(userId, user.ProjectId, true);
            //}

            //var userCurrent = await _projectUserRepository.GetByMainAppUserIdAsync(userInfo.UserId, user.ProjectId);
            //if (userCurrent == null || !userCurrent.IsAdmin || userCurrent.Deactivated)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.HaveNoAccessToEditProject);
            //}

            user.Role = UserRoleEnum.Editor;
            if (isAdmin)
                user.Role = UserRoleEnum.Admin;
            if (deactivated)
                user.Role = UserRoleEnum.Deactivated;
            await _projectUserRepository.UpdateAsync(user);
            return user;

        }

        public async Task<ProjectUser> DeleteAsync(long userIdForDel, long projectId, long userId)
        {
            await ThrowIfNotAccessToProject(userId, projectId, true);

            var user = await _projectUserRepository.GetByMainAppUserIdAsync(userIdForDel, projectId);
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


        public async Task<string> GetNotificationEmailAsync(long userId)
        {
            return await _userRepo.GetEmail(userId);
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

        //public async Task<List<ProjectUser>> GetProjectUserAsync(long projectId, List<long> usersId)
        //{
        //    return await _projectUserRepository.GetProjectUserAsync(projectId, usersId);
        //}


    }
}
