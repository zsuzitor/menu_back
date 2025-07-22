using Common.Models.Entity;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using System.Threading.Tasks;
using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services.Interfaces;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]
    public class SprintController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;
        private readonly IProjectService _projectService;
        private readonly ISprintService _sprintService;

        public SprintController(IApiHelper apiHealper, IJWTService jwtService
            , ILogger<StatusController> logger, ISprintService sprintService)
        {
            _apiHealper = apiHealper;
            _jwtService = jwtService;
            _logger = logger;
            _sprintService = sprintService;
            _sprintService = sprintService;
        }


        [Route("create")]
        [HttpPut]
        public async Task CreateSprint([FromForm] long projectId, [FromForm] string name)
        {

            name = _apiHealper.StringValidator(name);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _sprintService.Create(projectId, name, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new ProjectSprintReturn(res));

                }, Response, _logger);

        }

        [Route("delete")]
        [HttpDelete]
        public async Task DeleteSprint([FromForm] long id)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _sprintService.Delete(id, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResult(res != null));

                }, Response, _logger);

        }

        [Route("add-task-to-sprint")]
        [HttpPost]
        public async Task AddTaskToSprint([FromForm] long sprintId, [FromForm] long taskId)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _sprintService.AddTaskToSprint(sprintId, taskId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResult(res));

                }, Response, _logger);

        }

        [Route("delete-task-from-sprint")]
        [HttpPost]
        public async Task DeleteTaskFromSprint([FromForm] long sprintId, [FromForm] long taskId)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _sprintService.DeleteTaskFromSprint(sprintId, taskId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResult(res));

                }, Response, _logger);

        }


    }
}
