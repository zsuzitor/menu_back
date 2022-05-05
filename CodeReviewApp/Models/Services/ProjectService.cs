using BO.Models.Auth;
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserService _userService;
        private readonly ITaskReviewService _taskReviewService;
        public ProjectService(IProjectRepository projectRepository, IProjectUserService userService,
            ITaskReviewService taskReviewService)
        {
            _projectRepository = projectRepository;
            _userService = userService;
            _taskReviewService = taskReviewService;
        }

        public async Task<Project> CreateAsync(string name, UserInfo userInfo)
        {
            //конечно не очень красиво отходить от репозиториев, но ладно
            var user = new ProjectUser() { IsAdmin = true, UserName = "creator", MainAppUserId = userInfo.UserId };
            var project = await _projectRepository.CreateAsync(name, user);
            return project;
        }

        public async Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId)
        {
            return await _projectRepository.GetProjectsByMainAppUserIdAsync(userId);
        }

        public async Task<Project> GetAsync(long id)
        {
            return await _projectRepository.GetAsync(id);
        }

        public async Task<Project> GetByIdIfAccessAsync(long id, UserInfo userInfo)
        {
            return await _projectRepository.GetByIdIfAccessAsync(id, userInfo.UserId);
        }

        public async Task<bool> ExistIfAccessAsync(long id, UserInfo userInfo)
        {
            return await _projectRepository.ExistIfAccessAsync(id, userInfo.UserId);
        }

        public async Task<ProjectUser> CreateUserAsync(long projectId, string userName, long? mainAppUserId, UserInfo userInfo)
        {
            if (!await ExistIfAccessAsync(projectId, userInfo))
            {
                throw new SomeCustomException("project_not_found");//todo поиск и вынести
            }

            var user = new ProjectUser() { ProjectId = projectId, UserName = userName, MainAppUserId = mainAppUserId };
            return await _userService.CreateAsync(user);

        }


        public async Task<TaskReview> CreateTaskAsync(long projectId, string name, long creatorId, long? reviewerId, UserInfo userInfo)
        {
            if (!await ExistIfAccessAsync(projectId, userInfo))
            {
                throw new SomeCustomException("project_not_found");
            }

            var newTask = new TaskReview()
            {
                CreatorId = creatorId,
                Name = name,
                ProjectId = projectId,
                ReviewerId = reviewerId,
            };

            //todo проверяем что creator+reviwer входит в проект. по идеи если что упадет с исключением
            return await _taskReviewService.CreateAsync(newTask);
        }
    }
}
