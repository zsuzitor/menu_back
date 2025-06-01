
using BL.Models.Services;
using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using BO.Models.DAL.Domain;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using Microsoft.Extensions.Configuration;
using Pipelines.Sockets.Unofficial.Arenas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services
{
    public sealed class WorkTaskService : IWorkTaskService
    {
        private readonly IWorkTaskRepository _workTaskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserService _projectUserService;
        private readonly IWorkTaskCommentService _workTaskCommentService;
        private readonly ITaskManagementAppEmailService _taskManagementAppEmailService;
        private readonly ITaskStatusRepository _taskStatusRepository;
        private readonly IConfiguration _configuration;
        public WorkTaskService(IWorkTaskRepository workTaskRepository,
            IProjectRepository projectRepository, IProjectUserService projectUserService
            , IWorkTaskCommentService workTaskCommentService, ITaskManagementAppEmailService taskManagementAppEmailService, ITaskStatusRepository taskStatusRepository
            , IConfiguration configuration)
        {
            _workTaskRepository = workTaskRepository;
            _projectRepository = projectRepository;
            _projectUserService = projectUserService;
            _workTaskCommentService = workTaskCommentService;
            _taskManagementAppEmailService = taskManagementAppEmailService;
            _taskStatusRepository = taskStatusRepository;
            _configuration = configuration;
        }

        public async Task<WorkTask> CreateAsync(WorkTask task, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new SomeCustomException(Consts.ErrorConsts.EmptyTaskName);
            }

            if (task.StatusId == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.WorkTaskStatusNotExists);
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
        public async Task<List<WorkTask>> GetTasksAsync(long projectId, string name, long? creatorId
            , long? executorId, long? statusId, int pageNumber, int pageSize)
        {
            return await _workTaskRepository.GetTasksAsync(projectId, name
                , creatorId, executorId, statusId, pageNumber, pageSize);
        }

        public async Task<long> GetTasksCountAsync(long projectId, string name, long? creatorId
            , long? executorId, long? statusId)
        {
            return await _workTaskRepository.GetTasksCountAsync(projectId, name
                , creatorId, executorId, statusId);
        }

        public async Task<WorkTask> UpdateAsync(WorkTask task, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new SomeCustomException(Consts.ErrorConsts.EmptyTaskName);
            }

            if (task.StatusId == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.WorkTaskStatusNotExists);
            }


            var upTask = await GetIfEditAccess(task.Id, userInfo);
            var prevTask = upTask.CopyPlaneProp();

            if (task.StatusId != upTask.StatusId)
            {
                var status = task.StatusId == null ? null : await _taskStatusRepository.GetAsync(task.StatusId.Value);
                if (status == null || status.ProjectId != upTask.ProjectId)
                {
                    throw new SomeCustomException(Consts.ErrorConsts.WorkTaskStatusNotExists);
                }
            }


            if (task.ExecutorId != null)
            {
                var executorExist = await _projectUserService.ExistAsync(upTask.ProjectId, task.ExecutorId.Value);
                if (!executorExist)
                {
                    throw new SomeCustomException(Consts.ErrorConsts.UserNotFound);
                }
            }


            upTask.Status = task.Status;
            upTask.Name = task.Name;
            upTask.Description = task.Description;
            upTask.LastUpdateDate = DateTime.Now;
            //upTask.CreatorId = task.CreatorId;
            upTask.ExecutorId = task.ExecutorId;
            await _workTaskRepository.UpdateAsync(upTask);

            await TaskChangedNotifyAsync(prevTask, upTask, userInfo);

            return upTask;
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
                    throw new SomeCustomException(Consts.ErrorConsts.WorkTaskStatusNotExists);
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
            var task = await _workTaskRepository.GetTaskWithCommentsAsync(taskId);
            if (task == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            }

            var projectAccessed = await _projectRepository.ExistIfAccessAsync(task.ProjectId, userInfo.UserId);
            if (!projectAccessed.access)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return task.Comments;
        }

        public async Task<WorkTask> GetByIdIfAccessAsync(long id, UserInfo userInfo)
        {
            var task = await _workTaskRepository.GetNoTrackAsync(id);
            if (task == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            }

            var projectAccessed = await _projectRepository.ExistIfAccessAsync(task.ProjectId, userInfo.UserId);
            if (!projectAccessed.access)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

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
            var projectUserId = await _projectUserService.GetIdByMainAppIdAsync(userInfo, task.ProjectId);
            if (projectUserId == null)
            {
                //по идеи не должно сюда заходить тк выше проверяем доступ GetByIdIfAccessAsync
                throw new SomeCustomException(Consts.ErrorConsts.TaskHaveNoAccess);
            }

            var newComment = new WorkTaskComment() { CreatorId = projectUserId.Value, TaskId = taskId, Text = text };
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
            var task = await _workTaskRepository.GetAsync(id);
            if (task == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            }

            var canAddToProject = await _projectRepository.ExistIfAccessAsync(task.ProjectId, userInfo.UserId);
            if (!canAddToProject.access)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectHaveNoAccess);
            }

            //тк любой человек в проекте должен иметь возможноть поменять название или исполнителя, это убрал
            //if (task.CreatorEntityId != userInfo.UserId && !canAddToProject.isAdmin)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.TaskHaveNoAccess);
            //}

            return task;
        }



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
    }
}
