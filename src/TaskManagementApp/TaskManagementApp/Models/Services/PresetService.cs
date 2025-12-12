using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models.Exceptions;
using Pipelines.Sockets.Unofficial.Arenas;
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
    public class PresetService : IPresetService
    {
        private readonly IPresetRepository _presetRepo;
        private readonly IProjectRepository _projectRepository;
        private readonly IWorkTaskLabelRepository _labelRepository;
        private readonly ITaskStatusRepository _taskStatusRepository;
        private readonly IProjectUserService _projectUserService;

        public PresetService(IPresetRepository presetRepo, IProjectRepository projectRepository, IWorkTaskLabelRepository labelRepository, ITaskStatusRepository taskStatusRepository, IProjectUserService projectUserService)
        {
            _presetRepo = presetRepo;
            _projectRepository = projectRepository;
            _labelRepository = labelRepository;
            _taskStatusRepository = taskStatusRepository;
            _projectUserService = projectUserService;
        }

        public async Task<Preset> CreateAsync(long projectId, string name, UserInfo userInfo)
        {
            var s = await ExistIfAccessAdminAsync(projectId, userInfo);
            if (!s)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            var newPreset = new Preset()
            {
                Name = name,
                ProjectId = projectId,
            };
            return await _presetRepo.AddAsync(newPreset);

        }

        public async Task<Preset> DeleteAsync(long presetId, UserInfo userInfo)
        {
            var oldPreset = await _presetRepo.GetAsync(presetId) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.PresetNotFound);

            var s = await ExistIfAccessAdminAsync(oldPreset.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await _presetRepo.DeleteAsync(oldPreset);
        }

        public async Task<Preset> EditAsync(Preset preset, UserInfo userInfo)
        {
            var oldPreset = await _presetRepo.GetWithLabelsAsync(preset.Id) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.PresetNotFound);

            var s = await ExistIfAccessAdminAsync(oldPreset.ProjectId, userInfo);
            if (!s)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }


            var labels = await _labelRepository.GetNoTrackAsync(preset.Labels.Select(x=>x.LabelId).ToList()) ?? throw new SomeCustomException(Consts.ErrorConsts.LabelNotFound);
            var projIds = labels.Select(x => x.ProjectId).Distinct().ToList();
            if (projIds.Count > 1)
            {
                //намешали лейблов из разных проектов
                throw new SomeCustomException(Consts.ErrorConsts.LabelNotFound);
            }



            if (projIds.Count != 0 && oldPreset.ProjectId != projIds.First())
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            if (preset.StatusId != null && preset.StatusId != oldPreset.StatusId)
            {
                var status = await _taskStatusRepository.GetAsync(preset.StatusId.Value);
                if (status == null || status.ProjectId != oldPreset.ProjectId)
                {
                    throw new SomeCustomException(Consts.ErrorConsts.PresetNotFound);
                }
            }


            if (preset.ExecutorId != null && preset.ExecutorId != oldPreset.ExecutorId)
            {
                var executorExist = await _projectUserService.ExistAsync(oldPreset.ProjectId, preset.ExecutorId.Value);
                if (!executorExist)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.UserNotFound);
                }
            }

            if (preset.CreatorId != null && preset.CreatorId != oldPreset.CreatorId)
            {
                var creatorExist = await _projectUserService.ExistAsync(oldPreset.ProjectId, preset.CreatorId.Value);
                if (!creatorExist)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.UserNotFound);
                }
            }


            var labelRelationForDel = new List<WorkTaskLabelPresetRelation>();
            foreach (var label in oldPreset.Labels.ToList())
            {
                //удаляем те что не переданы
                var sp = preset.Labels.FirstOrDefault(x => x.LabelId == label.LabelId);
                if (sp == default)
                {
                    labelRelationForDel.Add(label);
                    oldPreset.Labels.Remove(label);
                }
            }

            foreach (var label in preset.Labels)
            {
                //добавляем те что переданы
                var sp = oldPreset.Labels.FirstOrDefault(x => x.LabelId == label.LabelId);
                if (sp == null)
                {
                    oldPreset.Labels.Add(new WorkTaskLabelPresetRelation() { LabelId = label.LabelId, PresetId = oldPreset.Id });
                }
            }

            await _presetRepo.DeleteAsync(labelRelationForDel);//todo хорошо бы в транзакции


            oldPreset.StatusId = preset.StatusId;
            oldPreset.CreatorId = preset.CreatorId;
            oldPreset.ExecutorId = preset.ExecutorId;
            oldPreset.SprintId = preset.SprintId;
            oldPreset.Name = preset.Name;


            await _presetRepo.UpdateAsync(oldPreset);
            return oldPreset;


        }



        public async Task<List<Preset>> GetAllAsync(long projectId, UserInfo userInfo)
        {
            var s = await ExistIfAccessAdminAsync(projectId, userInfo);
            if (!s)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await _presetRepo.GetAllAsync(projectId);
        }

        public async Task<List<Preset>> GetAllAsync(long projectId)
        {
            return await _presetRepo.GetAllAsync(projectId);
        }

        private async Task<bool> ExistIfAccessAdminAsync(long id, UserInfo userInfo)
        {
            return await _projectRepository.ExistIfAccessAdminAsync(id, userInfo.UserId);
        }
    }
}
