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
        Task<ProjectUser> GetByMainAppUserIdAsync(long projectId, long mainAppUserId);
        Task<ProjectUser> GetByMainAppIdAsync(long userId);
    }
}
