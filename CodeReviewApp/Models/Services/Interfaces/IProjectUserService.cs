
using BO.Models.Auth;
using BO.Models.CodeReviewApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services.Interfaces
{
    public interface IProjectUserService
    {
        Task<ProjectUser> CreateAsync(ProjectUser user);

        /// <summary>
        /// не смотрит уровень доступа
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<List<ProjectUser>> GetProjectUsersAccessAsync(long projectId, UserInfo userInfo);
        Task<List<ProjectUser>> GetProjectUsersAsync(long projectId, UserInfo userInfo);
        Task<bool> ExistAsync(long projectId, long userId);


        Task<ProjectUser> ChangeAsync(long userId, string name, string email, bool isAdmin, bool deactivated, UserInfo userInfo);
        Task<ProjectUser> DeleteAsync(long userId, UserInfo userInfo);
        Task<ProjectUser> GetByMainAppIdAsync(UserInfo userInfo, long projectId);
        Task<long?> GetIdByMainAppIdAsync(UserInfo userInfo, long projectId);
        Task<string> GetNotificationEmailAsync(long userId);
        Task<(string email, long? mainAppId)> GetNotificationEmailWithMainAppIdAsync(long userId);

    }
}
