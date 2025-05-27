using CodeReviewApp.Models.Returns;
using CodeReviewApp.Models.Services.Interfaces;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Menu.Controllers.CodeReviewApp
{

    [Route("api/codereview/[controller]")]
    public class StatusController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;
        private readonly IProjectService _projectService;

        public StatusController(IProjectService projectService, IApiHelper apiHealper, IJWTService jwtService, ILogger<StatusController> logger)
        {
            _projectService = projectService;
            _apiHealper = apiHealper;
            _jwtService = jwtService;
            _logger = logger;
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

                    var res = await _projectService.GetStatusesAccessAsync(projectId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res.Select(x => new TaskReviewStatusReturn(x)));

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

                    var res = await _projectService.CreateStatusAsync(status, projectId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new TaskReviewStatusReturn(res));

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

                    var res = await _projectService.DeleteStatusAsync(statusId, userInfo);
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

                    var res = await _projectService.UpdateStatusAsync(statusId, status, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                         , new
                         {
                             result = res != null,
                         });

                }, Response, _logger);

        }
    }
}
