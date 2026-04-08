using Auth.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models;
using Common.Models.Exceptions;
using Common.Models.Return;
using DAL.Migrations;
using jwtLib.JWTAuth.Interfaces;
using Menu.Host.Infrastructure;
using Menu.Models.TaskManagementApp.Mappers;
using Menu.Models.TaskManagementApp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagementApp.Models;
using TaskManagementApp.Models.DTO;
using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services;
using TaskManagementApp.Models.Services.Interfaces;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Tags("taskmanagement")]

    public class TaskController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IProjectService _projectService;
        private readonly IWorkTaskService _workTaskService;


        public TaskController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILoggerFactory loggerFactory, IProjectService projectService, IWorkTaskService workTaskService
            )
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = loggerFactory.CreateLogger(Constants.Loggers.MenuApp);

            _projectService = projectService;
            _workTaskService = workTaskService;
        }

        [Route("get-task-select-info")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<GetProjectTaskSelectInfoReturn>> GetProjectTaskSelectInfo(long taskId)
        {

            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var task = await _workTaskService.GetProjectTaskSelectInfoAsync(taskId, userInfo.UserId);

            return new JsonResult(new GetProjectTaskSelectInfoReturn() { Id = task.Id, Name = task.Name }, GetJsonOptions());
        }


        [Route("get-project-tasks")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult<GetProjectTasksReturn>> GetProjectTasks([FromBody] GetProjectTasksByFilterRequest request)
        {
            request.NameLike = _apiHealper.StringValidator(request.NameLike);
            if (string.IsNullOrWhiteSpace(request.NameLike))
            {
                request.NameLike = null;
            }

            if (request.CreatorId < 1)
            {
                request.CreatorId = null;
            }

            if (request.ExecutorId < 1)
            {
                request.ExecutorId = null;
            }

            if (request.StatusId < 1)
            {
                request.StatusId = null;
            }
            if (request.SprintId < 1)
            {
                request.SprintId = null;
            }
            if (request.PresetId < 1)
            {
                request.PresetId = null;
            }
            if (request.LabelId == null || request.LabelId.Length ==0)
            {
                request.LabelId = null;
            }

            var userId = User.GetUserId();

            var res = await _projectService.ExistIfAccessAsync(request.ProjectId, userId);
            if (!res.access)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFound);
            }

            var tasks = await _workTaskService.GetTasksAsync(new GetTasksByFilter()
            {
                CreatorId = request.CreatorId,
                ExecutorId = request.ExecutorId,
                StatusId = request.StatusId,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Name = request.NameLike,
                ProjectId = request.ProjectId,
                SprintId = request.SprintId,
                PresetId = request.PresetId,
                LabelIds = request.LabelId?.ToList(),
            });
            var tasksCount = await _workTaskService.GetTasksCountAsync(new GetTasksCountByFilter()
            {
                CreatorId = request.CreatorId,
                ExecutorId = request.ExecutorId,
                StatusId = request.StatusId,
                Name = request.NameLike,
                ProjectId = request.ProjectId,
                SprintId = request.SprintId,
                PresetId = request.PresetId,
                LabelIds = request.LabelId?.ToList(),
            });
            var taskReturn = tasks.Select(x => new WorkTaskReturn(x)).ToList();

            return new JsonResult(new GetProjectTasksReturn() { Tasks = taskReturn, TasksCount = tasksCount }, GetJsonOptions());

        }

        [Route("get-project-task")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTaskReturn>> GetProjectTask(long id)
        {
            var userId = User.GetUserId();

            var task = await _workTaskService.GetTaskFullAsync(id) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
            var res = await _projectService.ExistIfAccessAsync(task.ProjectId, userId);
            if (!res.access)
            {
                throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFound);
            }

            var taskReturn = new WorkTaskReturn(task);
            return new JsonResult(taskReturn, GetJsonOptions());

        }

        [Route("copy-project-task")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTaskReturn>> CopyProjectTask([FromForm] long id)
        {
            var userId = User.GetUserId();
            var task = await _workTaskService.CopyAsync(id, userId);
            var taskReturn = new WorkTaskReturn(task);
            return new JsonResult(taskReturn, GetJsonOptions());

        }

        [Route("get-project-task-name")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTaskReturn>> GetProjectTaskName(long id)
        {
            var userId = User.GetUserId();
            var task = await _workTaskService.GetTaskNameAsync(id, userId);
            return new JsonResult(new TaskNameReturn(task), GetJsonOptions());

        }

        [Route("add-task-relation")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTaskRelationReturn>> AddTaskRelation([FromBody] AddNewTaskRelationRequest request)
        {
            var userId = User.GetUserId();
            var relation = await _workTaskService.CreateRelationAsync(request.Map(), userId);
            var r = new WorkTaskRelationReturn(relation);

            return new JsonResult(r, GetJsonOptions());

        }

        [Route("delete-task-relation")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> DeleteTaskRelation([FromForm] long id)
        {
            var userId = User.GetUserId();
            var relation = await _workTaskService.DeleteRelationAsync(id, userId);
            var r = new WorkTaskRelationReturn(relation);
            return new JsonResult(new BoolResultNewReturn(r!=null), GetJsonOptions());

        }

        [Route("add-new-task")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<AddNewTaskReturn>> AddNewTask([FromBody] AddNewTaskRequest request)
        {
            if (request.TaskReviwerId < 1)
            {
                request.TaskReviwerId = null;
            }

            var userId = User.GetUserId();
            request.TaskName = _apiHealper.StringValidator(request.TaskName);
            request.TaskLink = _apiHealper.StringValidator(request.TaskLink);
            request.Description = _apiHealper.StringValidator(request.Description);
            var res = await _projectService.CreateTaskAsync(request.Map(), userId);
            return new JsonResult(new AddNewTaskReturn(res), GetJsonOptions());

        }

        [Route("update-task")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateTask(UpdateTaskRequest request)
        {
            var userId = User.GetUserId();

            request.Name = _apiHealper.StringValidator(request.Name);
            //request.Description = _apiHealper.StringValidator(request.Description);
            if (request.ExecutorId < 1)
            {
                request.ExecutorId = null;
            }


            var res = await _workTaskService.UpdateAsync(request.Map(), userId);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("delete-task")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> DeleteTask([FromForm] long taskId)
        {
            var userId = User.GetUserId();

            var res = await _workTaskService.DeleteIfAccess(taskId, userId);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }


        [Route("update-name")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateTaskName([FromForm] long id, [FromForm] string name)
        {
            var userId = User.GetUserId();
            name = _apiHealper.StringValidator(name);
            var res = await _workTaskService.UpdateNameAsync(id, name, userId);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("update-description")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateTaskDescription([FromForm] long id, [FromForm] string description)
        {
            var userId = User.GetUserId();
            description = _apiHealper.StringValidator(description);
            var res = await _workTaskService.UpdateDescriptionAsync(id, description, userId);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("update-status")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateTaskStatus([FromForm] long id, [FromForm] long statusId)
        {
            var userId = User.GetUserId();
            var res = await _workTaskService.UpdateStatusAsync(id, statusId, userId);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());

        }

        [Route("update-executor")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateExecutorStatus([FromForm] long id, [FromForm] long personId)
        {
            var userId = User.GetUserId();
            var res = await _workTaskService.UpdateExecutorAsync(id, personId, userId);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("task-relations")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTaskRelationReturn>> GetTaskRelations(long taskId)
        {
            var userId = User.GetUserId();
            var res = await _workTaskService.GetRelationsAsync(taskId, userId);
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
