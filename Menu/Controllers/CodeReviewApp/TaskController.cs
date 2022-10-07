using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models;
using CodeReviewApp.Models.Returns;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            , long? creatorId, long? reviewerId, int? status, int pageNumber, int pageSize)
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

                    if (status == -1)
                    {
                        status = null;
                    }

                    CodeReviewTaskStatus? enumStatus = null;
                    if (status != null)
                    {
                        if (!Enum.GetValues(typeof(CodeReviewTaskStatus)).Cast<int>().Contains((int)status))
                        {
                            throw new SomeCustomException(Consts.CodeReviewErrorConsts.BadTaskReviewStatus);
                        }
                        else
                        {
                            enumStatus = (CodeReviewTaskStatus)status;
                        }
                    }


                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectService.ExistIfAccessAsync(projectId, userInfo);
                    if (!res.access)
                    {
                        throw new SomeCustomException(Consts.CodeReviewErrorConsts.ProjectNotFound);
                    }

                    var tasks = await _taskReviewService.GetTasksAsync(projectId
                        , nameLike, creatorId, reviewerId, enumStatus, pageNumber, pageSize);
                    var tasksCount = await _taskReviewService.GetTasksCountAsync(projectId
                        , nameLike, creatorId, reviewerId, enumStatus);
                    var taskReturn = tasks.Select(x => new TaskReviewReturn(x));

                    await _apiHealper.WriteResponseAsync(Response,
                        new { Tasks = taskReturn, TasksCount = tasksCount });// new { Tasks = taskReturn });//"projectInfo_" + projectId

                }, Response, _logger);

        }


        [Route("add-new-task")]
        [HttpPut]
        public async Task AddNewTask([FromForm] string taskName, [FromForm] long taskCreatorId
            , [FromForm] long? taskReviwerId, [FromForm] long projectId)
        {
            taskName = _apiHealper.StringValidator(taskName);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    if (taskReviwerId < 1)
                    {
                        taskReviwerId = null;
                    }

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectService.CreateTaskAsync(projectId, taskName, taskCreatorId, taskReviwerId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            Id = res.Id,
                            Name = res.Name,
                            CreatorId = res.CreatorId,
                            ReviewerId = res.ReviewerId,
                            Status = res.Status,
                        });

                }, Response, _logger);

        }

        [Route("update-task")]
        [HttpPatch]
        public async Task UpdateTask([FromForm] long taskId, [FromForm] string name
            , [FromForm] int status, [FromForm] long creatorId, [FromForm] long? reviewerId)
        {
            name = _apiHealper.StringValidator(name);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    if (reviewerId < 1)
                    {
                        reviewerId = null;
                    }

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    if (!Enum.GetValues(typeof(CodeReviewTaskStatus)).Cast<int>().Contains(status))
                    {
                        throw new SomeCustomException(Consts.CodeReviewErrorConsts.BadTaskReviewStatus);
                    }

                    var res = await _taskReviewService.UpdateAsync(new TaskReview()
                    {
                        Id = taskId,
                        Name = name,
                        Status = (CodeReviewTaskStatus)status,
                        CreatorId = creatorId,
                        ReviewerId = reviewerId
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
    }
}
