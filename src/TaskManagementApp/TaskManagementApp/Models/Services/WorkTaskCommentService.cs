
using BO.Models.Auth;
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
        private readonly IProjectUserService _projectUserService;
        private readonly IWorkTaskRepository _workTaskRepository;


        public WorkTaskCommentService(IWorkTaskCommentRepository workTaskCommentRepository,
            IProjectUserService projectUserService, IWorkTaskRepository workTaskRepository)
        {
            _workTaskCommentRepository = workTaskCommentRepository;
            _projectUserService = projectUserService;
            _workTaskRepository = workTaskRepository;
        }

        public async Task<WorkTaskComment> CreateAsync(WorkTaskComment comment)
        {
            return await _workTaskCommentRepository.AddAsync(comment);
        }

        public async Task<WorkTaskComment> DeleteAsync(long commentId, UserInfo userInfo)
        {
            //todo можно сильно оптимизировать
            var comment = await _workTaskCommentRepository.GetAsync(commentId);
            if (comment == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.CommentNotFound);
            }

            var task = await _workTaskRepository.GetNoTrackAsync(comment.TaskId);
            //if (task == null)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            //}

            var user = await _projectUserService.GetByMainAppIdAsync(userInfo, task.ProjectId);
            if (user == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.UserNotFound);
            }

            if(comment.CreatorId != user.Id)
            {
                throw new SomeCustomException(Consts.ErrorConsts.CommentNotFoundOrNotAccess);
            }

            var deleted = await _workTaskCommentRepository.DeleteAsync(comment);


            return deleted;
        }

        public async Task<WorkTaskComment> EditAsync(long commentId, string text, UserInfo userInfo)
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

            var task = await _workTaskRepository.GetNoTrackAsync(comment.TaskId);

            //if (task == null)
            //{
            //    throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            //}

            var user = await _projectUserService.GetByMainAppIdAsync(userInfo, task.ProjectId);
            if (user == null)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.UserNotFound);
            }

            if (comment.CreatorId != user.Id)
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
