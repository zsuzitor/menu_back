
using BO.Models.CodeReviewApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories.Interfaces
{
    public interface ITaskReviewCommentRepository : IGeneralRepository<CommentReview, long>
    {
        Task<CommentReview> DeleteAsync(long id, long userId);
        Task<CommentReview> UpdateAsync(long id, long userId, string text);
    }
}
