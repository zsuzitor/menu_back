using CodeReviewApp.Models.Returns;
using CodeReviewApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;
using CodeReviewApp.Models;

namespace Menu.Controllers.CodeReviewApp
{
    [Route("api/codereview/[controller]")]

    public class UserController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IProjectService _projectService;
        private readonly IProjectUserService _projectUserService;
        private readonly IUserService _mainAppUserService;


        public UserController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILogger<ProjectController> logger, IProjectService projectService,
             IProjectUserService projectUserService, ITaskReviewService taskReviewService,
             ITaskReviewCommentService taskReviewCommentService, IUserService mainAppUserService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = logger;

            _projectService = projectService;
            _projectUserService = projectUserService;
            _mainAppUserService = mainAppUserService;
        }


        [Route("add-new-user")]
        [HttpPut]
        public async Task AddNewUser([FromForm] string userName, [FromForm] string mainAppUserEmail, [FromForm] long projectId)
        {
            userName = _apiHealper.StringValidator(userName);
            mainAppUserEmail = _apiHealper.StringValidator(mainAppUserEmail);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    long? userIdForAdd = null;
                    if (!string.IsNullOrWhiteSpace(mainAppUserEmail))
                    {
                        userIdForAdd = await _mainAppUserService.GetIdByEmailAsync(mainAppUserEmail);
                        if(userIdForAdd == null)
                        {
                            throw new SomeCustomException(Consts.CodeReviewErrorConsts.UserInMainAppNotFound);
                        }
                    }

                    var res = await _projectService.CreateUserAsync(projectId, userName
                        , mainAppUserEmail, userIdForAdd, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new ProjectUserReturn(res));

                }, Response, _logger);

        }

        [Route("change-user")]
        [HttpPatch]
        public async Task ChangeUser([FromForm] long userId, [FromForm] string name
            , [FromForm] string email, [FromForm] bool deactivated, [FromForm] bool isAdmin = false)
        {
            name = _apiHealper.StringValidator(name);
            email = _apiHealper.StringValidator(email);


            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectUserService.ChangeAsync(userId, name, email, isAdmin, deactivated, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res != null,
                        });

                }, Response, _logger);

        }

        [Route("delete-user")]
        [HttpDelete]
        public async Task DeleteUser([FromForm] long userId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _projectUserService.DeleteAsync(userId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res != null,
                        });

                }, Response, _logger);

        }
    }
}
