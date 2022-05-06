using BO.Models.CodeReviewApp.DAL.Domain;
using CodeReviewApp.Models.Returns;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Error.services.Interfaces;
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
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly IErrorService _errorService;
        private readonly ILogger _logger;

        private readonly IProjectService _projectService;
        private readonly IProjectUserService _projectUserService;
        private readonly ITaskReviewService _taskReviewService;


        public ProjectController(
             IJWTService jwtService, IApiHelper apiHealper, IErrorService errorService
            , ILogger<ProjectController> logger, IProjectService projectService,
             IProjectUserService projectUserService, ITaskReviewService taskReviewService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _errorService = errorService;
            _logger = logger;

            _projectService = projectService;
            _projectUserService = projectUserService;
            _taskReviewService = taskReviewService;
        }

        [Route("get-projects")]
        [HttpGet]
        public async Task GetProjects()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _projectService.GetProjectsByMainAppUserIdAsync(userInfo.UserId);
                    res = res ?? new System.Collections.Generic.List<Project>();
                    await _apiHealper.WriteResponseAsync(Response, res.Select(x => new ProjectInList(x)));

                }, Response, _logger);

        }

        [Route("add-new-project")]
        [HttpPut]
        public async Task AddProject([FromForm] string projectName)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectService.CreateAsync(projectName, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new ProjectInList(res));

                }, Response, _logger);

        }

        [Route("get-project-info")]
        [HttpGet]
        public async Task GetProjectInfo(long projectId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectService.GetByIdIfAccessAsync(projectId, userInfo);
                    if (res == null)
                    {
                        throw new SomeCustomException("project_not_found");
                    }

                    var users = await _projectUserService.GetProjectUsersAsync(projectId);
                    var tasks = await _taskReviewService.GetTasksAsync(projectId, null, null, null);
                    var usersReturn = users.Select(x => new ProjectUserReturn(x));
                    var taskReturn = tasks.Select(x => new TaskReviewReturn(x));

                    await _apiHealper.WriteResponseAsync(Response,
                        new { Users = usersReturn, Tasks = taskReturn });//"projectInfo_" + projectId

                }, Response, _logger);

        }


        [Route("add-new-user")]
        [HttpPut]
        public async Task AddNewUser([FromForm] string userName, [FromForm] long projectId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectService.CreateUserAsync(projectId, userName, null, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new { Id = res.Id, Name = res.UserName });

                }, Response, _logger);

        }

        [Route("add-new-task")]
        [HttpPut]
        public async Task AddNewTask([FromForm] string taskName, [FromForm] long taskCreatorId
            , [FromForm] long? taskReviwerId, [FromForm] long projectId)
        {
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
                        , new { Id = res.Id, Name = res.Name
                        , CreatorId = res.CreatorId, ReviewerId = res.ReviewerId, Status = res.Status });

                }, Response, _logger);

        }

        [Route("delete-project")]
        [HttpDelete]
        public async Task DeleteProject([FromForm] long projectId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectService.DeleteAsync(projectId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res,
                        });

                }, Response, _logger);

        }

        [Route("change-user")]
        [HttpPatch]
        public async Task ChangeUser([FromForm] long userId, [FromForm] string name, [FromForm] string email)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectUserService.ChangeAsync(userId, name, email, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res != null,
                        });

                }, Response, _logger);

        }

        [Route("delete-user")]
        [HttpDelete]
        public async Task DeleteUser([FromForm] long userId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectUserService.DeleteAsync(userId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res != null,
                        });

                }, Response, _logger);

        }

        [Route("update-task")]
        [HttpPatch]
        public async Task UpdateTask([FromForm] long taskId, [FromForm] string name
            , [FromForm] int status, [FromForm] long creatorId, [FromForm] long reviewerId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    if (!Enum.GetValues(typeof(CodeReviewTaskStatus)).Cast<int>().Contains(status))
                    {
                        throw new SomeCustomException("bad_task_review_status");
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
    }
}
