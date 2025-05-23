
using BO.Models.Auth;
using BO.Models.CodeReviewApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services.Interfaces
{
    public interface ITaskReviewService
    {
        Task<TaskReview> CreateAsync(TaskReview task, UserInfo userInfo);
        Task<List<TaskReview>> GetTasksAsync(long projectId);
        Task<bool> ExistAsync(long projectId, long statusId);
        Task<List<TaskReview>> GetTasksAsync(long projectId, string name, long? creatorId
            , long? reviewerId, long? statusId, int pageNumber, int pageSize);
        Task<long> GetTasksCountAsync(long projectId, string name, long? creatorId
            , long? reviewerId, long? statusId);
        Task<TaskReview> UpdateAsync(TaskReview task, UserInfo userInfo);
        Task<TaskReview> DeleteIfAccess(long id, UserInfo userInfo);
        Task<List<CommentReview>> GetCommentsAsync(long taskId, UserInfo userInfo);
        Task<TaskReview> GetByIdIfAccessAsync(long id, UserInfo userInfo);
        Task<CommentReview> CreateCommentAsync(long taskId, string text, UserInfo userInfo);

    }
}
