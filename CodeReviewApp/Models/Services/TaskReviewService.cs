
using BO.Models.Auth;
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{
    public sealed class TaskReviewService : ITaskReviewService
    {
        private readonly ITaskReviewRepository _taskReviewRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserService _projectUserService;
        private readonly ITaskReviewCommentService _taskReviewCommentService;
        private readonly IReviewAppEmailService _reviewAppEmailService;
        public TaskReviewService(ITaskReviewRepository taskReviewRepository,
            IProjectRepository projectRepository, IProjectUserService projectUserService
            , ITaskReviewCommentService taskReviewCommentService, IReviewAppEmailService reviewAppEmailService)
        {
            _taskReviewRepository = taskReviewRepository;
            _projectRepository = projectRepository;
            _projectUserService = projectUserService;
            _taskReviewCommentService = taskReviewCommentService;
            _reviewAppEmailService = reviewAppEmailService;
        }

        public async Task<TaskReview> CreateAsync(TaskReview task, UserInfo userInfo)
        {
            var addedTask = await _taskReviewRepository.CreateAsync(task);
            if (addedTask.ReviewerId != null)
            {
                var us = await _projectUserService.GetNotificationEmailWithMainAppIdAsync(task.ReviewerId.Value);
                if (!string.IsNullOrWhiteSpace(us.email) && us.mainAppId != userInfo.UserId)
                {
                    await _reviewAppEmailService.QueueReviewerInReviewTaskAsync(us.email, task.Name);
                }
            }

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
            , long? reviewerId, CodeReviewTaskStatus? status, int pageNumber, int pageSize)
        {
            return await _taskReviewRepository.GetTasksAsync(projectId, name
                , creatorId, reviewerId, status, pageNumber, pageSize);
        }

        public async Task<long> GetTasksCountAsync(long projectId, string name, long? creatorId
            , long? reviewerId, CodeReviewTaskStatus? status)
        {
            return await _taskReviewRepository.GetTasksCountAsync(projectId, name
                , creatorId, reviewerId, status);
        }

        public async Task<TaskReview> UpdateAsync(TaskReview task, UserInfo userInfo)
        {
            if (string.IsNullOrWhiteSpace(task.Name))
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.EmptyTaskName);
            }

            var upTask = await _taskReviewRepository.GetAsync(task.Id);
            if (upTask == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskNotFound);
            }

            var canAddToProject = await _projectRepository.ExistIfAccessAsync(upTask.ProjectId, userInfo.UserId);
            if (!canAddToProject.access)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectHaveNoAccess);
            }

            if (upTask.CreatorEntityId != userInfo.UserId && !canAddToProject.isAdmin)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskHaveNoAccess);
            }

            if (task.ReviewerId != null)
            {
                var reviewerExist = await _projectUserService.ExistAsync(upTask.ProjectId, task.ReviewerId.Value);
                if (!reviewerExist)
                {
                    throw new SomeCustomException(Consts.CodeReviewErrorConsts.UserNotFound);
                }
            }

            bool needNotifyReviewer = false;
            if (upTask.ReviewerId != task.ReviewerId && task.ReviewerId != null)
            {
                needNotifyReviewer = true;
            }

            bool statusWasChanged = upTask.Status != task.Status;

            upTask.Status = task.Status;
            upTask.Name = task.Name;
            upTask.Link = task.Link;
            upTask.CreatorId = task.CreatorId;
            upTask.ReviewerId = task.ReviewerId;
            await _taskReviewRepository.UpdateAsync(upTask);

            string reviewerEmailNotification = null;
            long? reviewerMainAppUserId = null;
            //if (needNotifyReviewer || statusWasChanged)
            //{

            //}

            if (needNotifyReviewer)
            {
                var us = await _projectUserService.GetNotificationEmailWithMainAppIdAsync(upTask.ReviewerId.Value);
                reviewerEmailNotification = us.email;
                reviewerMainAppUserId = us.mainAppId;
                if (!string.IsNullOrWhiteSpace(reviewerEmailNotification) && reviewerMainAppUserId != userInfo.UserId)
                {
                    await _reviewAppEmailService.QueueReviewerInReviewTaskAsync(reviewerEmailNotification, task.Name);
                }
            }

            if (statusWasChanged)
            {
                if (upTask.ReviewerId != null && string.IsNullOrWhiteSpace(reviewerEmailNotification))
                {
                    var us = await _projectUserService.GetNotificationEmailWithMainAppIdAsync(upTask.ReviewerId.Value);
                    reviewerEmailNotification = us.email;
                    reviewerMainAppUserId = us.mainAppId;
                }

                if (!string.IsNullOrWhiteSpace(reviewerEmailNotification) && reviewerMainAppUserId != userInfo.UserId)
                {
                    await _reviewAppEmailService.QueueChangeStatusTaskAsync(reviewerEmailNotification, task.Name, upTask.Status.ToString());
                }

                var usc = await _projectUserService.GetNotificationEmailWithMainAppIdAsync(upTask.CreatorId);
                if (!string.IsNullOrWhiteSpace(usc.email) && usc.mainAppId != userInfo.UserId)
                {
                    await _reviewAppEmailService.QueueChangeStatusTaskAsync(usc.email, task.Name, upTask.Status.ToString());
                }
            }

            return upTask;
        }

        public async Task<TaskReview> DeleteIfAccess(long id, UserInfo userInfo)
        {
            var task = await _taskReviewRepository.GetAsync(id);
            if (task == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskNotFound);
            }

            var canAddToProject = await _projectRepository.ExistIfAccessAsync(task.ProjectId, userInfo.UserId);
            if (!canAddToProject.access)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectHaveNoAccess);
            }

            if (task.CreatorEntityId != userInfo.UserId && !canAddToProject.isAdmin)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskHaveNoAccess);
            }

            return await _taskReviewRepository.DeleteAsync(task);
        }

        public async Task<List<CommentReview>> GetCommentsAsync(long taskId, UserInfo userInfo)
        {
            var task = await _taskReviewRepository.GetTaskWithCommentsAsync(taskId);
            if (task == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskNotFound);
            }

            var projectAccessed = await _projectRepository.ExistIfAccessAsync(task.ProjectId, userInfo.UserId);
            if (!projectAccessed.access)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return task.Comments;
        }

        public async Task<TaskReview> GetByIdIfAccessAsync(long id, UserInfo userInfo)
        {
            var task = await _taskReviewRepository.GetNoTrackAsync(id);
            if (task == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskNotFound);
            }

            var projectAccessed = await _projectRepository.ExistIfAccessAsync(task.ProjectId, userInfo.UserId);
            if (!projectAccessed.access)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return task;
        }

        public async Task<CommentReview> CreateCommentAsync(long taskId, string text, UserInfo userInfo)
        {
            //todo много запросов что то получается
            var task = await GetByIdIfAccessAsync(taskId, userInfo);
            var projectUserId = await _projectUserService.GetIdByMainAppIdAsync(userInfo, task.ProjectId);
            //projectUser - уже должен быть свалидирован по GetByIdIfAccessAsync
            var newComment = new CommentReview() { CreatorId = projectUserId.Value, TaskId = taskId, Text = text };
            var comment = await _taskReviewCommentService.CreateAsync(newComment);
            var emailForNotification = new List<string>();
            if (task.ReviewerId != projectUserId && task.ReviewerId != null)
            {
                emailForNotification.Add(await _projectUserService.GetNotificationEmailAsync(task.ReviewerId.Value));
            }

            if (task.CreatorId != projectUserId)
            {
                emailForNotification.Add(await _projectUserService.GetNotificationEmailAsync(task.CreatorId));

            }

            await _reviewAppEmailService.QueueNewCommentInReviewTaskAsync(emailForNotification, task.Name);
            return comment;
        }
    }
}
