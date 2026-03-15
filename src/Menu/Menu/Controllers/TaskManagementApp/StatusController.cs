using Auth.Models.Auth;
using Common.Models;
using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Host.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services.Interfaces;
using WEB.Common.Models.Helpers.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Menu.Controllers.TaskManagementApp
{

    [Route("api/taskmanagement/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Tags("taskmanagement")]
    public class StatusController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;
        private readonly IProjectService _projectService;
        private readonly IWorkTaskStatusService _workTaskStatusService;

        public StatusController(IProjectService projectService, IApiHelper apiHealper, IJWTService jwtService, ILoggerFactory loggerFactory
            , IWorkTaskStatusService workTaskStatusService)
        {
            _projectService = projectService;
            _apiHealper = apiHealper;
            _jwtService = jwtService;
            _logger = loggerFactory.CreateLogger(Constants.Loggers.MenuApp);
            _workTaskStatusService = workTaskStatusService;
        }


        [Route("get-statuses")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<List<WorkTaskStatusReturn>>> GetStatus(long projectId)
        {
            var userId = User.GetUserId();
            var res = await _workTaskStatusService.GetStatusesAccessAsync(projectId, userId);

            return new JsonResult(res.Select(x => new WorkTaskStatusReturn(x)), GetJsonOptions());

        }

        [Route("create-status")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTaskStatusReturn>> CreateStatus([FromForm] long projectId, [FromForm] string status)
        {
            var userId = User.GetUserId();

            status = _apiHealper.StringValidator(status);
            var res = await _workTaskStatusService.CreateStatusAsync(status, projectId, userId);

            return new JsonResult(new WorkTaskStatusReturn(res), GetJsonOptions());

        }

        [Route("delete-status")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<ProjectSprintReturn>> DeleteStatus([FromForm] long statusId)
        {
            var userId = User.GetUserId();
            var res = await _workTaskStatusService.DeleteStatusAsync(statusId, userId);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("update-status")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateStatus([FromForm] long statusId, [FromForm] string status)
        {
            var userId = User.GetUserId();
            status = _apiHealper.StringValidator(status);
            var res = await _workTaskStatusService.UpdateStatusAsync(statusId, status, userId);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
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
