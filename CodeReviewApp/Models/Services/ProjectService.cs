using BO.Models.Auth;
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using Menu.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{
    public sealed class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserService _userService;
        private readonly ITaskReviewService _taskReviewService;
        private readonly IUserService _mainAppUserService;
        public ProjectService(IProjectRepository projectRepository, IProjectUserService userService,
            ITaskReviewService taskReviewService, IUserService mainAppUserService)
        {
            _projectRepository = projectRepository;
            _userService = userService;
            _taskReviewService = taskReviewService;
            _mainAppUserService = mainAppUserService;
        }

        public async Task<Project> CreateAsync(string name, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.EmptyProjectName);
            }
            //конечно не очень красиво отходить от репозиториев, но ладно
            var mainAppUserInfo = await _mainAppUserService.GetShortInfoAsync(userInfo.UserId);

            var user = new ProjectUser()
            {
                IsAdmin = true,
                UserName = string.IsNullOrEmpty(mainAppUserInfo.Name) ? mainAppUserInfo.Email : mainAppUserInfo.Name,
                MainAppUserId = userInfo.UserId,
                NotifyEmail = mainAppUserInfo.Email,
            };
            var project = await _projectRepository.CreateAsync(name, user);
            return project;
        }

        public async Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId)
        {
            return await _projectRepository.GetProjectsByMainAppUserIdAsync(userId);
        }

        public async Task<Project> GetAsync(long id)
        {
            return await _projectRepository.GetNoTrackAsync(id);
        }

        public async Task<Project> GetByIdIfAccessAsync(long id, UserInfo userInfo)
        {
            return await _projectRepository.GetByIdIfAccessAsync(id, userInfo.UserId);
        }

        public async Task<Project> GetByIdIfAccessAdminAsync(long id, UserInfo userInfo)
        {
            return await _projectRepository.GetByIdIfAccessAdminAsync(id, userInfo.UserId);
        }

        public async Task<(bool access, bool isAdmin)> ExistIfAccessAsync(long id, UserInfo userInfo)
        {
            return await _projectRepository.ExistIfAccessAsync(id, userInfo.UserId);
        }

        public async Task<bool> ExistIfAccessAdminAsync(long id, UserInfo userInfo)
        {
            return await _projectRepository.ExistIfAccessAdminAsync(id, userInfo.UserId);
        }


        public async Task<ProjectUser> CreateUserAsync(long projectId, string userName, string email, long? mainAppUserId, UserInfo userInfo)
        {
            if (!await ExistIfAccessAdminAsync(projectId, userInfo))
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.EmptyUserName);

            }

            var user = new ProjectUser()
            {
                ProjectId = projectId,
                UserName = userName,
                MainAppUserId = mainAppUserId,
                NotifyEmail = email
            };
            return await _userService.CreateAsync(user);

        }


        public async Task<TaskReview> CreateTaskAsync(long projectId, string name
            , long creatorId, long? reviewerId, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.EmptyTaskName);
            }

            if (!(await ExistIfAccessAsync(projectId, userInfo)).access)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            var creatorExist = await _userService.ExistAsync(projectId, creatorId);
            if (!creatorExist)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.UserNotFound);
            }

            if (reviewerId != null)
            {
                var reviewerExist = await _userService.ExistAsync(projectId, reviewerId.Value);
                if (!reviewerExist)
                {
                    throw new SomeCustomException(Consts.CodeReviewErrorConsts.UserNotFound);
                }
            }

            var newTask = new TaskReview()
            {
                CreatorId = creatorId,
                Name = name,
                ProjectId = projectId,
                ReviewerId = reviewerId,
                CreatorEntityId = userInfo.UserId,
            };

            //todo проверяем что creator+reviwer входит в проект. по идеи если что упадет с исключением
            return await _taskReviewService.CreateAsync(newTask);
        }

        public async Task<bool> DeleteAsync(long projectId, UserInfo userInfo)
        {
            var project = await _projectRepository.GetByIdIfAccessAdminAsync(projectId, userInfo.UserId);
            if (project == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible);
            }
            await _projectRepository.DeleteAsync(project);
            //project.IsDeleted = true;
            //await _projectRepository.UpdateAsync(project);
            return true;
        }

        public async Task AlertAsync()
        {
            var g = 10;
        }
    }
}
