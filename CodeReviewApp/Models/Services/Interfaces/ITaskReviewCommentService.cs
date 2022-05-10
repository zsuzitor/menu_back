
using BO.Models.Auth;
using BO.Models.CodeReviewApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services.Interfaces
{
    public interface ITaskReviewCommentService
    {
        /// <summary>
        /// без валидаций
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<CommentReview> CreateAsync(CommentReview comment);
        Task<CommentReview> EditAsync(long commentId, string text, UserInfo userInfo);
        Task<CommentReview> DeleteAsync(long commentId, UserInfo userInfo);

    }
}
