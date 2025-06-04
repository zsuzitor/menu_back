using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services.Interfaces;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]
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
        public async Task Create([FromForm] long taskId, [FromForm] string text, [FromForm] long minutes, [FromForm] DateTime dayOfLog)
        {
            text = _apiHealper.StringValidator(text);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _workTimeLogService.CreateAsync(new BO.Models.TaskManagementApp.DAL.Domain.WorkTimeLog()
                    {
                        Comment = text,
                        DayOfLog = dayOfLog,
                        TimeMinutes = minutes,
                        WorkTaskId = taskId
                    }, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new WorkTimeLogReturn(res));

                }, Response, _logger);

        }

        [Route("update")]
        [HttpPatch]
        public async Task Update([FromForm] long id, [FromForm] long taskId, [FromForm] string text, [FromForm] long minutes, [FromForm] DateTime dayOfLog)
        {
            text = _apiHealper.StringValidator(text);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _workTimeLogService.EditAsync(new BO.Models.TaskManagementApp.DAL.Domain.WorkTimeLog()
                    {
                        Id = id,
                        Comment = text,
                        DayOfLog = dayOfLog,
                        TimeMinutes = minutes,
                        WorkTaskId = taskId
                    }, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new WorkTimeLogReturn(res));

                }, Response, _logger);

        }

        [Route("delete")]
        [HttpDelete]
        public async Task Delete([FromForm] long id)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _workTimeLogService.DeleteAsync(id, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new WorkTimeLogReturn(res));

                }, Response, _logger);

        }

        [Route("project-time")]
        [HttpGet]
        public async Task GetProjectTime([FromForm] long id, [FromForm] DateTime dateFrom, [FromForm] DateTime dateTo)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _workTimeLogService.GetTimeForProjectAsync(id, dateFrom, dateTo, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res.Select(x=> new WorkTimeLogReturn(x)).ToList());

                }, Response, _logger);

        }

        [Route("task-time")]
        [HttpGet]
        public async Task GetTaskTime([FromForm] long id, [FromForm] DateTime dateFrom, [FromForm] DateTime dateTo)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _workTimeLogService.GetTimeForTaskAsync(id, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res.Select(x => new WorkTimeLogReturn(x)).ToList());

                }, Response, _logger);

        }
    }
}
