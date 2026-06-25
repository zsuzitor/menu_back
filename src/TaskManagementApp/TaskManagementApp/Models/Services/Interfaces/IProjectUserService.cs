
using BO.Models.TaskManagementApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface IProjectUserService
    {
        Task<ProjectUser> CreateAsync(ProjectUser user);

        Task<List<ProjectUser>> GetProjectUsersAsync(long projectId);
        /// <summary>
        /// не смотрит уровень доступа
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="usersId"></param>
        /// <returns></returns>
        //Task<List<ProjectUser>> GetProjectUserByMainAppUserIdAsync(long projectId, List<long> usersId);
        Task<bool> ExistByMainAppUserIdAsync(long projectId, long mainAppUserId);


        Task<ProjectUser> ChangeAsync(long userIdForChange, long projectId, bool isAdmin, bool deactivated, long userId);
        Task<ProjectUser> DeleteAsync(long userIdForDel, long projectId, long userId);
        Task<string> GetNotificationEmailAsync(long userId);

    }
}
