using TaskManagementApp.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models.Exceptions;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using Menu.Models.Services;
using TaskManagementApp.Models.DAL.Repositories;
using Pipelines.Sockets.Unofficial.Arenas;

namespace TaskManagementApp.Models.Services
{
    internal class WorkTaskStatusService : IWorkTaskStatusService
    {

        private readonly IProjectRepository _projectRepository;
        private readonly ITaskStatusRepository _taskStatusRepository;
        private readonly IWorkTaskRepository _workTaskRepository;

        public WorkTaskStatusService(IProjectRepository projectRepository, ITaskStatusRepository taskStatusRepository
            , IWorkTaskRepository workTaskRepository)
        {
            _projectRepository = projectRepository;
            _taskStatusRepository = taskStatusRepository;
            _workTaskRepository = workTaskRepository;

        }


        public async Task<List<WorkTaskStatus>> GetStatusesAsync(long projectId, UserInfo userInfo)
        {
            return await _taskStatusRepository.GetForProjectAsync(projectId);

        }


        public async Task<List<WorkTaskStatus>> GetStatusesAccessAsync(long projectId, UserInfo userInfo)
        {
            var s = await ExistIfAccessAsync(projectId, userInfo);
            if (!s.access)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await GetStatusesAsync(projectId, userInfo);

        }

        public async Task<WorkTaskStatus> CreateStatusAsync(string status, long projectId, UserInfo userInfo)
        {
            var s = await ExistIfAccessAdminAsync(projectId, userInfo);
            if (!s)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await _taskStatusRepository.AddAsync(new WorkTaskStatus() { Name = status, ProjectId = projectId });
        }

        public async Task<WorkTaskStatus> DeleteStatusAsync(long statusId, UserInfo userInfo)
        {
            var status = await _taskStatusRepository.GetAsync(statusId);
            if (status == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.WorkTaskStatusNotExists);
            }

            var s = await ExistIfAccessAdminAsync(status.ProjectId, userInfo);
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

        public async Task<WorkTaskStatus> UpdateStatusAsync(long statusId, string status, UserInfo userInfo)
        {

            var statusEntity = await _taskStatusRepository.GetAsync(statusId);
            if (statusEntity == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.WorkTaskStatusNotExists);
            }

            var s = await ExistIfAccessAdminAsync(statusEntity.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            statusEntity.Name = status;
            var res = await _taskStatusRepository.UpdateAsync(statusEntity);
            return res;

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
