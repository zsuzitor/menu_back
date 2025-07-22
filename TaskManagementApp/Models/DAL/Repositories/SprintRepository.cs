using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace TaskManagementApp.Models.DAL.Repositories
{
    public class SprintRepository : GeneralRepository<ProjectSprint, long>, ISprintRepository
    {
        public SprintRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }


    }
}
