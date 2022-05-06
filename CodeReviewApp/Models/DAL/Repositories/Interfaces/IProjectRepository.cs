
using BO.Models.CodeReviewApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories.Interfaces
{
    public interface IProjectRepository : IGeneralRepository<Project, long>
    {
        Task<Project> CreateAsync(string name, ProjectUser user);
        Task<List<Project>> GetProjectsByMainAppUserIdAsync(long userId);
        Task<Project> GetAsync(long id);
        Task<Project> GetByIdIfAccessAsync(long id, long mainAppUserId);
        Task<bool> ExistIfAccessAsync(long id, long mainAppUserId);
        Task<Project> DeleteAsync(Project project);

    }
}
