using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface IProjectUserCahcedRepository : IProjectUserRepository;
    public interface IProjectUserRepository : IGeneralRepository<ProjectUser, long>
    {
        Task<List<ProjectUser>> GetProjectUsersWithMainAppUserAsync(long projectId);
        Task<bool> ExistByMainIdAsync(long projectId, long mainAppUserId);

        //Task<List<ProjectUser>> GetProjectUserAsync(long projectId, List<long> usersId);


        Task<ProjectUser> GetByMainAppUserIdAsync(long mainAppUserId, long projectId);



    }
}
