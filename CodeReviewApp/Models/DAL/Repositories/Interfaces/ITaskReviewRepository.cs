
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface ITaskReviewRepository : IGeneralRepository<TaskReview, long>
    {
        Task<TaskReview> GetAsync(long id, long projectId);
        Task<TaskReview> CreateAsync(TaskReview task);
        Task<List<TaskReview>> GetTasksAsync(long projectId, string name, long? creatorId
            , long? reviewerId, long? statusId, int pageNumber, int pageSize);
        Task<long> GetTasksCountAsync(long projectId, string name, long? creatorId
            , long? reviewerId, long? statusId);
        Task<List<TaskReview>> GetTasksByProjectIdAsync(long projectId);
        Task<TaskReview> GetTaskWithCommentsAsync(long id);
        Task<bool> ExistAsync(long projectId, long statusId);

    }
}
