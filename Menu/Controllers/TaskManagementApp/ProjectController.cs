using BO.Models.TaskManagementApp.DAL.Domain;
using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IProjectService _projectService;
        private readonly IProjectUserService _projectUserService;


        public ProjectController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILogger<ProjectController> logger, IProjectService projectService,
             IProjectUserService projectUserService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = logger;

            _projectService = projectService;
            _projectUserService = projectUserService;
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
            projectName = _apiHealper.StringValidator(projectName);

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

                    var statuses = (await _projectService.GetStatusesAsync(projectId, userInfo)).Select(x => new WorkTaskStatusReturn(x));

                    var users = await _projectUserService.GetProjectUsersAsync(projectId, userInfo);
                    var usersReturn = users.Select(x => new ProjectUserReturn(x));

                    await _apiHealper.WriteResponseAsync(Response,
                        new { Users = usersReturn, Statuses = statuses });//, Tasks = taskReturn

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

        

        

        

       

        

        [Route("alert")]
        [HttpGet]
        public async Task Alert()
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   _ = _projectService.AlertAsync();//не ждем
                   await _apiHealper.WriteResponseAsync(Response
                       , "true");

               }, Response, _logger);
        }
    }
}
