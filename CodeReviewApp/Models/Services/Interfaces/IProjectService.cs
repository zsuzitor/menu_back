﻿using BO.Models.Auth;
using BO.Models.CodeReviewApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId);
        Task<Project> GetAsync(long id);
        Task<Project> GetByIdIfAccessAsync(long id, UserInfo userInfo);
        Task<Project> GetByIdIfAccessAdminAsync(long id, UserInfo userInfo);

        Task<(bool access, bool isAdmin)> ExistIfAccessAsync(long id, UserInfo userInfo);
        Task<bool> ExistIfAccessAdminAsync(long id, UserInfo userInfo);

        Task<Project> CreateAsync(string name, UserInfo userInfo);
        Task<ProjectUser> CreateUserAsync(long projectId, string userName, string email, long? mainAppUserId, UserInfo userInfo);
        Task<TaskReview> CreateTaskAsync(TaskReview task, UserInfo userInfo);
        Task<bool> DeleteAsync(long projectId, UserInfo userInfo);
        Task AlertAsync();

    }
}
