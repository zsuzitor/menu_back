using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;

namespace TaskManagementApp.Models.Services
{
    public class WorkTaskStatusService : IWorkTaskStatusService
    {

        private readonly ITaskStatusCachedRepository _taskStatusRepository;
        private readonly IWorkTaskRepository _workTaskRepository;
        private readonly ITasksManagmentAuthRepository _auth;

        public WorkTaskStatusService( ITaskStatusCachedRepository taskStatusRepository
            , IWorkTaskRepository workTaskRepository,  ITasksManagmentAuthRepository auth)
        {
            _taskStatusRepository = taskStatusRepository;
            _workTaskRepository = workTaskRepository;
            _auth = auth;
        }


        public async Task<List<WorkTaskStatus>> GetStatusesAsync(long projectId)
        {
            return await _taskStatusRepository.GetForProjectAsync(projectId);

        }


        public async Task<List<WorkTaskStatus>> GetStatusesAccessAsync(long projectId, long userId)
        {
            if (!await _auth.CanViewProject(projectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await GetStatusesAsync(projectId);

        }

        public async Task<WorkTaskStatus> CreateStatusAsync(string status, long projectId, long userId)
        {
            if (!await _auth.CanEditProject(projectId, userId))
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

            if (!await _auth.CanEditProject(status.ProjectId, userId))
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

            if (!await _auth.CanEditProject(statusEntity.ProjectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            statusEntity.Name = status;
            var res = await _taskStatusRepository.UpdateAsync(statusEntity);
            return res;

        }

    }
}
