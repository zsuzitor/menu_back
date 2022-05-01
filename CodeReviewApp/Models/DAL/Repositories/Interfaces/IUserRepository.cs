using BO.Models.CodeReviewApp.DAL.Domain;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<ProjectUser> Create(ProjectUser user);
    }
}
