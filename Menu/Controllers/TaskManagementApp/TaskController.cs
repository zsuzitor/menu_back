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

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]

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
        public async Task GetProjectTasks(long projectId, string nameLike
            , long? creatorId, long? executorId, int? statusId, int pageNumber, int pageSize)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    if (string.IsNullOrWhiteSpace(nameLike))
                    {
                        nameLike = null;
                    }

                    if (creatorId == -1)
                    {
                        creatorId = null;
                    }

                    if (executorId == -1)
                    {
                        executorId = null;
                    }

                    if (statusId == -1)
                    {
                        statusId = null;
                    }


                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectService.ExistIfAccessAsync(projectId, userInfo);
                    if (!res.access)
                    {
                        throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFound);
                    }

                    var tasks = await _workTaskService.GetTasksAsync(projectId
                        , nameLike, creatorId, executorId, statusId, pageNumber, pageSize);
                    var tasksCount = await _workTaskService.GetTasksCountAsync(projectId
                        , nameLike, creatorId, executorId, statusId);
                    var taskReturn = tasks.Select(x => new WorkTaskReturn(x));

                    await _apiHealper.WriteResponseAsync(Response,
                        new { Tasks = taskReturn, TasksCount = tasksCount });// new { Tasks = taskReturn });//"projectInfo_" + projectId

                }, Response, _logger);

        }

        [Route("get-project-task")]
        [HttpGet]
        public async Task GetProjectTask(long id)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var task = await _workTaskService.GetTaskWithCommentsAsync(id) ?? throw new SomeCustomException(Consts.ErrorConsts.TaskNotFound);
                    var res = await _projectService.ExistIfAccessAsync(task.ProjectId, userInfo);
                    if (!res.access)
                    {
                        throw new SomeCustomException(Consts.ErrorConsts.ProjectNotFound);
                    }


                    var taskReturn = new WorkTaskReturn(task);

                    await _apiHealper.WriteResponseAsync(Response,
                        taskReturn);// new { Tasks = taskReturn });//"projectInfo_" + projectId

                }, Response, _logger);

        }

        [Route("add-new-task")]
        [HttpPut]
        public async Task AddNewTask([FromBody] AddNewTaskRequest request)
        {
            request.TaskName = _apiHealper.StringValidator(request.TaskName);
            request.TaskLink = _apiHealper.StringValidator(request.TaskLink);
            request.Description = _apiHealper.StringValidator(request.Description);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    if (request.TaskReviwerId < 1)
                    {
                        request.TaskReviwerId = null;
                    }

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectService.CreateTaskAsync(request.Map(), userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            Id = res.Id,
                            Name = res.Name,
                            CreatorId = res.CreatorId,
                            ExecutorId = res.ExecutorId,
                            Status = new WorkTaskStatusReturn(res.Status),
                            CreateDate = res.CreateDate.ToString(),
                            LastUpdateDate = res.LastUpdateDate.ToString()
                        });

                }, Response, _logger);

        }

        [Route("update-task")]
        [HttpPatch]
        public async Task UpdateTask(UpdateTaskRequest request)
        {
            request.Name = _apiHealper.StringValidator(request.Name);
            request.Description = _apiHealper.StringValidator(request.Description);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    if (request.ExecutorId < 1)
                    {
                        request.ExecutorId = null;
                    }

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _workTaskService.UpdateAsync(request.Map(), userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new BoolResultReturn(res != null));

                }, Response, _logger);

        }

        [Route("delete-task")]
        [HttpDelete]
        public async Task DeleteTask([FromForm] long taskId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _workTaskService.DeleteIfAccess(taskId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new BoolResultReturn(res != null));

                }, Response, _logger);

        }


        [Route("update-name")]
        [HttpPatch]
        public async Task UpdateTaskName([FromForm] long id, [FromForm] string name)
        {
            name = _apiHealper.StringValidator(name);

            await _apiHealper.DoStandartSomething(
                async () =>
                {

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);


                    var res = await _workTaskService.UpdateNameAsync(id,name, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new BoolResultReturn(res != null));

                }, Response, _logger);

        }

        [Route("update-description")]
        [HttpPatch]
        public async Task UpdateTaskDescription([FromForm] long id, [FromForm] string description)
        {
            description = _apiHealper.StringValidator(description);

            await _apiHealper.DoStandartSomething(
                async () =>
                {

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);


                    var res = await _workTaskService.UpdateDescriptionAsync(id, description, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new BoolResultReturn(res != null));

                }, Response, _logger);

        }

        [Route("update-status")]
        [HttpPatch]
        public async Task UpdateTaskStatus([FromForm] long id, [FromForm] long statusId)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);


                    var res = await _workTaskService.UpdateStatusAsync(id, statusId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new BoolResultReturn(res != null));

                }, Response, _logger);

        }

        [Route("update-executor")]
        [HttpPatch]
        public async Task UpdateExecutorStatus([FromForm] long id, [FromForm] long personId)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);


                    var res = await _workTaskService.UpdateExecutorAsync(id, personId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new BoolResultReturn(res != null));

                }, Response, _logger);

        }
    }
}
