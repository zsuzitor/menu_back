using Auth.Models.Auth;
using Common.Models;
using Common.Models.Exceptions;
using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Host.Infrastructure;
using Menu.Models.Services.Interfaces;
using Menu.Models.TaskManagementApp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagementApp.Models;
using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services.Interfaces;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Tags("taskmanagement")]

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
            , ILoggerFactory loggerFactory, IProjectService projectService,
             IProjectUserService projectUserService, IUserService mainAppUserService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = loggerFactory.CreateLogger(Constants.Loggers.MenuApp);

            _projectService = projectService;
            _projectUserService = projectUserService;
            _mainAppUserService = mainAppUserService;
        }


        [Route("add-new-user")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<ProjectUserReturn>> AddNewUser([FromBody] AddNewUserRequest request)
        {
            var userId = User.GetUserId();
            request.UserName = _apiHealper.StringValidator(request.UserName);
            request.MainAppUserEmail = _apiHealper.StringValidator(request.MainAppUserEmail);

            long? userIdForAdd = null;
            if (!string.IsNullOrWhiteSpace(request.MainAppUserEmail))
            {
                userIdForAdd = await _mainAppUserService.GetIdByEmailAsync(request.MainAppUserEmail);
                if (userIdForAdd == null)
                {
                    throw new SomeCustomException(Consts.ErrorConsts.UserInMainAppNotFound);
                }
            }

            var res = await _projectService.CreateUserAsync(request.ProjectId, request.UserName
                , request.MainAppUserEmail, userIdForAdd, userId);


            return new JsonResult(new ProjectUserReturn(res ), GetJsonOptions());

        }

        [Route("change-user")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> ChangeUser([FromBody] ChangeUserRequest request)
        {
            var userId = User.GetUserId();

            request.Name = _apiHealper.StringValidator(request.Name);
            request.Email = _apiHealper.StringValidator(request.Email);
            var res = await _projectUserService.ChangeAsync(request.UserId, request.Name, request.Email, request.IsAdmin, request.Deactivated, userId);

            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
        }

        [Route("delete-user")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> DeleteUser([FromForm] long userId)
        {
            var currentUserId = User.GetUserId();
            var res = await _projectUserService.DeleteAsync(userId, currentUserId);
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
