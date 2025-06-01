
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface IProjectRepository : IGeneralRepository<Project, long>
    {
        Task<Project> CreateAsync(string name, ProjectUser user);
        Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId);
        Task<Project> GetByIdIfAccessAsync(long id, long mainAppUserId);
        Task<Project> GetByIdIfAccessAdminAsync(long id, long mainAppUserId);
        Task<(bool access, bool isAdmin)> ExistIfAccessAsync(long id, long mainAppUserId);
        Task<bool> ExistIfAccessAdminAsync(long id, long mainAppUserId);
    }
}
