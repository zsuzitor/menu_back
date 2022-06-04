
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.DAL.Repositories
{
    public sealed class TaskReviewCommentRepository : GeneralRepository<CommentReview, long>, ITaskReviewCommentRepository
    {
        public TaskReviewCommentRepository(MenuDbContext db) : base(db)
        {
        }

        public async Task<CommentReview> DeleteAsync(long id, long userId)
        {
            var comment = _db.ReviewComment.FirstOrDefault(x => x.Id == id && x.CreatorId == userId);
            if (comment == null)
            {
                return null;
            }

            return await DeleteAsync(comment);
        }

        public async Task<CommentReview> UpdateAsync(long id, long userId, string text)
        {
            var comment = _db.ReviewComment.FirstOrDefault(x => x.Id == id && x.CreatorId == userId);
            if (comment == null)
            {
                return null;
            }

            comment.Text = text;
            await _db.SaveChangesAsync();
            return comment;
        }
    }
}
