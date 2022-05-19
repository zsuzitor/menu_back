
using BO.Models.Auth;
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{
    public class TaskReviewService : ITaskReviewService
    {
        private readonly ITaskReviewRepository _taskReviewRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IProjectUserService _projectUserService;
        private readonly ITaskReviewCommentService _taskReviewCommentService;
        public TaskReviewService(ITaskReviewRepository taskReviewRepository,
            IProjectRepository projectRepository, IProjectUserService projectUserService
            , ITaskReviewCommentService taskReviewCommentService)
        {
            _taskReviewRepository = taskReviewRepository;
            _projectRepository = projectRepository;
            _projectUserService = projectUserService;
            _taskReviewCommentService = taskReviewCommentService;
        }

        public async Task<TaskReview> CreateAsync(TaskReview task)
        {
            return await _taskReviewRepository.CreateAsync(task);
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
            var upTask = await _taskReviewRepository.GetAsync(task.Id);
            if (upTask == null)
            {
                throw new SomeCustomException("task_not_founded");
            }

            var canAddToProject = await _projectRepository.ExistIfAccessAdminAsync(upTask.ProjectId, userInfo.UserId);
            if (!canAddToProject)
            {
                throw new SomeCustomException("project_have_no_access");
            }

            upTask.Status = task.Status;
            upTask.Name = task.Name;
            upTask.CreatorId = task.CreatorId;
            upTask.ReviewerId = task.ReviewerId;
            await _taskReviewRepository.UpdateAsync(upTask);
            return upTask;
        }

        public async Task<TaskReview> DeleteIfAccess(long id, UserInfo userInfo)
        {
            var task = await _taskReviewRepository.GetAsync(id);
            if (task == null)
            {
                throw new SomeCustomException("task_not_found");
            }
            
            var projectAccess =  await _projectRepository.ExistIfAccessAdminAsync(task.ProjectId, userInfo.UserId);
            if (!projectAccess)
            {
                throw new SomeCustomException("project_have_no_access");
            }

            return await _taskReviewRepository.DeleteAsync(task);
        }

        public async Task<List<CommentReview>> GetCommentsAsync(long taskId, UserInfo userInfo)
        {
            var task = await _taskReviewRepository.GetTaskWithCommentsAsync(taskId);
            if (task == null)
            {
                throw new SomeCustomException("task_not_found");
            }

            var projectAccessed = await _projectRepository.ExistIfAccessAsync(task.ProjectId, userInfo.UserId);
            if (!projectAccessed)
            {
                throw new SomeCustomException("project_not_found");
            }

            return task.Comments;
        }

        public async Task<TaskReview> GetByIdIfAccessAsync(long id, UserInfo userInfo)
        {
            var task = await _taskReviewRepository.GetNoTrackAsync(id);
            if (task == null)
            {
                throw new SomeCustomException("task_not_found");
            }

            var projectAccessed = await _projectRepository.ExistIfAccessAsync(task.ProjectId, userInfo.UserId);
            if (!projectAccessed)
            {
                throw new SomeCustomException("project_not_found");
            }

            return task;
        }

        public async Task<CommentReview> CreateCommentAsync(long taskId, string text, UserInfo userInfo)
        {
            _ = await GetByIdIfAccessAsync(taskId, userInfo);
            var projectUser = await _projectUserService.GetByMainAppIdAsync(userInfo);
            //projectUser - уже должен быть свалидирован по GetByIdIfAccessAsync
            var newComment = new CommentReview() { CreatorId = projectUser.Id, TaskId = taskId, Text = text };
            return await _taskReviewCommentService.CreateAsync(newComment);
        }
    }
}
