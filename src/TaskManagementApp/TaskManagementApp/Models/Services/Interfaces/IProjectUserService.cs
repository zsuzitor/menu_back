
using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface IProjectUserService
    {
        Task<ProjectUser> CreateAsync(ProjectUser user);

        /// <summary>
        /// не смотрит уровень доступа
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<List<ProjectUser>> GetProjectUsersAccessAsync(long projectId, long userId);
        Task<List<ProjectUser>> GetProjectUsersAsync(long projectId, long userId);
        /// <summary>
        /// не смотрит уровень доступа
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="usersId"></param>
        /// <returns></returns>
        Task<List<ProjectUser>> GetProjectUserAsync(long projectId, List<long> usersId);
        Task<bool> ExistAsync(long projectId, long userId);


        Task<ProjectUser> ChangeAsync(long userIdForChange, string name, string email, bool isAdmin, bool deactivated, long userId);
        Task<ProjectUser> DeleteAsync(long userIdForDel, long userId);
        Task<ProjectUser> GetByMainAppIdAsync(long userId, long projectId);
        Task<ProjectUser> GetAdminByMainAppIdAsync(long userId, long projectId);
        Task<long?> GetIdByMainAppIdAsync(long userId, long projectId);
        Task<string> GetNotificationEmailAsync(long userId);
        Task<(string email, long? mainAppId)> GetNotificationEmailWithMainAppIdAsync(long userId);

    }
}
