using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services.Interfaces;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{
    public class ProjectUserService : IProjectUserService
    {
        private readonly IProjectUserRepository _projectUserRepository;
        public ProjectUserService(IProjectUserRepository projectUserRepository)
        {
            _projectUserRepository = projectUserRepository;
        }

        public async Task<ProjectUser> CreateAsync(ProjectUser user)
        {
            return await _projectUserRepository.Create(user);
        }
    }
}
