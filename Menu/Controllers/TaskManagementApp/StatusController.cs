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

namespace Menu.Controllers.TaskManagementApp
{

    [Route("api/taskmanagement/[controller]")]
    public class StatusController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;
        private readonly IProjectService _projectService;
        private readonly IWorkTaskStatusService _workTaskStatusService;

        public StatusController(IProjectService projectService, IApiHelper apiHealper, IJWTService jwtService, ILogger<StatusController> logger
            , IWorkTaskStatusService workTaskStatusService)
        {
            _projectService = projectService;
            _apiHealper = apiHealper;
            _jwtService = jwtService;
            _logger = logger;
            _workTaskStatusService = workTaskStatusService;
        }


        [Route("get-statuses")]
        [HttpGet]
        public async Task GetStatus(long projectId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _workTaskStatusService.GetStatusesAccessAsync(projectId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res.Select(x => new WorkTaskStatusReturn(x)));

                }, Response, _logger);

        }

        [Route("create-status")]
        [HttpPut]
        public async Task CreateStatus([FromForm] long projectId, [FromForm] string status)
        {

            status = _apiHealper.StringValidator(status);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _workTaskStatusService.CreateStatusAsync(status, projectId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new WorkTaskStatusReturn(res));

                }, Response, _logger);

        }

        [Route("delete-status")]
        [HttpDelete]
        public async Task DeleteStatus([FromForm] long statusId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _workTaskStatusService.DeleteStatusAsync(statusId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                         , new
                         {
                             result = res != null,
                         });

                }, Response, _logger);

        }

        [Route("update-status")]
        [HttpPatch]
        public async Task UpdateStatus([FromForm] long statusId, [FromForm] string status)
        {
            status = _apiHealper.StringValidator(status);
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _workTaskStatusService.UpdateStatusAsync(statusId, status, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                         , new
                         {
                             result = res != null,
                         });

                }, Response, _logger);

        }
    }
}
