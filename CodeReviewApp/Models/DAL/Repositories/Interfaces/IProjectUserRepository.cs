using BO.Models.CodeReviewApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories.Interfaces
{
    public interface IProjectUserRepository : IGeneralRepository<ProjectUser, long>
    {
        Task<ProjectUser> CreateAsync(ProjectUser user);
        Task<List<ProjectUser>> GetProjectUsersAsync(long projectId);
        Task<bool> ExistAsync(long projectId, long userId);
        Task<bool> ExistByMainIdAsync(long projectId, long mainAppUserId);

        
        Task<ProjectUser> GetByMainAppUserIdAsync(long mainAppUserId, long projectId);

        /// <summary>
        /// только активные пользователи
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<long?> GetIdByMainAppIdAsync(long userId, long projectId);
        Task<string> GetNotificationEmailAsync(long userId);

    }
}
