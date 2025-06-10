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
    public class SprintController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;
        private readonly IProjectService _projectService;
        private readonly ISprintService _sprintService;

        public SprintController(IProjectService projectService, IApiHelper apiHealper, IJWTService jwtService
            , ILogger<StatusController> logger, ISprintService sprintService)
        {
            _projectService = projectService;
            _apiHealper = apiHealper;
            _jwtService = jwtService;
            _logger = logger;
            _sprintService = sprintService;
            _sprintService = sprintService;
        }


        [Route("create")]
        [HttpPut]
        public async Task CreateSprint([FromForm] long projectId, [FromForm] string name)
        {

            name = _apiHealper.StringValidator(name);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _projectService.CreateStatusAsync(name, projectId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new WorkTaskStatusReturn(res));

                }, Response, _logger);

        }


    }
}
