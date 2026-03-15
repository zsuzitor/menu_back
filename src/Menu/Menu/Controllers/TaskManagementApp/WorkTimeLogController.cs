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
using System;
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
    [ApiController]
    [Produces("application/json")]
    [Tags("taskmanagement")]
    public class WorkTimeLogController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IWorkTaskService _workTaskService;
        private readonly IWorkTimeLogService _workTimeLogService;


        public WorkTimeLogController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILoggerFactory loggerFactory, IWorkTaskService workTaskService
            , IWorkTimeLogService workTimeLogService
             )
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = loggerFactory.CreateLogger(Constants.Loggers.MenuApp);

            _workTaskService = workTaskService;
            _workTimeLogService = workTimeLogService;
        }



        [Route("create")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTimeLogReturn>> Create([FromBody] WorkTimeLogCreateRequest request)
        {
            var userId = User.GetUserId();

            request.Text = _apiHealper.StringValidator(request.Text);
            var res = await _workTimeLogService.CreateAsync(request.Map(), userId);

            return new JsonResult(new WorkTimeLogReturn(res), GetJsonOptions());
        }

        [Route("update")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTimeLogReturn>> Update([FromBody] WorkTimeLogUpdateRequest request)
        {
            var userId = User.GetUserId();
            request.Text = _apiHealper.StringValidator(request.Text);
            var res = await _workTimeLogService.EditAsync(request.Map(), userId);
            return new JsonResult(new WorkTimeLogReturn(res), GetJsonOptions());

        }

        [Route("delete")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> Delete([FromForm] long id)
        {
            var userId = User.GetUserId();
            var res = await _workTimeLogService.DeleteAsync(id, userId);

            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("project-time")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<List<WorkTimeLogReturn>>> GetProjectTime(long id, DateTime dateFrom, DateTime dateTo, long? userId)
        {
            var currentUserId = User.GetUserId();
            var res = await _workTimeLogService.GetTimeForProjectAsync(id, dateFrom, dateTo, currentUserId, userId);
            return new JsonResult(res.Select(x => new WorkTimeLogReturn(x)), GetJsonOptions());
        }

        [Route("task-time")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTimeLogReturn>> GetTaskTime(long taskId)
        {
            var userId = User.GetUserId();
            var res = await _workTimeLogService.GetTimeForTaskAsync(taskId, userId);
            return new JsonResult(res.Select(x => new WorkTimeLogReturn(x)), GetJsonOptions());
        }

        [Route("user-time")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTimeLogReturn>> GetUserTime(long? projectId, DateTime dateFrom, DateTime dateTo, long? userId)
        {
            var currentUserId = User.GetUserId();
            var res = await _workTimeLogService.GetTimeForUserAsync(userId, dateFrom, dateTo, currentUserId);
            return new JsonResult(res.Select(x => new WorkTimeLogReturn(x)), GetJsonOptions());
            //todo projectId

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
