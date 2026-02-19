using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services.Interfaces;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;
using static System.Net.Mime.MediaTypeNames;
using Common.Models.Return;
using System.Text.Json;
using System.Collections.Generic;
using Common.Models;

namespace Menu.Controllers.TaskManagementApp
{

    [Route("api/taskmanagement/[controller]")]
    [ApiController]
    [Produces("application/json")]
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
        public async Task<ActionResult<List<WorkTaskStatusReturn>>> GetStatus(long projectId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _workTaskStatusService.GetStatusesAccessAsync(projectId, userInfo);

            return new JsonResult(res.Select(x => new WorkTaskStatusReturn(x)), GetJsonOptions());

        }

        [Route("create-status")]
        [HttpPut]
        public async Task<ActionResult<WorkTaskStatusReturn>> CreateStatus([FromForm] long projectId, [FromForm] string status)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

            status = _apiHealper.StringValidator(status);
            var res = await _workTaskStatusService.CreateStatusAsync(status, projectId, userInfo);

            return new JsonResult(new WorkTaskStatusReturn(res), GetJsonOptions());

        }

        [Route("delete-status")]
        [HttpDelete]
        public async Task<ActionResult<ProjectSprintReturn>> DeleteStatus([FromForm] long statusId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _workTaskStatusService.DeleteStatusAsync(statusId, userInfo);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("update-status")]
        [HttpPatch]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateStatus([FromForm] long statusId, [FromForm] string status)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            status = _apiHealper.StringValidator(status);
            var res = await _workTaskStatusService.UpdateStatusAsync(statusId, status, userInfo);
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
