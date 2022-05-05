using BO.Models.CodeReviewApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories.Interfaces
{
    public interface IProjectUserRepository
    {
        Task<ProjectUser> CreateAsync(ProjectUser user);
        Task<List<ProjectUser>> GetProjectUsersAsync(long projectId);
    }
}
