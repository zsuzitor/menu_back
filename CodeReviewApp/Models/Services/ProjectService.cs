using BO.Models.Auth;
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using Menu.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{
    public sealed class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskStatusRepository _taskStatusRepository;
        private readonly IProjectUserService _projectUserService;
        private readonly ITaskReviewService _taskReviewService;
        private readonly IUserService _mainAppUserService;
        public ProjectService(IProjectRepository projectRepository, IProjectUserService userService,
            ITaskReviewService taskReviewService, IUserService mainAppUserService, ITaskStatusRepository taskStatusRepository)
        {
            _projectRepository = projectRepository;
            _projectUserService = userService;
            _taskReviewService = taskReviewService;
            _mainAppUserService = mainAppUserService;
            _taskStatusRepository = taskStatusRepository;
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
            return await _projectUserService.CreateAsync(user);

        }


        public async Task<TaskReview> CreateTaskAsync(TaskReview task, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.EmptyTaskName);
            }

            var creator = await _projectUserService.GetAdminByMainAppIdAsync(userInfo, task.ProjectId);
            if (creator == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible);
            }
            //if (!(await ExistIfAccessAsync(task.ProjectId, userInfo)).access)
            //{
            //    throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible);
            //}

            task.CreatorId = creator.Id;
            //var creatorExist = await _projectUserService.ExistAsync(task.ProjectId, task.CreatorId);
            //if (!creatorExist)
            //{
            //    throw new SomeCustomException(Consts.CodeReviewErrorConsts.UserNotFound);
            //}

            if (task.ReviewerId != null)
            {
                var reviewerExist = await _projectUserService.ExistAsync(task.ProjectId, task.ReviewerId.Value);
                if (!reviewerExist)
                {
                    throw new SomeCustomException(Consts.CodeReviewErrorConsts.UserNotFound);
                }
            }



            var status = task.StatusId == null ? null : await _taskStatusRepository.GetAsync(task.StatusId.Value);
            if (status == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskReviewStatusNotExists);

            }

            var newTask = new TaskReview()
            {
                CreatorId = task.CreatorId,
                Name = task.Name,
                ProjectId = task.ProjectId,
                ReviewerId = task.ReviewerId,
                CreatorEntityId = userInfo.UserId,
                CreateDate = DateTime.Now,
                LastUpdateDate = DateTime.Now,
                StatusId = task.StatusId,
            };

            //todo проверяем что creator+reviwer входит в проект. по идеи если что упадет с исключением
            return await _taskReviewService.CreateAsync(newTask, userInfo);
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

        public async Task<List<TaskReviewStatus>> GetStatusesAsync(long projectId, UserInfo userInfo)
        {
            return await _taskStatusRepository.GetForProjectAsync(projectId);

        }


        public async Task<List<TaskReviewStatus>> GetStatusesAccessAsync(long projectId, UserInfo userInfo)
        {
            var s = await ExistIfAccessAsync(projectId, userInfo);
            if (!s.access)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await GetStatusesAsync(projectId, userInfo);

        }

        public async Task<TaskReviewStatus> CreateStatusAsync(string status, long projectId, UserInfo userInfo)
        {
            var s = await ExistIfAccessAdminAsync(projectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await _taskStatusRepository.AddAsync(new TaskReviewStatus() { Name = status, ProjectId = projectId });
        }

        public async Task<TaskReviewStatus> DeleteStatusAsync(long statusId, UserInfo userInfo)
        {
            var status = await _taskStatusRepository.GetAsync(statusId);
            if (status == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskReviewStatusNotExists);
            }

            var s = await ExistIfAccessAdminAsync(status.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            var taskExists = await _taskReviewService.ExistAsync(status.ProjectId, statusId);
            if (taskExists)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskWithStatusExists);

            }

            return await _taskStatusRepository.DeleteAsync(status);
        }

        public async Task<TaskReviewStatus> UpdateStatusAsync(long statusId, string status, UserInfo userInfo)
        {

            var statusEntity = await _taskStatusRepository.GetAsync(statusId);
            if (statusEntity == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskReviewStatusNotExists);
            }

            var s = await ExistIfAccessAdminAsync(statusEntity.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            statusEntity.Name = status;
            var res = await _taskStatusRepository.UpdateAsync(statusEntity);
            return res;
            
        }
    }
}
