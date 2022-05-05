﻿using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories
{
    public class TaskReviewRepository : GeneralRepository<TaskReview, long>, ITaskReviewRepository
    {
        public TaskReviewRepository(MenuDbContext db) : base(db)
        {
        }

        public async Task<TaskReview> CreateAsync(TaskReview task)
        {
            return await base.AddAsync(task);
        }

        public async Task<List<TaskReview>> GetTasksAsync(long projectId, long? creatorId
            , long? reviewerId, CodeReviewTaskStatus? status)
        {
            return await _db.ReviewTasks.Where(x => x.ProjectId == projectId
                && (creatorId == null ? true : x.CreatorId == creatorId)
                && (reviewerId == null ? true : x.ReviewerId == reviewerId)
                && (status == null ? true : x.Status == status)).ToListAsync();
        }

    }
}
