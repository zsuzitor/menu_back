
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
    public sealed class TaskReviewService : ITaskReviewService
    {
        private readonly ITaskReviewRepository _taskReviewRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserService _projectUserService;
        private readonly ITaskReviewCommentService _taskReviewCommentService;
        private readonly IReviewAppEmailService _reviewAppEmailService;
        private readonly ITaskStatusRepository _taskStatusRepository;
        private readonly IConfiguration _configuration;
        public TaskReviewService(ITaskReviewRepository taskReviewRepository,
            IProjectRepository projectRepository, IProjectUserService projectUserService
            , ITaskReviewCommentService taskReviewCommentService, IReviewAppEmailService reviewAppEmailService, ITaskStatusRepository taskStatusRepository
            , IConfiguration configuration)
        {
            _taskReviewRepository = taskReviewRepository;
            _projectRepository = projectRepository;
            _projectUserService = projectUserService;
            _taskReviewCommentService = taskReviewCommentService;
            _reviewAppEmailService = reviewAppEmailService;
            _taskStatusRepository = taskStatusRepository;
            _configuration = configuration;
        }

        public async Task<TaskReview> CreateAsync(TaskReview task, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new SomeCustomException(Consts.ErrorConsts.EmptyTaskName);
            }

            if (task.StatusId == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskReviewStatusNotExists);
            }


            var addedTask = await _taskReviewRepository.CreateAsync(task);
            await TaskChangedNotifyAsync(null, addedTask, userInfo);

            return addedTask;
        }

        public async Task<List<TaskReview>> GetTasksAsync(long projectId)
        {
            return await _taskReviewRepository.GetTasksByProjectIdAsync(projectId);

        }

        /// <summary>
        /// без валидации
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="name"></param>
        /// <param name="creatorId"></param>
        /// <param name="reviewerId"></param>
        /// <param name="status"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<List<TaskReview>> GetTasksAsync(long projectId, string name, long? creatorId
            , long? reviewerId, long? statusId, int pageNumber, int pageSize)
        {
            return await _taskReviewRepository.GetTasksAsync(projectId, name
                , creatorId, reviewerId, statusId, pageNumber, pageSize);
        }

        public async Task<long> GetTasksCountAsync(long projectId, string name, long? creatorId
            , long? reviewerId, long? statusId)
        {
            return await _taskReviewRepository.GetTasksCountAsync(projectId, name
                , creatorId, reviewerId, statusId);
        }

        public async Task<TaskReview> UpdateAsync(TaskReview task, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new SomeCustomException(Consts.ErrorConsts.EmptyTaskName);
            }

            if (task.StatusId == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskReviewStatusNotExists);
            }


            var upTask = await GetIfEditAccess(task.Id, userInfo);
            var prevTask = upTask.CopyPlaneProp();

            if (task.StatusId != upTask.StatusId)
            {
                var status = task.StatusId == null ? null : await _taskStatusRepository.GetAsync(task.StatusId.Value);
                if (status == null || status.ProjectId != upTask.ProjectId)
                {
                    throw new SomeCustomException(Consts.ErrorConsts.TaskReviewStatusNotExists);
                }
            }


            if (task.ReviewerId != null)
            {
                var reviewerExist = await _projectUserService.ExistAsync(upTask.ProjectId, task.ReviewerId.Value);
                if (!reviewerExist)
                {
                    throw new SomeCustomException(Consts.ErrorConsts.UserNotFound);
                }
            }


            upTask.Status = task.Status;
            upTask.Name = task.Name;
            upTask.Description = task.Description;
            upTask.LastUpdateDate = DateTime.Now;
            //upTask.CreatorId = task.CreatorId;
            upTask.ReviewerId = task.ReviewerId;
            await _taskReviewRepository.UpdateAsync(upTask);

            await TaskChangedNotifyAsync(prevTask, upTask, userInfo);

            return upTask;
        }



        public async Task<TaskReview> UpdateNameAsync(long id, string name, UserInfo userInfo)
        {

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new SomeCustomException(Consts.ErrorConsts.EmptyTaskName);
            }


            var upTask = await GetIfEditAccess(id, userInfo);
            var oldTask = upTask.CopyPlaneProp();
            upTask.Name = name;
            await _taskReviewRepository.UpdateAsync(upTask);
            await TaskChangedNotifyAsync(oldTask, upTask, userInfo);
            return upTask;

        }

        public async Task<TaskReview> UpdateDescriptionAsync(long id, string description, UserInfo userInfo)
        {

            var upTask = await GetIfEditAccess(id, userInfo);
            var oldTask = upTask.CopyPlaneProp();
            upTask.Description = description;
            await _taskReviewRepository.UpdateAsync(upTask);
            await TaskChangedNotifyAsync(oldTask, upTask, userInfo);
            return upTask;
        }

        public async Task<TaskReview> UpdateStatusAsync(long id, long statusId, UserInfo userInfo)
        {

            var upTask = await GetIfEditAccess(id, userInfo);

            if (statusId != upTask.StatusId)
            {
                var status = await _taskStatusRepository.GetAsync(statusId);
                if (status == null || status.ProjectId != upTask.ProjectId)
                {
                    throw new SomeCustomException(Consts.ErrorConsts.TaskReviewStatusNotExists);
                }
                upTask.StatusId = statusId;
                await _taskReviewRepository.UpdateAsync(upTask);

                await StatusChangedNotifyAsync(upTask, userInfo);

                return upTask;
            }

            return null;
        }

        public async Task<TaskReview> UpdateExecutorAsync(long id, long executorId, UserInfo userInfo)
        {
            var upTask = await GetIfEditAccess(id, userInfo);
            var oldTask = upTask.CopyPlaneProp();

            var reviewerExist = await _projectUserService.ExistAsync(upTask.ProjectId, executorId);
            if (!reviewerExist)
            {
                throw new SomeCustomException(Consts.ErrorConsts.UserNotFound);
            }

            upTask.ReviewerId = executorId;
            await _taskReviewRepository.UpdateAsync(upTask);
            await TaskChangedNotifyAsync(oldTask, upTask, userInfo);
            return upTask;
        }

        public async Task<TaskReview> DeleteIfAccess(long id, UserInfo userInfo)
        {
            var task = await GetByIdIfAccessAsync(id, userInfo);
            return await _taskReviewRepository.DeleteAsync(task);
        }

        public async Task<List<CommentReview>> GetCommentsAsync(long taskId, UserInfo userInfo)
        {
            var task = await _taskReviewRepository.GetTaskWithCommentsAsync(taskId);
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

        public async Task<TaskReview> GetByIdIfAccessAsync(long id, UserInfo userInfo)
        {
            var task = await _taskReviewRepository.GetNoTrackAsync(id);
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

        public async Task<CommentReview> CreateCommentAsync(long taskId, string text, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskReviewEmptyStatusName);
            }

            //todo много запросов что то получается
            var task = await GetByIdIfAccessAsync(taskId, userInfo);
            var projectUserId = await _projectUserService.GetIdByMainAppIdAsync(userInfo, task.ProjectId);
            if (projectUserId == null)
            {
                //по идеи не должно сюда заходить тк выше проверяем доступ GetByIdIfAccessAsync
                throw new SomeCustomException(Consts.ErrorConsts.TaskHaveNoAccess);
            }

            var newComment = new CommentReview() { CreatorId = projectUserId.Value, TaskId = taskId, Text = text };
            var comment = await _taskReviewCommentService.CreateAsync(newComment);
            var emails = await GetTaskUsersForNotificationAsync(task, userInfo);

            await _reviewAppEmailService.QueueNewCommentInTaskAsync(emails, task.Name, GetTaskUrl(task));
            return comment;
        }

        public async Task<bool> ExistAsync(long projectId, long statusId)
        {
            return await _taskReviewRepository.ExistAsync(projectId, statusId);
        }

        public async Task<TaskReview> GetTaskAsync(long id)
        {
            return await _taskReviewRepository.GetNoTrackAsync(id);
        }

        public async Task<TaskReview> GetTaskWithCommentsAsync(long id)
        {
            return await _taskReviewRepository.GetTaskWithCommentsAsync(id);
        }

        public async Task<TaskReview> GetIfEditAccess(long id, UserInfo userInfo)
        {
            var task = await _taskReviewRepository.GetAsync(id);
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



        private async Task StatusChangedNotifyAsync(TaskReview upTask, UserInfo userInfo)
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
            await _reviewAppEmailService.QueueChangeStatusTaskAsync(users, upTask.Name, status.Name, taskUrl);

        }

        private async Task TaskChangedNotifyAsync(TaskReview prevTask, TaskReview upTask, UserInfo userInfo)
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

            if (prevTask?.ReviewerId != upTask.ReviewerId)
            {
                var prevReviewer = prevTask?.Reviewer;
                var newReviewer = upTask.Reviewer;
                var reviewforLoad = new List<long>();
                if (prevReviewer == null && prevTask?.ReviewerId != null)
                {
                    reviewforLoad.Add(prevTask.ReviewerId.Value);
                }

                if (newReviewer == null && upTask.ReviewerId.HasValue)
                {
                    reviewforLoad.Add(upTask.ReviewerId.Value);
                }

                var reviewers = await _projectUserService.GetProjectUserAsync(upTask.ProjectId, reviewforLoad);

                prevReviewer = prevReviewer ?? reviewers.FirstOrDefault(x => x.Id == prevTask?.ReviewerId);
                newReviewer = newReviewer ?? reviewers.FirstOrDefault(x => x.Id == upTask.ReviewerId);
                changes.Add(new EmailServiceBase.Changes()
                {
                    PropName = "Исполнитель",
                    PropPrevValue = prevReviewer?.NotifyEmail,//todo тут не уверен что эти почты надо брать
                    PropNewValue = newReviewer?.NotifyEmail
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
            await _reviewAppEmailService.QueueChangeTaskAsync(users, upTask.Name, changes, taskUrl);

        }


        private async Task<List<string>> GetTaskUsersForNotificationAsync(TaskReview upTask, UserInfo userInfo)
        {
            //todo я не понимаю почему у юзера в код ревью может быть пустым us.mainAppId, это ошибка?
            var emails = new List<(long? id, string email)>();
            if (upTask == null)
            {
                return emails.Select(x => x.email).ToList();
            }

            if (upTask.ReviewerId.HasValue)
            {
                var us = await _projectUserService.GetNotificationEmailWithMainAppIdAsync(upTask.ReviewerId.Value);
                emails.Add((us.mainAppId, us.email));
            }

            if (upTask.CreatorId != upTask.ReviewerId)
            {
                var us = await _projectUserService.GetNotificationEmailWithMainAppIdAsync(upTask.CreatorId);
                emails.Add((us.mainAppId, us.email));
            }


            //если юзер редачит таску то ему отправлять не надо
            return emails.Where(x => x.id != userInfo.UserId && !string.IsNullOrEmpty(x.email))
                .Select(x => x.email).Distinct().ToList();
        }



        private string GetTaskUrl(TaskReview task)
        {
            if (task == null)
            {
                return "";
            }

            var baseUrl = _configuration["SiteSettings:BaseUrl"];
            var endpoint = $"code-review/proj-{task.ProjectId}/task-{task.Id}";
            var fullUri = new Uri(new Uri(baseUrl), endpoint).ToString();
            return fullUri;
        }
    }
}
