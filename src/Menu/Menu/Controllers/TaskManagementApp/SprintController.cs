using Common.Models;
using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.TaskManagementApp.Mappers;
using Menu.Models.TaskManagementApp.Requests;
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
        public async Task<ActionResult<ProjectSprintReturn>> GetSprint(long sprintId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _sprintService.Get(sprintId, userInfo);
            return new JsonResult(new ProjectSprintReturn(res), GetJsonOptions());

        }

        [Route("get-for-project")]
        [HttpGet]
        [ProducesResponseType(typeof(List<ProjectSprintReturn>), 200)]
        public async Task<ActionResult<List<ProjectSprintReturn>>> GetSprintForProject(long projectId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _sprintService.GetForProjectWithRights(projectId, userInfo);
            return new JsonResult(res.Select(x => new ProjectSprintReturn(x)).ToList(), GetJsonOptions());

        }

        [Route("get-tasks")]
        [HttpGet]
        [ProducesResponseType(typeof(WorkTaskReturn), 200)]
        public async Task<ActionResult<WorkTaskReturn>> GetSprintTasks(long sprintId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _sprintService.GetTasks(sprintId, userInfo);
            return new JsonResult(res.Select(x => new WorkTaskReturn(x)), GetJsonOptions());
        }

        [Route("create")]
        [HttpPut]
        [ProducesResponseType(typeof(ProjectSprintReturn), 200)]
        public async Task<ActionResult<ProjectSprintReturn>> CreateSprint([FromBody] AddNewSprintRequest request)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            request.Name = _apiHealper.StringValidator(request.Name);
            var res = await _sprintService.Create(request.Map(), userInfo);
            return new JsonResult(new ProjectSprintReturn(res), GetJsonOptions());

        }

        [Route("update")]
        [HttpPatch]
        [ProducesResponseType(typeof(ProjectSprintReturn), 200)]
        public async Task<ActionResult<ProjectSprintReturn>> UpdateSprint([FromBody] UpdateSprintRequest request)
        {

            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            request.Name = _apiHealper.StringValidator(request.Name);
            var res = await _sprintService.Update(request.Map(), userInfo);
            return new JsonResult(new ProjectSprintReturn(res), GetJsonOptions());


        }



        [Route("delete")]
        [HttpDelete]
        [ProducesResponseType(typeof(BoolResultNewReturn), 200)]
        public async Task<ActionResult<ProjectSprintReturn>> DeleteSprint([FromForm] long id)
        {

            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _sprintService.Delete(id, userInfo);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());

        }

        [Route("update-task-sprints")]
        [HttpPost]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateTaskSprints([FromBody] UpdateTaskSprints req)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _sprintService.UpdateTaskSprints(req.SprintId, req.TaskId, userInfo);
            return new JsonResult(new BoolResultNewReturn(res), GetJsonOptions());

        }

        [Route("add-task-to-sprint")]
        [HttpPost]
        public async Task<ActionResult<BoolResultNewReturn>> AddTaskToSprint([FromForm] long sprintId, [FromForm] long taskId)
        {

            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

            var res = await _sprintService.AddTaskToSprint(sprintId, taskId, userInfo);
            return new JsonResult(new BoolResultNewReturn(res), GetJsonOptions());

        }

        [Route("delete-task-from-sprint")]
        [HttpPost]
        public async Task<ActionResult<BoolResultNewReturn>> DeleteTaskFromSprint([FromForm] long taskId, [FromForm] long sprintId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _sprintService.DeleteTaskFromSprint(sprintId, taskId, userInfo);
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
