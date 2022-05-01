using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Error.services.Interfaces;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.CodeReviewApp
{
    [Route("api/codereview/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly IErrorService _errorService;
        private readonly ILogger _logger;
        private readonly IProjectService _projectService;


        public ProjectController(
             IJWTService jwtService, IApiHelper apiHealper, IErrorService errorService
            , ILogger<ProjectController> logger, IProjectService projectService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _errorService = errorService;
            _logger = logger;

            _projectService = projectService;
        }

        [Route("get-projects")]
        [HttpGet]
        public async Task GetProjects()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _projectService.GetProjectsByMainAppUserIdAsync(userInfo.UserId);

                    await _apiHealper.WriteResponseAsync(Response, _articleShortReturnFactory.GetObjectReturn(res));

                }, Response, _logger);

        }
    }
}
