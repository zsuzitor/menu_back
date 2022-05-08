
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
        public TaskReviewService(ITaskReviewRepository taskReviewRepository,
            IProjectRepository projectRepository)
        {
            _taskReviewRepository = taskReviewRepository;
            _projectRepository = projectRepository;
        }

        public async Task<TaskReview> CreateAsync(TaskReview task)
        {
            return await _taskReviewRepository.CreateAsync(task);
        }

        public async Task<List<TaskReview>> GetTasksAsync(long projectId)
        {
            return await _taskReviewRepository.GetTasksByProjectIdAsync(projectId);

        }

        public async Task<List<TaskReview>> GetTasksAsync(long projectId, string name, long? creatorId
            , long? reviewerId, CodeReviewTaskStatus? status, int pageNumber, int pageSize)
        {
            return await _taskReviewRepository.GetTasksAsync(projectId, name
                , creatorId, reviewerId, status, pageNumber, pageSize);
        }

        public async Task<TaskReview> UpdateAsync(TaskReview task, UserInfo userInfo)
        {
            var upTask = await _taskReviewRepository.GetAsync(task.Id);
            if (upTask == null)
            {
                throw new SomeCustomException("task_not_founded");
            }

            var canAddToProject = await _projectRepository.ExistIfAccessAsync(upTask.ProjectId, userInfo.UserId);
            if (!canAddToProject)
            {
                throw new SomeCustomException("project_not_found");
            }

            upTask.Status = task.Status;
            upTask.Name = task.Name;
            upTask.CreatorId = task.CreatorId;
            upTask.ReviewerId = task.ReviewerId;
            await _taskReviewRepository.UpdateAsync(upTask);
            return upTask;
        }
    }
}
