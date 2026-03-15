using TaskManagementApp.Models.Services.Interfaces;
using System.Collections.Generic;
using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models.Exceptions;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace TaskManagementApp.Models.Services
{
    public class WorkTaskStatusService : IWorkTaskStatusService
    {

        private readonly IProjectRepository _projectRepository;
        private readonly IProjectCachedRepository _projectCacheRepository;
        private readonly ITaskStatusRepository _taskStatusRepository;
        private readonly IWorkTaskRepository _workTaskRepository;

        public WorkTaskStatusService(IProjectRepository projectRepository, ITaskStatusRepository taskStatusRepository
            , IWorkTaskRepository workTaskRepository, IProjectCachedRepository projectCacheRepository)
        {
            _projectRepository = projectRepository;
            _taskStatusRepository = taskStatusRepository;
            _workTaskRepository = workTaskRepository;
            _projectCacheRepository = projectCacheRepository;
        }


        public async Task<List<WorkTaskStatus>> GetStatusesAsync(long projectId)
        {
            return await _taskStatusRepository.GetForProjectAsync(projectId);

        }


        public async Task<List<WorkTaskStatus>> GetStatusesAccessAsync(long projectId, long userId)
        {
            var s = await ExistIfAccessAsync(projectId, userId);
            if (!s.access)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await GetStatusesAsync(projectId);

        }

        public async Task<WorkTaskStatus> CreateStatusAsync(string status, long projectId, long userId)
        {
            var s = await ExistIfAccessAdminAsync(projectId, userId);
            if (!s)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await _taskStatusRepository.AddAsync(new WorkTaskStatus() { Name = status, ProjectId = projectId });
        }

        public async Task<WorkTaskStatus> DeleteStatusAsync(long statusId, long userId)
        {
            var status = await _taskStatusRepository.GetAsync(statusId);
            if (status == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.WorkTaskStatusNotExists);
            }

            var s = await ExistIfAccessAdminAsync(status.ProjectId, userId);
            if (!s)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            var taskExists = await _workTaskRepository.ExistAsync(status.ProjectId, statusId);
            if (taskExists)
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskWithStatusExists);

            }

            return await _taskStatusRepository.DeleteAsync(status);
        }

        public async Task<WorkTaskStatus> UpdateStatusAsync(long statusId, string status, long userId)
        {

            var statusEntity = await _taskStatusRepository.GetAsync(statusId);
            if (statusEntity == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.WorkTaskStatusNotExists);
            }

            var s = await ExistIfAccessAdminAsync(statusEntity.ProjectId, userId);
            if (!s)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            statusEntity.Name = status;
            var res = await _taskStatusRepository.UpdateAsync(statusEntity);
            return res;

        }

        private async Task<(bool access, bool isAdmin)> ExistIfAccessAsync(long id, long userId)
        {
            return await _projectCacheRepository.ExistIfAccessAsync(id, userId);
        }

        private async Task<bool> ExistIfAccessAdminAsync(long id, long userId)
        {
            return await _projectCacheRepository.ExistIfAccessAdminAsync(id, userId);
        }
    }
}
