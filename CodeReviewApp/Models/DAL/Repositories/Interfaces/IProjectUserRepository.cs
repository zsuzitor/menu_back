using BO.Models.CodeReviewApp.DAL.Domain;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories.Interfaces
{
    public interface IProjectUserRepository
    {
        Task<ProjectUser> Create(ProjectUser user);
    }
}
