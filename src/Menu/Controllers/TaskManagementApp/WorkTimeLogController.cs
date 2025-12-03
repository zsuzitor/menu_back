using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
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
    public class WorkTimeLogController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IWorkTaskService _workTaskService;
        private readonly IWorkTimeLogService _workTimeLogService;


        public WorkTimeLogController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILogger<ProjectController> logger, IWorkTaskService workTaskService
            , IWorkTimeLogService workTimeLogService
             )
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = logger;

            _workTaskService = workTaskService;
            _workTimeLogService = workTimeLogService;
        }



        [Route("create")]
        [HttpPut]
        public async Task<ActionResult<WorkTimeLogReturn>> Create([FromBody] WorkTimeLogCreateRequest request)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

            request.Text = _apiHealper.StringValidator(request.Text);
            var res = await _workTimeLogService.CreateAsync(request.Map(), userInfo);

            return new JsonResult(new WorkTimeLogReturn(res), GetJsonOptions());
        }

        [Route("update")]
        [HttpPatch]
        public async Task<ActionResult<WorkTimeLogReturn>> Update([FromBody] WorkTimeLogUpdateRequest request)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            request.Text = _apiHealper.StringValidator(request.Text);
            var res = await _workTimeLogService.EditAsync(request.Map(), userInfo);
            return new JsonResult(new WorkTimeLogReturn(res), GetJsonOptions());

        }

        [Route("delete")]
        [HttpDelete]
        public async Task<ActionResult<BoolResultNewReturn>> Delete([FromForm] long id)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _workTimeLogService.DeleteAsync(id, userInfo);

            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("project-time")]
        [HttpGet]
        public async Task<ActionResult<List<WorkTimeLogReturn>>> GetProjectTime(long id, DateTime dateFrom, DateTime dateTo, long? userId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _workTimeLogService.GetTimeForProjectAsync(id, dateFrom, dateTo, userInfo, userId);
            return new JsonResult(res.Select(x => new WorkTimeLogReturn(x)), GetJsonOptions());
        }

        [Route("task-time")]
        [HttpGet]
        public async Task<ActionResult<WorkTimeLogReturn>> GetTaskTime(long taskId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _workTimeLogService.GetTimeForTaskAsync(taskId, userInfo);
            return new JsonResult(res.Select(x => new WorkTimeLogReturn(x)), GetJsonOptions());
        }

        [Route("user-time")]
        [HttpGet]
        public async Task<ActionResult<WorkTimeLogReturn>> GetUserTime(long? projectId, DateTime dateFrom, DateTime dateTo, long? userId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _workTimeLogService.GetTimeForUserAsync(userId, dateFrom, dateTo, userInfo);
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
