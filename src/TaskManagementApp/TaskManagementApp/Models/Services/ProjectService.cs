using BL.Models.Services.Interfaces;
using BO.Models.TaskManagementApp.DAL;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models.Exceptions;
using Menu.Models.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;

namespace TaskManagementApp.Models.Services
{
    public sealed class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectCachedRepository _projectCacheRepository;
        private readonly ITaskStatusRepository _taskStatusRepository;
        private readonly IProjectUserService _projectUserService;
        private readonly IWorkTaskService _workTaskService;
        private readonly IUserService _mainAppUserService;
        private readonly IDateTimeProvider _dateTimeProvider;
        public ProjectService(IProjectRepository projectRepository, IProjectUserService userService,
            IWorkTaskService workTaskService, IUserService mainAppUserService
            , ITaskStatusRepository taskStatusRepository, IDateTimeProvider dateTimeProvider
            , IProjectCachedRepository projectCacheRepository)
        {
            _projectRepository = projectRepository;
            _projectCacheRepository = projectCacheRepository;
            _projectUserService = userService;
            _workTaskService = workTaskService;
            _mainAppUserService = mainAppUserService;
            _taskStatusRepository = taskStatusRepository;
            _dateTimeProvider = dateTimeProvider;

        }

        public async Task<Project> CreateAsync(string name, long userId)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new SomeCustomException(Consts.ErrorConsts.EmptyProjectName);
            }
            //конечно не очень красиво отходить от репозиториев, но ладно
            var mainAppUserInfo = await _mainAppUserService.GetShortInfoAsync(userId);

            var user = new ProjectUser()
            {
                Role = UserRoleEnum.Admin,
                MainAppUserId = userId,
            };
            var project = await _projectCacheRepository.CreateAsync(name, user);
            return project;
        }

        public async Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId)
        {
            return await _projectRepository.GetProjectsByMainAppUserIdAsync(userId);
        }

        public async Task<Project> GetAsync(long id)
        {
            return await _projectCacheRepository.GetNoTrackAsync(id);
        }

        public async Task<Project> GetByIdIfAccessAsync(long id, long userId)
        {
            return await _projectCacheRepository.GetByIdIfAccessAsync(id, userId);
        }

        public async Task<Project> GetByIdIfAccessAdminAsync(long id, long userId)
        {
            return await _projectCacheRepository.GetByIdIfAccessAdminAsync(id, userId);
        }

        public async Task<(bool access, bool isAdmin)> ExistIfAccessAsync(long id, long userId)
        {
            return await _projectCacheRepository.ExistIfAccessAsync(id, userId);
        }

        public async Task<bool> ExistIfAccessAdminAsync(long id, long userId)
        {
            return await _projectCacheRepository.ExistIfAccessAdminAsync(id, userId);
        }


        public async Task<ProjectUser> CreateUserAsync(long projectId, long? mainAppUserId, long userId)
        {
            if (!await ExistIfAccessAdminAsync(projectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            var user = new ProjectUser()
            {
                ProjectId = projectId,
                MainAppUserId = mainAppUserId,
            };
            return await _projectUserService.CreateAsync(user);

        }


        public async Task<WorkTask> CreateTaskAsync(WorkTask task, long userId)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new SomeCustomException(Consts.ErrorConsts.EmptyTaskName);
            }

            if (!await ExistIfAccessAdminAsync(task.ProjectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }


            task.CreatorId = userId;
            //var creatorExist = await _projectUserService.ExistAsync(task.ProjectId, task.CreatorId);
            //if (!creatorExist)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.UserNotFound);
            //}

            if (task.ExecutorId != null)
            {
                var executorExist = await _projectUserService.ExistByMainAppUserIdAsync(task.ProjectId, task.ExecutorId.Value);
                if (!executorExist)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.UserNotFound);
                }
            }
            else
            {
                task.ExecutorId = userId;
            }



            var status = task.StatusId == null ? null : await _taskStatusRepository.GetAsync(task.StatusId.Value);
            if (status == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.WorkTaskStatusNotExists);

            }

            var newTask = new WorkTask()
            {
                CreatorId = task.CreatorId,
                Name = task.Name,
                ProjectId = task.ProjectId,
                ExecutorId = task.ExecutorId,
                CreateDate = _dateTimeProvider.CurrentDateTime(),
                LastUpdateDate = _dateTimeProvider.CurrentDateTime(),
                StatusId = task.StatusId,
            };

            //todo проверяем что creator+reviwer входит в проект. по идеи если что упадет с исключением
            return await _workTaskService.CreateAsync(newTask, userId);
        }

        public async Task<bool> DeleteAsync(long projectId, long userId)
        {
            var project = await _projectRepository.GetByIdIfAccessAdminAsync(projectId, userId);
            if (project == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
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
