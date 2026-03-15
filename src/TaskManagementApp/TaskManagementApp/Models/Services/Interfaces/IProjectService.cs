using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface IProjectService
    {
        Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId);
        Task<Project> GetAsync(long id);
        Task<Project> GetByIdIfAccessAsync(long id, long userId);
        Task<Project> GetByIdIfAccessAdminAsync(long id, long userId);

        Task<(bool access, bool isAdmin)> ExistIfAccessAsync(long id, long userId);
        Task<bool> ExistIfAccessAdminAsync(long id, long userId);

        Task<Project> CreateAsync(string name, long userId);
        Task<ProjectUser> CreateUserAsync(long projectId, string userName, string email, long? mainAppUserId, long userId);
        Task<WorkTask> CreateTaskAsync(WorkTask task, long userId);
        Task<bool> DeleteAsync(long projectId, long userId);
        Task AlertAsync();






    }
}
