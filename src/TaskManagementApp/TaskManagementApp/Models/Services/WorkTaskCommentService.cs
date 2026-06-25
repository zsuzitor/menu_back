

using BO.Models.TaskManagementApp.DAL.Domain;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using System.Threading.Tasks;

namespace TaskManagementApp.Models.Services
{
    public sealed class WorkTaskCommentService : IWorkTaskCommentService
    {

        private readonly IWorkTaskCommentRepository _workTaskCommentRepository;
        private readonly IWorkTaskRepository _workTaskRepository;
        private readonly ITasksManagmentAuthRepository _auth;


        public WorkTaskCommentService(IWorkTaskCommentRepository workTaskCommentRepository, IWorkTaskRepository workTaskRepository, ITasksManagmentAuthRepository auth)
        {
            _workTaskCommentRepository = workTaskCommentRepository;
            _workTaskRepository = workTaskRepository;
            _auth = auth;
        }

        public async Task<WorkTaskComment> CreateAsync(WorkTaskComment comment)
        {
            return await _workTaskCommentRepository.AddAsync(comment);
        }

        public async Task<WorkTaskComment> DeleteAsync(long commentId, long userId)
        {
            //todo можно сильно оптимизировать
            var comment = await _workTaskCommentRepository.GetAsync(commentId);
            if (comment == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.CommentNotFound);
            }

            if (!await _auth.CanEditTask(comment.TaskId, userId))
                throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);

            var task = await _workTaskRepository.GetAsync(comment.TaskId, userId);
            if (task == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            }

            if(comment.CreatorId != userId)
            {
                throw new SomeCustomException(Consts.ErrorConsts.CommentNotFoundOrNotAccess);
            }

            var deleted = await _workTaskCommentRepository.DeleteAsync(comment);


            return deleted;
        }

        public async Task<WorkTaskComment> EditAsync(long commentId, string text, long userId)
        {
            //todo можно сильно оптимизировать

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new SomeCustomException(Consts.ErrorConsts.WorkTaskEmptyStatusName);
            }

            var comment = await _workTaskCommentRepository.GetAsync(commentId);
            if (comment == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.CommentNotFound);
            }


            if (!await _auth.CanEditTask(comment.TaskId, userId))
                throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);

            var task = await _workTaskRepository.GetAsync(comment.TaskId, userId);
            if (task == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            }


            if (comment.CreatorId != userId)
            {
                throw new SomeCustomException(Consts.ErrorConsts.CommentNotFoundOrNotAccess);
            }

            comment.Text = text;
            var updated = await _workTaskCommentRepository.UpdateAsync(comment);
            if (updated == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.CommentNotFoundOrNotAccess);
            }

            return updated;
        }

       
    }
}
