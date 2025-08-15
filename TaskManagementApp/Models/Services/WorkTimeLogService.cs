using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models.Exceptions;
using Pipelines.Sockets.Unofficial.Arenas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;

namespace TaskManagementApp.Models.Services
{
    public class WorkTimeLogService : IWorkTimeLogService
    {
        private readonly IWorkTimeLogRepository _workTimeLogRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkTaskRepository _workTaskRepository;
        private readonly IProjectUserRepository _projectUserRepository;


        public WorkTimeLogService(IWorkTimeLogRepository workTimeLogRepository, IProjectRepository projectRepository
            , IWorkTaskRepository workTaskRepository, IProjectUserRepository projectUserRepository)
        {
            _workTimeLogRepository = workTimeLogRepository;
            _projectRepository = projectRepository;
            _workTaskRepository = workTaskRepository;
            _projectUserRepository = projectUserRepository;
        }

        public async Task<WorkTimeLog> CreateAsync(WorkTimeLog obj, UserInfo userInfo)
        {
            if (obj.DayOfLog == default)
            {
                throw new SomeCustomException(Consts.ErrorConsts.WorkTaskTimeLogValidationError);

            }
            if (obj.WorkTaskId <= 0)
            {
                throw new SomeCustomException(Consts.ErrorConsts.WorkTaskTimeLogValidationError);
            }

            if (obj.TimeMinutes <= 0 && obj.RangeStartOfLog == null && obj.RangeEndOfLog == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.WorkTaskTimeLogValidationError);
            }

            if (obj.DayOfLog.Date != obj.RangeStartOfLog || obj.DayOfLog.Date != obj.RangeEndOfLog)
            {
                throw new SomeCustomException(Consts.ErrorConsts.WorkTaskTimeLogIntervalValidationError);
            }

            var userId = await _workTaskRepository.GetUserIdAccessAsync(obj.WorkTaskId, userInfo.UserId);
            if (userId == 0)
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            }

            obj.ProjectUserId = userId;
            obj.CreationTime = DateTime.Now;

            return await _workTimeLogRepository.AddAsync(obj);
        }

        public async Task<WorkTimeLog> DeleteAsync(long id, UserInfo userInfo)
        {
            var time = await _workTimeLogRepository.GetTimeAccessAsync(id, userInfo);
            //var time = await _workTimeLogRepository.GetAsync(id);
            if (time == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskLogTimeNotFound);
            }

            //var access = await _workTaskRepository.HaveAccessAsync(time.WorkTaskId, userInfo.UserId);
            //if (!access)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.TaskLogTimeNotFound);

            //}

            return await _workTimeLogRepository.DeleteAsync(time);

        }

        public async Task<WorkTimeLog> EditAsync(WorkTimeLog obj, UserInfo userInfo)
        {
            var time = await _workTimeLogRepository.GetTimeAccessAsync(obj.Id, userInfo);
            //var time = await _workTimeLogRepository.GetAsync(obj.Id);
            if (time == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskLogTimeNotFound);
            }

            //var access = await _workTaskRepository.HaveAccessAsync(time.WorkTaskId, userInfo.UserId);
            //if (!access)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.TaskLogTimeNotFound);

            //}

            time.DayOfLog = obj.DayOfLog;
            time.TimeMinutes = obj.TimeMinutes;
            time.Comment = obj.Comment;

            return await _workTimeLogRepository.UpdateAsync(time);

        }

        public async Task<WorkTimeLog> GetAsync(long id, UserInfo userInfo)
        {
            var time = await _workTimeLogRepository.GetTimeAccessAsync(id, userInfo);
            //var time = await _workTimeLogRepository.GetAsync(id);
            if (time == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskLogTimeNotFound);
            }

            //var access = await _workTaskRepository.HaveAccessAsync(time.WorkTaskId, userInfo.UserId);
            //if (!access)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.TaskLogTimeNotFound);

            //}

            return time;

        }

        public async Task<List<WorkTimeLog>> GetTimeForProjectAsync(long projectId, DateTime startDate, DateTime endDate, UserInfo userInfo, long? userId)
        {
            var access = await _projectRepository.ExistIfAccessAsync(projectId, userInfo.UserId);
            if (!access.access)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectHaveNoAccess);

            }

            return await _workTimeLogRepository.GetTimeForProjectAsync(projectId, startDate, endDate, userId);
        }

        public async Task<List<WorkTimeLog>> GetTimeForTaskAsync(long taskId, UserInfo userInfo)
        {
            var access = await _workTaskRepository.HaveAccessAsync(taskId, userInfo.UserId);
            if (!access)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectHaveNoAccess);
            }

            return await _workTimeLogRepository.GetTimeForTaskAsync(taskId);

        }

        public async Task<List<WorkTimeLog>> GetTimeForUserAsync(long? userId, DateTime startDate, DateTime endDate, UserInfo userInfo)
        {

            return await _workTimeLogRepository.GetTimeForUserAsync(userId, startDate, endDate, userInfo);
        }
    }
}
