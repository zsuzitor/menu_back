
using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.Models.DTO;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface IWorkTaskService
    {
        Task<WorkTask> CreateAsync(WorkTask task, long userId);
        Task<List<WorkTask>> GetTasksAsync(long projectId);
        Task<WorkTask> GetTaskAsync(long id);
        Task<GetProjectTaskSelectInfo> GetProjectTaskSelectInfoAsync(long id, long userId);
        Task<WorkTask> GetTaskWithCommentsAsync(long id);
        Task<WorkTask> GetTaskFullAsync(long id);
        Task<WorkTask> CopyAsync(long id, long userId);
        Task<TaskRelation> CreateRelationAsync(TaskRelation req, long userId);
        Task<List<TaskRelation>> GetRelationsAsync(long taskId, long userId);
        Task<TaskRelation> DeleteRelationAsync(long relationId, long userId);
        Task<bool> ExistAsync(long projectId, long statusId);
        Task<List<WorkTask>> GetTasksAsync(GetTasksByFilter filters);
        Task<long> GetTasksCountAsync(GetTasksCountByFilter filters);
        Task<WorkTask> UpdateAsync(WorkTask task, long userId);
        Task<WorkTask> UpdateNameAsync(long id, string name, long userId);
        Task<WorkTask> UpdateDescriptionAsync(long id, string description, long userId);
        Task<WorkTask> UpdateStatusAsync(long id, long statusId, long userId);
        Task<WorkTask> UpdateExecutorAsync(long id, long executorId, long userId);
        Task<WorkTask> DeleteIfAccess(long id, long userId);
        Task<WorkTask> GetIfEditAccess(long id, long userId);
        Task<List<WorkTaskComment>> GetCommentsAsync(long taskId, long userId);
        Task<WorkTask> GetByIdIfAccessAsync(long id, long userId);
        Task<WorkTaskComment> CreateCommentAsync(long taskId, string text, long userId);

    }
}
