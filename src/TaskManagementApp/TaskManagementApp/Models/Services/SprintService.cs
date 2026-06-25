using BO.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models.Exceptions;
using Pipelines.Sockets.Unofficial.Arenas;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.Services.Interfaces;

namespace TaskManagementApp.Models.Services
{
    internal class SprintService : ISprintService
    {
        private readonly ISprintCachedRepository _sprintRepository;
        private readonly IWorkTaskRepository _workTaskRepository;
        private readonly ITasksManagmentAuthRepository _auth;
        public SprintService(ISprintCachedRepository sprintRepository
            , IWorkTaskRepository workTaskRepository, ITasksManagmentAuthRepository auth)
        {
            _sprintRepository = sprintRepository;
            _workTaskRepository = workTaskRepository;
            _auth = auth;
        }

        public async Task<bool> AddTaskToSprint(long sprintId, long taskId, long userId)
        {
            var sprint = await _sprintRepository.GetNoTrackAsync(sprintId) ?? throw new SomeCustomException(Consts.ErrorConsts.SprintNotFound);

            if (!await _auth.CanEditProject(sprint.ProjectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            var task = await _workTaskRepository.GetNoTrackAsync(taskId) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            if (task.ProjectId != sprint.ProjectId)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            var exists = await _sprintRepository.ExistsAsync(sprintId, taskId);
            if (!exists)
            {
                var relation = await _sprintRepository.CreateAsync(new WorkTaskSprintRelation() { SprintId = sprintId, TaskId = taskId });
                return true;
            }

            return false;

        }

        public async Task<ProjectSprint> Create(ProjectSprint req, long userId)
        {
            if (!await _auth.CanEditProject(req.ProjectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await _sprintRepository.AddAsync(new ProjectSprint()
            {
                Name = req.Name,
                ProjectId = req.ProjectId,
                StartDate = req.StartDate,
                EndDate = req.EndDate
            });
        }

        public async Task<ProjectSprint> Update(ProjectSprint req, long userId)
        {
            var sprint = await _sprintRepository.GetAsync(req.Id) ?? throw new SomeCustomException(Consts.ErrorConsts.SprintNotFound);
            if (!await _auth.CanEditProject(sprint.ProjectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            sprint.StartDate = req.StartDate;
            sprint.EndDate = req.EndDate;
            sprint.Name = req.Name;

            return await _sprintRepository.UpdateAsync(sprint);
        }


        public async Task<ProjectSprint> Delete(long id, long userId)
        {
            var sprint = await _sprintRepository.GetAsync(id) ?? throw new SomeCustomException(Consts.ErrorConsts.SprintNotFound);

            if (!await _auth.CanEditProject(sprint.ProjectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            await _sprintRepository.DeleteAsync(sprint);
            return sprint;
        }

        public async Task<bool> DeleteTaskFromSprint(long sprintId, long taskId, long userId)
        {

            var sprint = await _sprintRepository.GetNoTrackAsync(sprintId) ?? throw new SomeCustomException(Consts.ErrorConsts.SprintNotFound);

            if (!await _auth.CanEditProject(sprint.ProjectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await _sprintRepository.RemoveFromTaskIdExistAsync(sprintId, taskId);

        }

        public async Task<ProjectSprint> Get(long sprintId, long userId)
        {
            var sprint = await _sprintRepository.GetNoTrackAsync(sprintId) ?? throw new SomeCustomException(Consts.ErrorConsts.SprintNotFound);

            if (!await _auth.CanViewProject(sprint.ProjectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return sprint;
        }

        public async Task<List<WorkTask>> GetTasks(long sprintId, long userId)
        {

            var sprint = await _sprintRepository.GetWithTasks(sprintId) ?? throw new SomeCustomException(Consts.ErrorConsts.SprintNotFound);

            if (!await _auth.CanViewProject(sprint.ProjectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return sprint.Tasks.Select(x => x.Task).ToList();
        }

        public async Task<List<ProjectSprint>> GetForProject(long projectId)
        {

            return await _sprintRepository.GetForProject(projectId);

        }


        public async Task<List<ProjectSprint>> GetForProjectWithRights(long projectId, long userId)
        {
            if (!await _auth.CanViewProject(projectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            return await _sprintRepository.GetForProject(projectId);
        }

        public async Task<bool> UpdateTaskSprints(List<long> sprintId, long taskId, long userId)
        {
            if (sprintId == null)
            {
                throw new SomeCustomException(Consts.ErrorConsts.SprintNotFound);
            }

            var sprints = await _sprintRepository.GetNoTrackAsync(sprintId) ?? throw new SomeCustomException(Consts.ErrorConsts.SprintNotFound);
            var projIds = sprints.Select(x=>x.ProjectId).Distinct().ToList();
            if (projIds.Count > 1)
            {
                //намешали спринтов из разных проектов
                throw new SomeCustomException(Consts.ErrorConsts.SprintNotFound);
            }

            var task = await _workTaskRepository.GetWithSprintRelationAsync(taskId) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);

            if (!await _auth.CanEditProject(task.ProjectId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            if (projIds.Count != 0 && task.ProjectId != projIds.First())
            {
                    throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFoundOrNotAccesible);
            }

            foreach (var sprint in task.Sprints.ToList()) {
                //удаляем те что не переданы
                var sp = sprintId.FirstOrDefault(x => x == sprint.SprintId);
                if (sp == default)
                {
                    task.Sprints.Remove(sprint);
                }
            }

            foreach (var sprint in sprintId)
            {
                //добавляем те что переданы
                var sp = task.Sprints.FirstOrDefault(x => x.SprintId == sprint);
                if (sp == null)
                {
                    task.Sprints.Add(new WorkTaskSprintRelation() {SprintId= sprint, TaskId=task.Id });
                }
            }

            await _workTaskRepository.UpdateAsync(task);

            return true;
        }


 
    }
}
