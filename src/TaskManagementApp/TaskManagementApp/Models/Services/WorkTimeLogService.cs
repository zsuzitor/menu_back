using BL.Models.Services.Interfaces;
using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;

namespace TaskManagementApp.Models.Services
{
    public class WorkTimeLogService : IWorkTimeLogService
    {
        private readonly IWorkTimeLogRepository _workTimeLogRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectCachedRepository _projectCacheRepository;
        private readonly IWorkTaskRepository _workTaskRepository;
        private readonly IProjectUserRepository _projectUserRepository;
        private readonly IDateTimeProvider _dateTimeProvider;


        public WorkTimeLogService(IWorkTimeLogRepository workTimeLogRepository, IProjectRepository projectRepository
            , IWorkTaskRepository workTaskRepository, IProjectUserRepository projectUserRepository
            , IDateTimeProvider dateTimeProvider, IProjectCachedRepository projectCacheRepository)
        {
            _workTimeLogRepository = workTimeLogRepository;
            _projectRepository = projectRepository;
            _workTaskRepository = workTaskRepository;
            _projectUserRepository = projectUserRepository;
            _dateTimeProvider = dateTimeProvider;
            _projectCacheRepository = projectCacheRepository;
        }

        public async Task<WorkTimeLog> CreateAsync(WorkTimeLog obj, long userId)
        {
            ValidateTimeLog(obj);

            userId = await _workTaskRepository.GetUserIdAccessAsync(obj.WorkTaskId, userId);
            if (userId == 0)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.TaskNotFound);
            }

            obj.ProjectUserId = userId;
            obj.CreationTime = _dateTimeProvider.CurrentDateTime();

            return await _workTimeLogRepository.AddAsync(obj);
        }

        public async Task<WorkTimeLog> DeleteAsync(long id, long userId)
        {
            var time = await _workTimeLogRepository.GetTimeAccessAsync(id, userId);
            //var time = await _workTimeLogRepository.GetAsync(id);
            if (time == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.TaskLogTimeNotFound);
            }

            //var access = await _workTaskRepository.HaveAccessAsync(time.WorkTaskId, userInfo.UserId);
            //if (!access)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.TaskLogTimeNotFound);

            //}

            return await _workTimeLogRepository.DeleteAsync(time);

        }

        public async Task<WorkTimeLog> EditAsync(WorkTimeLog obj, long userId)
        {
            ValidateTimeLog(obj);

            var time = await _workTimeLogRepository.GetTimeAccessAsync(obj.Id, userId);
            //var time = await _workTimeLogRepository.GetAsync(obj.Id);
            if (time == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.TaskLogTimeNotFound);
            }


            //var access = await _workTaskRepository.HaveAccessAsync(time.WorkTaskId, userInfo.UserId);
            //if (!access)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.TaskLogTimeNotFound);

            //}

            time.DayOfLog = obj.DayOfLog;
            time.TimeMinutes = obj.TimeMinutes;
            time.Comment = obj.Comment;
            time.RangeStartOfLog = obj.RangeStartOfLog;
            time.RangeEndOfLog = obj.RangeEndOfLog;

            return await _workTimeLogRepository.UpdateAsync(time);

        }

        public async Task<WorkTimeLog> GetAsync(long id, long userId)
        {
            var time = await _workTimeLogRepository.GetTimeAccessAsync(id, userId);
            //var time = await _workTimeLogRepository.GetAsync(id);
            if (time == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.TaskLogTimeNotFound);
            }

            //var access = await _workTaskRepository.HaveAccessAsync(time.WorkTaskId, userInfo.UserId);
            //if (!access)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.TaskLogTimeNotFound);

            //}

            return time;

        }

        public async Task<List<WorkTimeLog>> GetTimeForProjectAsync(long projectId, DateTime startDate, DateTime endDate, long currentUserId, long? userId)
        {
            var access = await _projectCacheRepository.ExistIfAccessAsync(projectId, currentUserId);
            if (!access.access)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectHaveNoAccess);
            }

            return await _workTimeLogRepository.GetTimeForProjectAsync(projectId, startDate, endDate, userId);
        }

        public async Task<List<WorkTimeLog>> GetTimeForTaskAsync(long taskId, long userId)
        {
            var access = await _workTaskRepository.HaveAccessAsync(taskId, userId);
            if (!access)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectHaveNoAccess);
            }

            return await _workTimeLogRepository.GetTimeForTaskAsync(taskId);

        }

        public async Task<List<WorkTimeLog>> GetTimeForUserAsync(long? userId, DateTime startDate, DateTime endDate, long currentUserId)
        {
            return await _workTimeLogRepository.GetTimeForUserAsync(userId, startDate, endDate, currentUserId);
        }


        private void ValidateTimeLog(WorkTimeLog obj)
        {
            if (obj.DayOfLog == default)
            {
                throw new SomeCustomException(Consts.ErrorConsts.WorkTaskTimeLogValidationError);
            }

            if (obj.WorkTaskId <= 0)
            {
                throw new SomeCustomException(Consts.ErrorConsts.WorkTaskTimeLogValidationError);
            }

            if (obj.TimeMinutes <= 0 && (obj.RangeStartOfLog == null || obj.RangeEndOfLog == null))
            {
                throw new SomeCustomException(Consts.ErrorConsts.WorkTaskTimeLogValidationError);
            }

            if (obj.RangeEndOfLog.HasValue || obj.RangeStartOfLog.HasValue)
            {
                if (obj.DayOfLog.Date != obj.RangeStartOfLog.Value.Date || obj.DayOfLog.Date != obj.RangeEndOfLog.Value.Date)
                {
                    throw new SomeCustomException(Consts.ErrorConsts.WorkTaskTimeLogIntervalValidationError);
                }

                var timeMinutes = obj.RangeEndOfLog.Value - obj.RangeStartOfLog.Value;
                obj.TimeMinutes = (int)timeMinutes.TotalMinutes;
            }
        }
    }
}
