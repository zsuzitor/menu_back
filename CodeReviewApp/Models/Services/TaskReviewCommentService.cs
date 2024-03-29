﻿
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
        //private readonly ITaskReviewService _taskReviewService;
        private readonly ITaskReviewRepository _taskReviewRepository;


        public TaskReviewCommentService(ITaskReviewCommentRepository taskReviewCommentRepository,
            IProjectUserService projectUserService, ITaskReviewRepository taskReviewRepository)
        {
            _taskReviewCommentRepository = taskReviewCommentRepository;
            _projectUserService = projectUserService;
            _taskReviewRepository = taskReviewRepository;
        }

        public async Task<CommentReview> CreateAsync(CommentReview comment)
        {
            return await _taskReviewCommentRepository.AddAsync(comment);
        }

        public async Task<CommentReview> DeleteAsync(long commentId, UserInfo userInfo)
        {
            //todo можно сильно оптимизировать
            var comment = await _taskReviewCommentRepository.GetAsync(commentId);
            if (comment == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.CommentNotFound);
            }

            //var task = await _taskReviewService.GetByIdIfAccessAsync(comment.TaskId, userInfo);
            var task = await _taskReviewRepository.GetNoTrackAsync(comment.TaskId);
            //if (task == null)
            //{
            //    throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskNotFound);
            //}

            var user = await _projectUserService.GetByMainAppIdAsync(userInfo, task.ProjectId);
            if (user == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.UserNotFound);
            }

            if(comment.CreatorId != user.Id)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.CommentNotFoundOrNotAccess);
            }

            //var deleted = await _taskReviewCommentRepository.DeleteAsync(commentId, user.Id);
            var deleted = await _taskReviewCommentRepository.DeleteAsync(comment);

            //var deleted =  await _taskReviewCommentRepository.DeleteAsync(commentId, user.Id);
            //if (deleted == null)
            //{
            //    throw new SomeCustomException(Consts.CodeReviewErrorConsts.CommentNotFoundOrNotAccess);
            //}

            return deleted;
        }

        public async Task<CommentReview> EditAsync(long commentId, string text, UserInfo userInfo)
        {
            //todo можно сильно оптимизировать
            //var user = await _projectUserService.GetByMainAppIdAsync(userInfo);
            //if (user == null)
            //{
            //    throw new SomeCustomException(Consts.CodeReviewErrorConsts.UserNotFound);
            //}
            var comment = await _taskReviewCommentRepository.GetAsync(commentId);
            if (comment == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.CommentNotFound);
            }

            //var task = await _taskReviewService.GetByIdIfAccessAsync(comment.TaskId, userInfo);
            var task = await _taskReviewRepository.GetNoTrackAsync(comment.TaskId);

            //if (task == null)
            //{
            //    throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskNotFound);
            //}

            var user = await _projectUserService.GetByMainAppIdAsync(userInfo, task.ProjectId);
            if (user == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.UserNotFound);
            }

            if (comment.CreatorId != user.Id)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.CommentNotFoundOrNotAccess);
            }

            comment.Text = text;
            var updated = await _taskReviewCommentRepository.UpdateAsync(comment);
            if (updated == null)
            {
                throw new SomeCustomException(Consts.CodeReviewErrorConsts.CommentNotFoundOrNotAccess);
            }

            return updated;
        }

       
    }
}
