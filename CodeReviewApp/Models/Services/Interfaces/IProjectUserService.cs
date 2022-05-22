
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
        Task<List<ProjectUser>> GetProjectUsersAsync(long projectId);

        Task<ProjectUser> ChangeAsync(long userId, string name, string email, bool isAdmin, UserInfo userInfo);
        Task<ProjectUser> DeleteAsync(long userId, UserInfo userInfo);
        Task<ProjectUser> GetByMainAppIdAsync(UserInfo userInfo);
        Task<long?> GetIdByMainAppIdAsync(UserInfo userInfo);
        Task<string> GetNotificationEmailAsync(long userId);

    }
}
