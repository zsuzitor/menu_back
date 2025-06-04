
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface IWorkTaskRepository : IGeneralRepository<WorkTask, long>
    {
        Task<WorkTask> GetAsync(long id, long projectId);
        Task<WorkTask> CreateAsync(WorkTask task);
        Task<List<WorkTask>> GetTasksAsync(long projectId, string name, long? creatorId
            , long? executorId, long? statusId, int pageNumber, int pageSize);
        Task<long> GetTasksCountAsync(long projectId, string name, long? creatorId
            , long? executorId, long? statusId);
        Task<List<WorkTask>> GetTasksByProjectIdAsync(long projectId);
        Task<WorkTask> GetTaskWithCommentsAsync(long id);
        Task<bool> ExistAsync(long projectId, long statusId);
        Task<bool> HaveAccessAsync(long taskId, long mainAppUserId);
        Task<long> GetUserIdAccessAsync(long taskId, long mainAppUserId);
        Task<WorkTask> GetAccessAsync(long taskId, long mainAppUserId);

    }
}
