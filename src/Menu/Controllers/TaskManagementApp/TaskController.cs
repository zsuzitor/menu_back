using BO.Models.TaskManagementApp.DAL.Domain;
using TaskManagementApp.Models;
using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;
using Common.Models.Return;
using Menu.Models.TaskManagementApp.Requests;
using Menu.Models.TaskManagementApp.Mappers;
using Nest;
using TaskManagementApp.Models.DTO;
using System.Text.Json;
using TaskManagementApp.Models.Services;

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]
    [ApiController]
    [Produces("application/json")]

    public class TaskController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IProjectService _projectService;
        private readonly IWorkTaskService _workTaskService;


        public TaskController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILogger<ProjectController> logger, IProjectService projectService, IWorkTaskService workTaskService
            )
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = logger;

            _projectService = projectService;
            _workTaskService = workTaskService;
        }


        [Route("get-project-tasks")]
        [HttpGet]
        public async Task<ActionResult<GetProjectTasksReturn>> GetProjectTasks(long projectId, string nameLike
            , long? creatorId, long? executorId, int? statusId, int pageNumber, int pageSize, long? sprintId, long? labelId)
        {
            nameLike = _apiHealper.StringValidator(nameLike);
            if (string.IsNullOrWhiteSpace(nameLike))
            {
                nameLike = null;
            }

            if (creatorId < 1)
            {
                creatorId = null;
            }

            if (executorId < 1)
            {
                executorId = null;
            }

            if (statusId < 1)
            {
                statusId = null;
            }
            if (sprintId < 1)
            {
                sprintId = null;
            }
            if (labelId < 1)
            {
                labelId = null;
            }

            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

            var res = await _projectService.ExistIfAccessAsync(projectId, userInfo);
            if (!res.access)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFound);
            }

            var tasks = await _workTaskService.GetTasksAsync(new GetTasksByFilter()
            {
                CreatorId = creatorId,
                ExecutorId = executorId,
                StatusId = statusId,
                PageNumber = pageNumber,
                PageSize = pageSize,
                Name = nameLike,
                ProjectId = projectId,
                SprintId = sprintId,
                LabelId = labelId,
            });
            var tasksCount = await _workTaskService.GetTasksCountAsync(new GetTasksCountByFilter()
            {
                CreatorId = creatorId,
                ExecutorId = executorId,
                StatusId = statusId,
                Name = nameLike,
                ProjectId = projectId,
                SprintId = sprintId,
                LabelId = labelId,
            });
            var taskReturn = tasks.Select(x => new WorkTaskReturn(x)).ToList();

            return new JsonResult(new GetProjectTasksReturn() { Tasks = taskReturn, TasksCount = tasksCount }, GetJsonOptions());

        }

        [Route("get-project-task")]
        [HttpGet]
        public async Task<ActionResult<WorkTaskReturn>> GetProjectTask(long id)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

            var task = await _workTaskService.GetTaskFullAsync(id) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            var res = await _projectService.ExistIfAccessAsync(task.ProjectId, userInfo);
            if (!res.access)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFound);
            }

            var taskReturn = new WorkTaskReturn(task);
            return new JsonResult(taskReturn, GetJsonOptions());

        }

        [Route("copy-project-task")]
        [HttpPut]
        public async Task<ActionResult<WorkTaskReturn>> CopyProjectTask([FromForm] long id)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var task = await _workTaskService.CopyAsync(id, userInfo);
            var taskReturn = new WorkTaskReturn(task);
            return new JsonResult(taskReturn, GetJsonOptions());

        }

        [Route("add-task-relation")]
        [HttpPut]
        public async Task<ActionResult<WorkTaskRelationReturn>> AddTaskRelation([FromBody] AddNewTaskRelationRequest request)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var relation = await _workTaskService.CreateRelationAsync(request.Map(), userInfo);
            var r = new WorkTaskRelationReturn(relation);

            return new JsonResult(r, GetJsonOptions());

        }

        [Route("delete-task-relation")]
        [HttpDelete]
        public async Task<ActionResult<BoolResultNewReturn>> DeleteTaskRelation([FromForm] long id)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var relation = await _workTaskService.DeleteRelationAsync(id, userInfo);
            var r = new WorkTaskRelationReturn(relation);
            return new JsonResult(new BoolResultNewReturn(r!=null), GetJsonOptions());

        }

        [Route("add-new-task")]
        [HttpPut]
        public async Task<ActionResult<AddNewTaskReturn>> AddNewTask([FromBody] AddNewTaskRequest request)
        {
            if (request.TaskReviwerId < 1)
            {
                request.TaskReviwerId = null;
            }

            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            request.TaskName = _apiHealper.StringValidator(request.TaskName);
            request.TaskLink = _apiHealper.StringValidator(request.TaskLink);
            request.Description = _apiHealper.StringValidator(request.Description);
            var res = await _projectService.CreateTaskAsync(request.Map(), userInfo);
            return new JsonResult(new AddNewTaskReturn(res), GetJsonOptions());

        }

        [Route("update-task")]
        [HttpPatch]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateTask(UpdateTaskRequest request)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

            request.Name = _apiHealper.StringValidator(request.Name);
            request.Description = _apiHealper.StringValidator(request.Description);
            if (request.ExecutorId < 1)
            {
                request.ExecutorId = null;
            }


            var res = await _workTaskService.UpdateAsync(request.Map(), userInfo);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("delete-task")]
        [HttpDelete]
        public async Task<ActionResult<BoolResultNewReturn>> DeleteTask([FromForm] long taskId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

            var res = await _workTaskService.DeleteIfAccess(taskId, userInfo);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }


        [Route("update-name")]
        [HttpPatch]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateTaskName([FromForm] long id, [FromForm] string name)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            name = _apiHealper.StringValidator(name);
            var res = await _workTaskService.UpdateNameAsync(id, name, userInfo);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("update-description")]
        [HttpPatch]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateTaskDescription([FromForm] long id, [FromForm] string description)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            description = _apiHealper.StringValidator(description);
            var res = await _workTaskService.UpdateDescriptionAsync(id, description, userInfo);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("update-status")]
        [HttpPatch]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateTaskStatus([FromForm] long id, [FromForm] long statusId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _workTaskService.UpdateStatusAsync(id, statusId, userInfo);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());

        }

        [Route("update-executor")]
        [HttpPatch]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateExecutorStatus([FromForm] long id, [FromForm] long personId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _workTaskService.UpdateExecutorAsync(id, personId, userInfo);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("task-relations")]
        [HttpGet]
        public async Task<ActionResult<WorkTaskRelationReturn>> GetTaskRelations(long taskId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _workTaskService.GetRelationsAsync(taskId, userInfo);
            return new JsonResult(res.Select(x => new WorkTaskRelationReturn(x)), GetJsonOptions());
        }

        private JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = null, // PascalCase
                WriteIndented = true
            };
        }
    }
}
