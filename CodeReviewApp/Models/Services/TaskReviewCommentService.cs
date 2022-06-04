
using BO.Models.Auth;
using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.DAL.Repositories.Interfaces;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using System.Threading.Tasks;

namespace CodeReviewApp.Models.Services
{
    public sealed class TaskReviewCommentService : ITaskReviewCommentService
    {

        private readonly ITaskReviewCommentRepository _taskReviewCommentRepository;
        private readonly IProjectUserService _projectUserService;


        public TaskReviewCommentService(ITaskReviewCommentRepository taskReviewCommentRepository,
            IProjectUserService projectUserService)
        {
            _taskReviewCommentRepository = taskReviewCommentRepository;
            _projectUserService = projectUserService;
        }

        public async Task<CommentReview> CreateAsync(CommentReview comment)
        {
            return await _taskReviewCommentRepository.AddAsync(comment);
        }

        public async Task<CommentReview> DeleteAsync(long commentId, UserInfo userInfo)
        {
            var user = await _projectUserService.GetByMainAppIdAsync(userInfo);
            if (user == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.UserNotFound);
            }

            return await _taskReviewCommentRepository.DeleteAsync(commentId, user.Id);
        }

        public async Task<CommentReview> EditAsync(long commentId, string text, UserInfo userInfo)
        {
            var user = await _projectUserService.GetByMainAppIdAsync(userInfo);
            if (user == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.UserNotFound);
            }

            return await _taskReviewCommentRepository.UpdateAsync(commentId, user.Id, text);
        }

       
    }
}
