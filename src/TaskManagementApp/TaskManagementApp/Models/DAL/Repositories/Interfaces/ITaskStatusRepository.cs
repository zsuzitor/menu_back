using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface ITaskStatusCachedRepository : ITaskStatusRepository;
    public interface ITaskStatusRepository : IGeneralRepository<WorkTaskStatus, long>
    {
        Task<List<WorkTaskStatus>> GetForProjectAsync(long projectId);
    }
}
