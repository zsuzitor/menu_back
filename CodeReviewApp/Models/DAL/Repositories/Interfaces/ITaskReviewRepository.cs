﻿
using BO.Models.CodeReviewApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories.Interfaces
{
    public interface ITaskReviewRepository : IGeneralRepository<TaskReview, long>
    {
        Task<TaskReview> CreateAsync(TaskReview task);
        Task<List<TaskReview>> GetTasksAsync(long projectId, string name, long? creatorId
            , long? reviewerId, CodeReviewTaskStatus? status, int pageNumber, int pageSize);

        Task<List<TaskReview>> GetTasksByProjectIdAsync(long projectId);
        
    }
}
