using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.TaskManagementApp.Mappers;
using Menu.Models.TaskManagementApp.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using System.Collections.Generic;
using System.Linq;
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

        [Route("get")]
        [HttpGet]
        public async Task GetSprint(long sprintId)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _sprintService.Get(sprintId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new ProjectSprintReturn(res));

                }, Response, _logger);

        }

        [Route("get-for-project")]
        [HttpGet]
        public async Task GetSprintForProject(long projectId)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _sprintService.GetForProjectWithRights(projectId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res.Select(x => new ProjectSprintReturn(x)));

                }, Response, _logger);

        }

        [Route("get-tasks")]
        [HttpGet]
        public async Task GetSprintTasks(long sprintId)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _sprintService.GetTasks(sprintId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res.Select(x => new WorkTaskReturn(x)));

                }, Response, _logger);

        }

        [Route("create")]
        [HttpPut]
        public async Task CreateSprint([FromBody] AddNewSprintRequest request)
        {

            request.Name = _apiHealper.StringValidator(request.Name);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _sprintService.Create(request.Map(), userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new ProjectSprintReturn(res));

                }, Response, _logger);

        }

        [Route("update")]
        [HttpPatch]
        public async Task UpdateSprint([FromBody] UpdateSprintRequest request)
        {

            request.Name = _apiHealper.StringValidator(request.Name);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _sprintService.Update(request.Map(), userInfo);
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
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultReturn(res != null));

                }, Response, _logger);

        }

        [Route("update-task-sprints")]
        [HttpPost]
        public async Task UpdateTaskSprints([FromBody] UpdateTaskSprints req)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _sprintService.UpdateTaskSprints(req.SprintId, req.TaskId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultReturn(res));

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
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultReturn(res));

                }, Response, _logger);

        }

        [Route("delete-task-from-sprint")]
        [HttpPost]
        public async Task DeleteTaskFromSprint([FromForm] long taskId, [FromForm] long sprintId)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _sprintService.DeleteTaskFromSprint(sprintId, taskId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultReturn(res));

                }, Response, _logger);

        }


    }
}
