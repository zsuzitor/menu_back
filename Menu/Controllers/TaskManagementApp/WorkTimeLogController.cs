using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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


        public WorkTimeLogController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILogger<ProjectController> logger, IWorkTaskService workTaskService
             )
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = logger;

            _workTaskService = workTaskService;
        }

        [Route("create")]
        [HttpPut]
        public async Task Create([FromForm] long taskId, [FromForm] string text, [FromForm] long minutes)
        {
            text = _apiHealper.StringValidator(text);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _workTaskService.CreateCommentAsync(taskId, text, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new WorkTaskCommentReturn(res));

                }, Response, _logger);

        }
    }
}
