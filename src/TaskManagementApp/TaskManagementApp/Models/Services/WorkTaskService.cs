
using BL.Models.Services;
using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using Microsoft.Extensions.Configuration;
using Pipelines.Sockets.Unofficial.Arenas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApp.Models.DTO;
using BL.Models.Services.Interfaces;

namespace TaskManagementApp.Models.Services
{
    public sealed class WorkTaskService : IWorkTaskService
    {
        private readonly IWorkTaskRepository _workTaskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IPresetRepository _presetRepository;
        private readonly ISprintRepository _sprintRepository;
        private readonly IWorkTaskLabelRepository _labelRepository;
        private readonly IProjectUserService _projectUserService;
        private readonly IWorkTaskCommentService _workTaskCommentService;
        private readonly ITaskManagementAppEmailService _taskManagementAppEmailService;
        private readonly ITaskStatusRepository _taskStatusRepository;
        private readonly IConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;

        public WorkTaskService(IWorkTaskRepository workTaskRepository,
            IProjectRepository projectRepository, IProjectUserService projectUserService
            , IWorkTaskCommentService workTaskCommentService, ITaskManagementAppEmailService taskManagementAppEmailService, ITaskStatusRepository taskStatusRepository
            , IConfiguration configuration, IDateTimeProvider dateTimeProvider, ISprintRepository sprintRepository, IWorkTaskLabelRepository labelRepository, IPresetRepository presetRepository)
        {
            _workTaskRepository = workTaskRepository;
            _projectRepository = projectRepository;
            _projectUserService = projectUserService;
            _workTaskCommentService = workTaskCommentService;
            _taskManagementAppEmailService = taskManagementAppEmailService;
            _taskStatusRepository = taskStatusRepository;
            _configuration = configuration;
            _dateTimeProvider = dateTimeProvider;
            _sprintRepository = sprintRepository;
            _labelRepository = labelRepository;
            _presetRepository = presetRepository;
        }

        public async Task<WorkTask> CreateAsync(WorkTask task, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new SomeCustomException(Consts.ErrorConsts.EmptyTaskName);
            }

            if (task.StatusId == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.WorkTaskStatusNotExists);
            }


            var addedTask = await _workTaskRepository.CreateAsync(task);
            await TaskChangedNotifyAsync(null, addedTask, userInfo);

            return addedTask;
        }

        public async Task<List<WorkTask>> GetTasksAsync(long projectId)
        {
            return await _workTaskRepository.GetTasksByProjectIdAsync(projectId);

        }

        /// <summary>
        /// без валидации
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="name"></param>
        /// <param name="creatorId"></param>
        /// <param name="executorId"></param>
        /// <param name="status"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<WorkTask>> GetTasksAsync(GetTasksByFilter filters)
        {
            if (filters.PresetId != null)
            {
                var preset = await _presetRepository.GetWithLabelsAsync(filters.PresetId.Value) ?? throw new SomeCustomException(Consts.ErrorConsts.PresetNotFound);
                filters.StatusId ??= preset.StatusId;
                filters.SprintId ??= preset.SprintId;
                filters.CreatorId ??= preset.CreatorId;
                filters.ExecutorId ??= preset.ExecutorId;
                filters.LabelIds ??= preset.Labels.Select(x => x.LabelId).ToList();
            }

            return await _workTaskRepository.GetTasksAsync(filters);
        }

        public async Task<long> GetTasksCountAsync(GetTasksCountByFilter filters)
        {
            if (filters.PresetId != null)
            {
                var preset = await _presetRepository.GetWithLabelsAsync(filters.PresetId.Value) ?? throw new SomeCustomException(Consts.ErrorConsts.PresetNotFound);
                filters.StatusId ??= preset.StatusId;
                filters.SprintId ??= preset.SprintId;
                filters.CreatorId ??= preset.CreatorId;
                filters.ExecutorId ??= preset.ExecutorId;
                filters.LabelIds ??= preset.Labels.Select(x => x.LabelId).ToList();
            }

            return await _workTaskRepository.GetTasksCountAsync(filters);
        }

        public async Task<WorkTask> UpdateAsync(WorkTask task, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new SomeCustomException(Consts.ErrorConsts.EmptyTaskName);
            }

            if (task.StatusId == null)
            {
                throw new SomeCustomBadRequestException(Consts.ErrorConsts.WorkTaskStatusNotExists);
            }


            var oldTask = await GetIfEditAccess(task.Id, userInfo);
            var prevTask = oldTask.CopyPlaneProp();

            if (task.StatusId != oldTask.StatusId)
            {
                var status = task.StatusId == null ? null : await _taskStatusRepository.GetAsync(task.StatusId.Value);
                if (status == null || status.ProjectId != oldTask.ProjectId)
                {
                    throw new SomeCustomException(Consts.ErrorConsts.WorkTaskStatusNotExists);
                }
            }


            if (task.ExecutorId != null && task.ExecutorId != oldTask.ExecutorId)
            {
                var executorExist = await _projectUserService.ExistAsync(oldTask.ProjectId, task.ExecutorId.Value);
                if (!executorExist)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.UserNotFound);
                }
            }


            oldTask.Status = task.Status;
            oldTask.Name = task.Name;
            //oldTask.Description = task.Description;
            oldTask.LastUpdateDate = _dateTimeProvider.CurrentDateTime();
            //upTask.CreatorId = task.CreatorId;
            oldTask.ExecutorId = task.ExecutorId;
            await _workTaskRepository.UpdateAsync(oldTask);

            await TaskChangedNotifyAsync(prevTask, oldTask, userInfo);

            return oldTask;
        }



        public async Task<WorkTask> UpdateNameAsync(long id, string name, UserInfo userInfo)
        {

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new SomeCustomException(Consts.ErrorConsts.EmptyTaskName);
            }


            var upTask = await GetIfEditAccess(id, userInfo);
            var oldTask = upTask.CopyPlaneProp();
            upTask.Name = name;
            await _workTaskRepository.UpdateAsync(upTask);
            await TaskChangedNotifyAsync(oldTask, upTask, userInfo);
            return upTask;

        }

        public async Task<WorkTask> UpdateDescriptionAsync(long id, string description, UserInfo userInfo)
        {

            var upTask = await GetIfEditAccess(id, userInfo);
            var oldTask = upTask.CopyPlaneProp();
            upTask.Description = description;
            await _workTaskRepository.UpdateAsync(upTask);
            await TaskChangedNotifyAsync(oldTask, upTask, userInfo);
            return upTask;
        }

        public async Task<WorkTask> UpdateStatusAsync(long id, long statusId, UserInfo userInfo)
        {

            var upTask = await GetIfEditAccess(id, userInfo);

            if (statusId != upTask.StatusId)
            {
                var status = await _taskStatusRepository.GetAsync(statusId);
                if (status == null || status.ProjectId != upTask.ProjectId)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.WorkTaskStatusNotExists);
                }

                upTask.StatusId = statusId;
                await _workTaskRepository.UpdateAsync(upTask);

                await StatusChangedNotifyAsync(upTask, userInfo);

                return upTask;
            }

            return null;
        }

        public async Task<WorkTask> UpdateExecutorAsync(long id, long executorId, UserInfo userInfo)
        {
            var upTask = await GetIfEditAccess(id, userInfo);
            var oldTask = upTask.CopyPlaneProp();

            var executorExist = await _projectUserService.ExistAsync(upTask.ProjectId, executorId);
            if (!executorExist)
            {
                throw new SomeCustomException(Consts.ErrorConsts.UserNotFound);
            }

            upTask.ExecutorId = executorId;
            await _workTaskRepository.UpdateAsync(upTask);
            await TaskChangedNotifyAsync(oldTask, upTask, userInfo);
            return upTask;
        }

        public async Task<WorkTask> DeleteIfAccess(long id, UserInfo userInfo)
        {
            var task = await GetByIdIfAccessAsync(id, userInfo);
            return await _workTaskRepository.DeleteAsync(task);
        }

        public async Task<List<WorkTaskComment>> GetCommentsAsync(long taskId, UserInfo userInfo)
        {
            var task = await _workTaskRepository.GetTaskWithCommentsAsync(taskId) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);

            var projectAccessed = await _projectRepository.ExistIfAccessAsync(task.ProjectId, userInfo.UserId);
            if (!projectAccessed.access)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return task.Comments;
        }

        public async Task<WorkTask> GetByIdIfAccessAsync(long id, UserInfo userInfo)
        {
            var task = await _workTaskRepository.GetAccessAsync(id, userInfo.UserId) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            //var task = await _workTaskRepository.GetNoTrackAsync(id);

            //var projectAccessed = await _projectRepository.ExistIfAccessAsync(task.ProjectId, userInfo.UserId);
            //if (!projectAccessed.access)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            //}

            return task;
        }

        public async Task<WorkTaskComment> CreateCommentAsync(long taskId, string text, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new SomeCustomException(Consts.ErrorConsts.WorkTaskEmptyStatusName);
            }

            //todo много запросов что то получается
            var task = await GetByIdIfAccessAsync(taskId, userInfo);
            var projectUserId = await _projectUserService.GetIdByMainAppIdAsync(userInfo, task.ProjectId) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskHaveNoAccess);

            var newComment = new WorkTaskComment() { CreatorId = projectUserId, TaskId = taskId, Text = text, CreateDate = _dateTimeProvider.CurrentDateTime() };
            var comment = await _workTaskCommentService.CreateAsync(newComment);
            var emails = await GetTaskUsersForNotificationAsync(task, userInfo);

            await _taskManagementAppEmailService.QueueNewCommentInTaskAsync(emails, task.Name, GetTaskUrl(task));
            return comment;
        }

        public async Task<bool> ExistAsync(long projectId, long statusId)
        {
            return await _workTaskRepository.ExistAsync(projectId, statusId);
        }

        public async Task<WorkTask> GetTaskAsync(long id)
        {
            return await _workTaskRepository.GetNoTrackAsync(id);
        }

        public async Task<WorkTask> GetTaskWithCommentsAsync(long id)
        {
            return await _workTaskRepository.GetTaskWithCommentsAsync(id);
        }

        public async Task<WorkTask> GetIfEditAccess(long id, UserInfo userInfo)
        {
            var task = await _workTaskRepository.GetAsync(id) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);

            var canAddToProject = await _projectRepository.ExistIfAccessAsync(task.ProjectId, userInfo.UserId);
            if (!canAddToProject.access)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectHaveNoAccess);
            }

            //тк любой человек в проекте должен иметь возможноть поменять название или исполнителя, это убрал
            //if (task.CreatorEntityId != userInfo.UserId && !canAddToProject.isAdmin)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.TaskHaveNoAccess);
            //}

            return task;
        }


        public async Task<WorkTask> GetTaskFullAsync(long id)
        {
            return await _workTaskRepository.GetTaskFullAsync(id);
        }

        public async Task<WorkTask> CopyAsync(long id, UserInfo userInfo)
        {
            var task = await _workTaskRepository.GetTaskFullAsync(id) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            var s = await ExistIfAccessAdminAsync(task.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            var projUser = await _projectUserService.GetIdByMainAppIdAsync(userInfo,task.ProjectId);
            var newTask = new WorkTask()
            {
                CreateDate = _dateTimeProvider.CurrentDateTime(),
                CreatorEntityId = userInfo.UserId,
                CreatorId = projUser.Value,
                Description = task.Description,
                ExecutorId = task.ExecutorId,
                LastUpdateDate = _dateTimeProvider.CurrentDateTime(),
                Name = task.Name,
                ProjectId = task.ProjectId,
                StatusId = task.StatusId,
            };

            await _workTaskRepository.AddAsync(newTask);

            newTask.Labels = task.Labels.Select(x => new WorkTaskLabelTaskRelation() { LabelId = x.LabelId, TaskId = newTask.Id }).ToList();
            newTask.Sprints = task.Sprints.Select(x => new WorkTaskSprintRelation() { SprintId = x.SprintId, TaskId = newTask.Id }).ToList();
            await _workTaskRepository.UpdateAsync(newTask);
            return newTask;
        }


        public async Task<TaskRelation> CreateRelationAsync(TaskRelation req, UserInfo userInfo)
        {
            if (req.MainWorkTaskId == req.SubWorkTaskId)
            {
                throw new SomeCustomBadRequestException(Consts.ErrorConsts.RelationError);
            }
            var mainTask = await GetIfEditAccess(req.MainWorkTaskId, userInfo);
            var subTask = await GetIfEditAccess(req.SubWorkTaskId, userInfo);
            if (mainTask.ProjectId != subTask.ProjectId)
            {
                throw new SomeCustomBadRequestException(Consts.ErrorConsts.RelationError);
            }

            if (await _workTaskRepository.ExistsRelationAsync(req.MainWorkTaskId, req.SubWorkTaskId))
            {
                throw new SomeCustomBadRequestException(Consts.ErrorConsts.RelationError);

            }

            return await _workTaskRepository.CreateRelationAsync(req);

        }

        public async Task<TaskRelation> DeleteRelationAsync(long relationId, UserInfo userInfo)
        {
            var relation = await _workTaskRepository.GetRelationAsync(relationId);
            if(relation == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.RelationNotFound);

            }
            _ = await GetIfEditAccess(relation.MainWorkTaskId, userInfo);

            return await _workTaskRepository.DeleteRelationAsync(relation);
        }


        public async Task<List<TaskRelation>> GetRelationsAsync(long taskId, UserInfo userInfo)
        {
            var task = await _workTaskRepository.GetAccessRelationsAsync(taskId, userInfo.UserId) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            var res = new List<TaskRelation>();
            res.AddRange(task.MainWorkTasksRelation);
            res.AddRange(task.SubWorkTasksRelation);
            return res;
        }

        public async Task<GetProjectTaskSelectInfo> GetProjectTaskSelectInfoAsync(long id, UserInfo userInfo)
        {
            return await _workTaskRepository.GetAccessSelectInfoAsync(id, userInfo.UserId) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
        }


        #region вспомогательные методы
        private async Task StatusChangedNotifyAsync(WorkTask upTask, UserInfo userInfo)
        {
            var users = await GetTaskUsersForNotificationAsync(upTask, userInfo);
            var status = upTask.Status;
            if (status == null && upTask.StatusId.HasValue)
            {
                status = await _taskStatusRepository.GetAsync(upTask.StatusId.Value);
            }

            if (status == null)
            {
                //todo по идеи такого не должно быть, какой то статус должен быть обязательно
                return;
            }

            var taskUrl = GetTaskUrl(upTask);
            await _taskManagementAppEmailService.QueueChangeStatusTaskAsync(users, upTask.Name, status.Name, taskUrl);

        }

        private async Task TaskChangedNotifyAsync(WorkTask prevTask, WorkTask upTask, UserInfo userInfo)
        {
            //todo тут потенциально одни и теже пользаки 2 раза грузятся GetTaskUsersForNotificationAsync, надо переписать
            var usersPrev = await GetTaskUsersForNotificationAsync(prevTask, userInfo);
            var usersNow = await GetTaskUsersForNotificationAsync(upTask, userInfo);
            var users = new List<string>(usersPrev);
            users.AddRange(usersNow);
            users = users.Distinct().ToList();
            var changes = new List<EmailServiceBase.Changes>();

            if (prevTask?.Description != upTask.Description)
            {
                changes.Add(new EmailServiceBase.Changes() { PropName = "Описание", PropPrevValue = prevTask?.Description, PropNewValue = upTask.Description });
            }

            if (prevTask?.Name != upTask.Name)
            {
                changes.Add(new EmailServiceBase.Changes() { PropName = "Название", PropPrevValue = prevTask?.Name, PropNewValue = upTask.Name });
            }

            if (prevTask?.ExecutorId != upTask.ExecutorId)
            {
                var prevExecutor = prevTask?.Executor;
                var newExecutor = upTask.Executor;
                var executorforLoad = new List<long>();
                if (prevExecutor == null && prevTask?.ExecutorId != null)
                {
                    executorforLoad.Add(prevTask.ExecutorId.Value);
                }

                if (newExecutor == null && upTask.ExecutorId.HasValue)
                {
                    executorforLoad.Add(upTask.ExecutorId.Value);
                }

                var executors = await _projectUserService.GetProjectUserAsync(upTask.ProjectId, executorforLoad);

                prevExecutor = prevExecutor ?? executors.FirstOrDefault(x => x.Id == prevTask?.ExecutorId);
                newExecutor = newExecutor ?? executors.FirstOrDefault(x => x.Id == upTask.ExecutorId);
                changes.Add(new EmailServiceBase.Changes()
                {
                    PropName = "Исполнитель",
                    PropPrevValue = prevExecutor?.NotifyEmail,//todo тут не уверен что эти почты надо брать
                    PropNewValue = newExecutor?.NotifyEmail
                });
            }

            if (prevTask?.StatusId != upTask.StatusId)
            {
                var newStatus = upTask.Status;
                var oldStatus = prevTask?.Status;

                if (oldStatus == null && prevTask?.StatusId != null)
                {
                    oldStatus = await _taskStatusRepository.GetAsync(prevTask.StatusId.Value);
                }

                if (newStatus == null && upTask.StatusId.HasValue)
                {
                    newStatus = await _taskStatusRepository.GetAsync(upTask.StatusId.Value);
                }

                changes.Add(new EmailServiceBase.Changes() { PropName = "Статус", PropPrevValue = oldStatus?.Name, PropNewValue = newStatus?.Name });
            }


            var taskUrl = GetTaskUrl(upTask);
            await _taskManagementAppEmailService.QueueChangeTaskAsync(users, upTask.Name, changes, taskUrl);

        }


        private async Task<List<string>> GetTaskUsersForNotificationAsync(WorkTask upTask, UserInfo userInfo)
        {
            //todo я не понимаю почему у юзера в код ревью может быть пустым us.mainAppId, это ошибка?
            var emails = new List<(long? id, string email)>();
            if (upTask == null)
            {
                return emails.Select(x => x.email).ToList();
            }

            if (upTask.ExecutorId.HasValue)
            {
                var us = await _projectUserService.GetNotificationEmailWithMainAppIdAsync(upTask.ExecutorId.Value);
                emails.Add((us.mainAppId, us.email));
            }

            if (upTask.CreatorId != upTask.ExecutorId)
            {
                var us = await _projectUserService.GetNotificationEmailWithMainAppIdAsync(upTask.CreatorId);
                emails.Add((us.mainAppId, us.email));
            }


            //если юзер редачит таску то ему отправлять не надо
            return emails.Where(x => x.id != userInfo.UserId && !string.IsNullOrEmpty(x.email))
                .Select(x => x.email).Distinct().ToList();
        }



        private string GetTaskUrl(WorkTask task)
        {
            if (task == null)
            {
                return "";
            }

            var baseUrl = _configuration["SiteSettings:BaseUrl"];
            var endpoint = $"task-management/proj-{task.ProjectId}/task-{task.Id}";
            var fullUri = new Uri(new Uri(baseUrl), endpoint).ToString();
            return fullUri;
        }

        private async Task<bool> ExistIfAccessAdminAsync(long id, UserInfo userInfo)
        {
            return await _projectRepository.ExistIfAccessAdminAsync(id, userInfo.UserId);
        }






        #endregion вспомогательные методы




    }
}
