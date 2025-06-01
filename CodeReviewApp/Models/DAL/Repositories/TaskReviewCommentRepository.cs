
using BO.Models.TaskManagementApp.DAL.Domain;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.DAL.Repositories
{
    public sealed class TaskReviewCommentRepository : GeneralRepository<CommentReview, long>, ITaskReviewCommentRepository
    {
        public TaskReviewCommentRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
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
