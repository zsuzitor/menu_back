
using BO.Models.TaskManagementApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.Models.DTO;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface IWorkTaskRepository : IGeneralRepository<WorkTask, long>
    {
        Task<WorkTask> GetAsync(long id, long projectId);
        Task<WorkTask> GetWithSprintRelationAsync(long id);
        Task<WorkTask> GetWithLabelRelationAsync(long id);
        Task<WorkTask> CreateAsync(WorkTask task);
        Task<List<WorkTask>> GetTasksAsync(GetTasksByFilter filters);
        Task<long> GetTasksCountAsync(GetTasksCountByFilter filters);
        Task<List<WorkTask>> GetTasksByProjectIdAsync(long projectId);
        Task<WorkTask> GetTaskWithCommentsAsync(long id);
        Task<WorkTask> GetTaskFullAsync(long id);
        Task<bool> ExistAsync(long projectId, long statusId);
        Task<bool> HaveAccessAsync(long taskId, long mainAppUserId);
        Task<long> GetUserIdAccessAsync(long taskId, long mainAppUserId);
        Task<WorkTask> GetAccessAsync(long taskId, long mainAppUserId);
        Task<WorkTask> GetAccessRelationsAsync(long taskId, long mainAppUserId);
        Task<TaskRelation> CreateRelationAsync(TaskRelation relation);
        Task<bool> ExistsRelationAsync(long task1Id, long task2Id);
        Task<TaskRelation> DeleteRelationAsync(TaskRelation relation);
        Task<TaskRelation> GetRelationAsync(long relationId);

    }
}
