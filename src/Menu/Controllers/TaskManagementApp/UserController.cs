using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;
using TaskManagementApp.Models;
using Common.Models.Return;
using Menu.Models.TaskManagementApp.Requests;
using Nest;
using System.Text.Json;

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]
    [ApiController]
    [Produces("application/json")]

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
             IProjectUserService projectUserService, IUserService mainAppUserService)
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
        public async Task<ActionResult<ProjectUserReturn>> AddNewUser([FromBody] AddNewUserRequest request)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
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
                , request.MainAppUserEmail, userIdForAdd, userInfo);


            return new JsonResult(new ProjectUserReturn(res ), GetJsonOptions());

        }

        [Route("change-user")]
        [HttpPatch]
        public async Task<ActionResult<BoolResultReturn>> ChangeUser([FromBody] ChangeUserRequest request)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

            request.Name = _apiHealper.StringValidator(request.Name);
            request.Email = _apiHealper.StringValidator(request.Email);
            var res = await _projectUserService.ChangeAsync(request.UserId, request.Name, request.Email, request.IsAdmin, request.Deactivated, userInfo);

            return new JsonResult(new BoolResultReturn(res != null), GetJsonOptions());
        }

        [Route("delete-user")]
        [HttpDelete]
        public async Task<ActionResult<BoolResultReturn>> DeleteUser([FromForm] long userId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _projectUserService.DeleteAsync(userId, userInfo);
            return new JsonResult(new BoolResultReturn(res != null), GetJsonOptions());

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
