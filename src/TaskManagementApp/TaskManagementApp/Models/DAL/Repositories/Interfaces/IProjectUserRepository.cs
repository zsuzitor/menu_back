using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface IProjectUserRepository : IGeneralRepository<ProjectUser, long>
    {
        Task<ProjectUser> CreateAsync(ProjectUser user);
        Task<List<ProjectUser>> GetProjectUsersAsync(long projectId);
        Task<bool> ExistAsync(long projectId, long userId);
        Task<bool> ExistByMainIdAsync(long projectId, long mainAppUserId);

        Task<List<ProjectUser>> GetProjectUserAsync(long projectId, List<long> usersId);


        Task<ProjectUser> GetByMainAppUserIdAsync(long mainAppUserId, long projectId);

        /// <summary>
        /// только активные пользователи
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<long?> GetIdByMainAppIdAsync(long userId, long projectId);
        Task<string> GetNotificationEmailAsync(long userId);
        Task<(string email, long? mainAppId)> GetNotificationEmailWithMainAppIdAsync(long userId);

    }
}
