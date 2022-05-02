﻿
using BO.Models.CodeReviewApp.DAL.Domain;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services.Interfaces
{
    public interface ITaskReviewService
    {
        Task<TaskReview> CreateAsync(TaskReview task);
    }
}