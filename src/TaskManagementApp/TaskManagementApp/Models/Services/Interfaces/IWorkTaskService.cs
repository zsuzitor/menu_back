
using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.Models.DTO;

namespace TaskManagementApp.Models.Services.Interfaces
{
    public interface IWorkTaskService
    {
        Task<WorkTask> CreateAsync(WorkTask task, UserInfo userInfo);
        Task<List<WorkTask>> GetTasksAsync(long projectId);
        Task<WorkTask> GetTaskAsync(long id);
        Task<WorkTask> GetTaskWithCommentsAsync(long id);
        Task<WorkTask> GetTaskFullAsync(long id);
        Task<WorkTask> CopyAsync(long id, UserInfo userInfo);
        Task<TaskRelation> CreateRelationAsync(TaskRelation req, UserInfo userInfo);
        Task<TaskRelation> DeleteRelationAsync(long relationId, UserInfo userInfo);
        Task<bool> ExistAsync(long projectId, long statusId);
        Task<List<WorkTask>> GetTasksAsync(GetTasksByFilter filters);
        Task<long> GetTasksCountAsync(GetTasksCountByFilter filters);
        Task<WorkTask> UpdateAsync(WorkTask task, UserInfo userInfo);
        Task<WorkTask> UpdateNameAsync(long id, string name, UserInfo userInfo);
        Task<WorkTask> UpdateDescriptionAsync(long id, string description, UserInfo userInfo);
        Task<WorkTask> UpdateStatusAsync(long id, long statusId, UserInfo userInfo);
        Task<WorkTask> UpdateExecutorAsync(long id, long executorId, UserInfo userInfo);
        Task<WorkTask> DeleteIfAccess(long id, UserInfo userInfo);
        Task<WorkTask> GetIfEditAccess(long id, UserInfo userInfo);
        Task<List<WorkTaskComment>> GetCommentsAsync(long taskId, UserInfo userInfo);
        Task<WorkTask> GetByIdIfAccessAsync(long id, UserInfo userInfo);
        Task<WorkTaskComment> CreateCommentAsync(long taskId, string text, UserInfo userInfo);

    }
}
