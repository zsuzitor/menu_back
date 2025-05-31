using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models;
using CodeReviewApp.Models.Returns;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Linq;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.CodeReviewApp
{
    [Route("api/codereview/[controller]")]

    public class TaskController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IProjectService _projectService;
        private readonly ITaskReviewService _taskReviewService;


        public TaskController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILogger<ProjectController> logger, IProjectService projectService, ITaskReviewService taskReviewService
            )
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = logger;

            _projectService = projectService;
            _taskReviewService = taskReviewService;
        }


        [Route("get-project-tasks")]
        [HttpGet]
        public async Task GetProjectTasks(long projectId, string nameLike
            , long? creatorId, long? reviewerId, int? statusId, int pageNumber, int pageSize)
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

                    if (reviewerId == -1)
                    {
                        reviewerId = null;
                    }

                    if (statusId == -1)
                    {
                        statusId = null;
                    }


                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectService.ExistIfAccessAsync(projectId, userInfo);
                    if (!res.access)
                    {
                        throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFound);
                    }

                    var tasks = await _taskReviewService.GetTasksAsync(projectId
                        , nameLike, creatorId, reviewerId, statusId, pageNumber, pageSize);
                    var tasksCount = await _taskReviewService.GetTasksCountAsync(projectId
                        , nameLike, creatorId, reviewerId, statusId);
                    var taskReturn = tasks.Select(x => new TaskReviewReturn(x));

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

                    var task = await _taskReviewService.GetTaskWithCommentsAsync(id) ?? throw new SomeCustomException(Consts.CodeReviewErrorConsts.TaskNotFound);
                    var res = await _projectService.ExistIfAccessAsync(task.ProjectId, userInfo);
                    if (!res.access)
                    {
                        throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFound);
                    }


                    var taskReturn = new TaskReviewReturn(task);

                    await _apiHealper.WriteResponseAsync(Response,
                        taskReturn);// new { Tasks = taskReturn });//"projectInfo_" + projectId

                }, Response, _logger);

        }

        [Route("add-new-task")]
        [HttpPut]
        public async Task AddNewTask([FromForm] string taskName
            , [FromForm] long? taskReviwerId, [FromForm] string taskLink, [FromForm] long projectId, [FromForm] long statusId
            , [FromForm] string description)
        {
            taskName = _apiHealper.StringValidator(taskName);
            taskLink = _apiHealper.StringValidator(taskLink);
            description = _apiHealper.StringValidator(description);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    if (taskReviwerId < 1)
                    {
                        taskReviwerId = null;
                    }

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectService.CreateTaskAsync(new TaskReview()
                    {
                        Name = taskName,
                        ReviewerId = taskReviwerId,
                        ProjectId = projectId,
                        StatusId = statusId,
                        Description = description,
                    }, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            Id = res.Id,
                            Name = res.Name,
                            CreatorId = res.CreatorId,
                            ReviewerId = res.ReviewerId,
                            Status = new TaskReviewStatusReturn(res.Status),
                            CreateDate = res.CreateDate.ToString(),
                            LastUpdateDate = res.LastUpdateDate.ToString()
                        });

                }, Response, _logger);

        }

        [Route("update-task")]
        [HttpPatch]
        public async Task UpdateTask([FromForm] long taskId, [FromForm] string name
            , [FromForm] int statusId, [FromForm] long? reviewerId
            , [FromForm] string description)
        {
            name = _apiHealper.StringValidator(name);
            description = _apiHealper.StringValidator(description);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    if (reviewerId < 1)
                    {
                        reviewerId = null;
                    }

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    

                    var res = await _taskReviewService.UpdateAsync(new TaskReview()
                    {
                        Id = taskId,
                        Name = name,
                        StatusId = statusId,
                        //CreatorId = creatorId,
                        ReviewerId = reviewerId,
                        Description = description,
                    }, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res != null,
                        });

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

                    var res = await _taskReviewService.DeleteIfAccess(taskId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res != null,
                        });

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


                    var res = await _taskReviewService.UpdateNameAsync(id,name, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res != null,
                        });

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


                    var res = await _taskReviewService.UpdateDescriptionAsync(id, description, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res != null,
                        });

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


                    var res = await _taskReviewService.UpdateStatusAsync(id, statusId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res != null,
                        });

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


                    var res = await _taskReviewService.UpdateExecutorAsync(id, personId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res != null,
                        });

                }, Response, _logger);

        }
    }
}
