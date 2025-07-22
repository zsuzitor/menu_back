using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models.Exceptions;
using Pipelines.Sockets.Unofficial.Arenas;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;

namespace TaskManagementApp.Models.Services
{
    internal class SprintService : ISprintService
    {
        private readonly ISprintRepository _sprintRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkTaskRepository _workTaskRepository;
        public SprintService(ISprintRepository sprintRepository, IProjectRepository projectRepository
            , IWorkTaskRepository workTaskRepository)
        {
            _sprintRepository = sprintRepository;
            _projectRepository = projectRepository;
            _workTaskRepository = workTaskRepository;
        }

        public async Task<bool> AddTaskToSprint(long sprintId, long taskId, UserInfo userInfo)
        {
            var sprint = await _sprintRepository.GetAsync(sprintId);
            if (sprint == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.SprintNotFound);
            }

            var s = await ExistIfAccessAdminAsync(sprint.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            var task = await _workTaskRepository.GetAsync(taskId);
            task.SprintId = sprintId;
            await _workTaskRepository.UpdateAsync(task);
            return true;

        }

        public async Task<ProjectSprint> Create(long projectId, string name, UserInfo userInfo)
        {

            var s = await ExistIfAccessAdminAsync(projectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await _sprintRepository.AddAsync(new ProjectSprint() { Name = name, ProjectId = projectId });
        }

        public async Task<ProjectSprint> Delete(long id, UserInfo userInfo)
        {
            var sprint = await _sprintRepository.GetAsync(id);
            if (sprint == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.SprintNotFound);
            }

            var s = await ExistIfAccessAdminAsync(sprint.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            await _sprintRepository.DeleteAsync(sprint);
            return sprint;
        }

        public async Task<bool> DeleteTaskFromSprint(long sprintId, long taskId, UserInfo userInfo)
        {

            var sprint = await _sprintRepository.GetAsync(sprintId);
            if (sprint == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.SprintNotFound);
            }

            var s = await ExistIfAccessAdminAsync(sprint.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            var task = await _workTaskRepository.GetAsync(taskId);
            task.SprintId = null;
            await _workTaskRepository.UpdateAsync(task);

            return true;
        }

        private async Task<(bool access, bool isAdmin)> ExistIfAccessAsync(long id, UserInfo userInfo)
        {
            return await _projectRepository.ExistIfAccessAsync(id, userInfo.UserId);
        }

        private async Task<bool> ExistIfAccessAdminAsync(long id, UserInfo userInfo)
        {
            return await _projectRepository.ExistIfAccessAdminAsync(id, userInfo.UserId);
        }
    }
}
