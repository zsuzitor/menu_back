using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models.Exceptions;
using DAL.Migrations;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;

namespace TaskManagementApp.Models.Services
{
    public class LabelService : ILabelService
    {

        private readonly IWorkTaskLabelRepository _labelRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkTaskRepository _workTaskRepository;
        public LabelService(IWorkTaskRepository workTaskRepository, IWorkTaskLabelRepository labelRepository, IProjectRepository projectRepository)
        {
            _labelRepository = labelRepository;
            _projectRepository = projectRepository;
            _workTaskRepository = workTaskRepository;
        }

        public async Task<bool> AddToTask(long labelId, long taskId, UserInfo userInfo)
        {
            var label = await _labelRepository.GetNoTrackAsync(labelId) ?? throw new SomeCustomException(Consts.ErrorConsts.LabelNotFound);

            var s = await ExistIfAccessAdminAsync(label.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }


            var task = await _workTaskRepository.GetNoTrackAsync(taskId) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            if (task.ProjectId != label.ProjectId)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            var exists = await _labelRepository.ExistsAsync(labelId, taskId);
            if (!exists)
            {
                var relation = await _labelRepository.CreateAsync(new WorkTaskLabelTaskRelation() { LabelId = labelId, TaskId = taskId });
                return true;
            }

            return false;
        }

        public async Task<WorkTaskLabel> Create(WorkTaskLabel req, UserInfo userInfo)
        {
            var s = await ExistIfAccessAdminAsync(req.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await _labelRepository.AddAsync(new WorkTaskLabel()
            {
                Name = req.Name,
                ProjectId = req.ProjectId,
            });

        }

        public async Task<bool> Delete(long id, UserInfo userInfo)
        {

            var label = await _labelRepository.GetAsync(id) ?? throw new SomeCustomException(Consts.ErrorConsts.LabelNotFound);

            var s = await ExistIfAccessAdminAsync(label.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            await _labelRepository.DeleteAsync(label);
            return true;
        }

        public async Task<List<WorkTaskLabel>> Get(long projectId, UserInfo userInfo)
        {
            var s = await ExistIfAccessAdminAsync(projectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }
            return await _labelRepository.GetForProjectAsync(projectId);
        }

        public async Task<bool> RemoveFromTask(long labelId, long taskId, UserInfo userInfo)
        {

            var label = await _labelRepository.GetNoTrackAsync(labelId) ?? throw new SomeCustomException(Consts.ErrorConsts.LabelNotFound);

            var s = await ExistIfAccessAdminAsync(label.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await _labelRepository.RemoveFromTaskIdExistAsync(labelId, taskId);
            
        }

        public async Task<bool> UpdateTaskLabels(List<long> labelId, long taskId, UserInfo userInfo)
        {

            var labels = await _labelRepository.GetNoTrackAsync(labelId) ?? throw new SomeCustomException(Consts.ErrorConsts.LabelNotFound);
            var projIds = labels.Select(x => x.ProjectId).Distinct().ToList();
            if (projIds.Count > 1)
            {
                //намешали лейблов из разных проектов
                throw new SomeCustomException(Consts.ErrorConsts.LabelNotFound);
            }

            var task = await _workTaskRepository.GetWithLabelRelationAsync(taskId) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);

            var s = await ExistIfAccessAdminAsync(task.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            if (projIds.Count != 0 && task.ProjectId != projIds.First())
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            foreach (var label in task.Labels.ToList())
            {
                //удаляем те что не переданы
                var sp = labelId.FirstOrDefault(x => x == label.LabelId);
                if (sp == default)
                {
                    task.Labels.Remove(label);
                }
            }

            foreach (var label in labelId)
            {
                //добавляем те что переданы
                var sp = task.Labels.FirstOrDefault(x => x.LabelId == label);
                if (sp == null)
                {
                    task.Labels.Add(new WorkTaskLabelTaskRelation() { LabelId = label, TaskId = task.Id });
                }
            }

            await _workTaskRepository.UpdateAsync(task);

            return true;
        }

        private async Task<bool> ExistIfAccessAdminAsync(long id, UserInfo userInfo)
        {
            return await _projectRepository.ExistIfAccessAdminAsync(id, userInfo.UserId);
        }
    }
}
