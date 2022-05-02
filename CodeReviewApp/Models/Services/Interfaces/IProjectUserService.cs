
using BO.Models.CodeReviewApp.DAL.Domain;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services.Interfaces
{
    public interface IProjectUserService
    {
        Task<ProjectUser> CreateAsync(ProjectUser user);
    }
}
