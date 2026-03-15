using Auth.Models.Auth;
using Common.Models;
using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Host.Infrastructure;
using Menu.Models.TaskManagementApp.Mappers;
using Menu.Models.TaskManagementApp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services.Interfaces;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Tags("taskmanagement")]
    public class SprintController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;
        private readonly ISprintService _sprintService;

        public SprintController(IApiHelper apiHealper, IJWTService jwtService
            , ILoggerFactory loggerFactory, ISprintService sprintService)
        {
            _apiHealper = apiHealper;
            _jwtService = jwtService;
            _logger = loggerFactory.CreateLogger(Constants.Loggers.MenuApp);
            _sprintService = sprintService;
            _sprintService = sprintService;
        }

        [Route("get")]
        [HttpGet]
        [ProducesResponseType(typeof(ProjectSprintReturn), 200)]
        [CustomAuthorize]
        public async Task<ActionResult<ProjectSprintReturn>> GetSprint(long sprintId)
        {
            var userId = User.GetUserId();
            var res = await _sprintService.Get(sprintId, userId);
            return new JsonResult(new ProjectSprintReturn(res), GetJsonOptions());

        }

        [Route("get-for-project")]
        [HttpGet]
        [CustomAuthorize]
        [ProducesResponseType(typeof(List<ProjectSprintReturn>), 200)]
        public async Task<ActionResult<List<ProjectSprintReturn>>> GetSprintForProject(long projectId)
        {
            var userId = User.GetUserId();
            var res = await _sprintService.GetForProjectWithRights(projectId, userId);
            return new JsonResult(res.Select(x => new ProjectSprintReturn(x)).ToList(), GetJsonOptions());

        }

        [Route("get-tasks")]
        [HttpGet]
        [CustomAuthorize]
        [ProducesResponseType(typeof(WorkTaskReturn), 200)]
        public async Task<ActionResult<WorkTaskReturn>> GetSprintTasks(long sprintId)
        {
            var userId = User.GetUserId();
            var res = await _sprintService.GetTasks(sprintId, userId);
            return new JsonResult(res.Select(x => new WorkTaskReturn(x)), GetJsonOptions());
        }

        [Route("create")]
        [HttpPut]
        [CustomAuthorize]
        [ProducesResponseType(typeof(ProjectSprintReturn), 200)]
        public async Task<ActionResult<ProjectSprintReturn>> CreateSprint([FromBody] AddNewSprintRequest request)
        {
            var userId = User.GetUserId();
            request.Name = _apiHealper.StringValidator(request.Name);
            var res = await _sprintService.Create(request.Map(), userId);
            return new JsonResult(new ProjectSprintReturn(res), GetJsonOptions());

        }

        [Route("update")]
        [HttpPatch]
        [CustomAuthorize]
        [ProducesResponseType(typeof(ProjectSprintReturn), 200)]
        public async Task<ActionResult<ProjectSprintReturn>> UpdateSprint([FromBody] UpdateSprintRequest request)
        {
            var userId = User.GetUserId();
            request.Name = _apiHealper.StringValidator(request.Name);
            var res = await _sprintService.Update(request.Map(), userId);
            return new JsonResult(new ProjectSprintReturn(res), GetJsonOptions());


        }



        [Route("delete")]
        [HttpDelete]
        [CustomAuthorize]
        [ProducesResponseType(typeof(BoolResultNewReturn), 200)]
        public async Task<ActionResult<ProjectSprintReturn>> DeleteSprint([FromForm] long id)
        {
            var userId = User.GetUserId();
            var res = await _sprintService.Delete(id, userId);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());

        }

        [Route("update-task-sprints")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateTaskSprints([FromBody] UpdateTaskSprints req)
        {
            var userId = User.GetUserId();
            var res = await _sprintService.UpdateTaskSprints(req.SprintId, req.TaskId, userId);
            return new JsonResult(new BoolResultNewReturn(res), GetJsonOptions());

        }

        [Route("add-task-to-sprint")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> AddTaskToSprint([FromForm] long sprintId, [FromForm] long taskId)
        {
            var userId = User.GetUserId();

            var res = await _sprintService.AddTaskToSprint(sprintId, taskId, userId);
            return new JsonResult(new BoolResultNewReturn(res), GetJsonOptions());

        }

        [Route("delete-task-from-sprint")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> DeleteTaskFromSprint([FromForm] long taskId, [FromForm] long sprintId)
        {
            var userId = User.GetUserId();
            var res = await _sprintService.DeleteTaskFromSprint(sprintId, taskId, userId);
            return new JsonResult(new BoolResultNewReturn(res), GetJsonOptions());

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
