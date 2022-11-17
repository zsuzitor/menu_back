
using System.Threading.Tasks;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Returns.Types;
using WEB.Common.Models.Helpers.Interfaces;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Common.Models.Return;
using Common.Models.Poco;
using Microsoft.AspNetCore.Http;
using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using Common.Models.Error;

namespace Menu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;
        private readonly IJWTService _jwtService;
        private readonly IUserService _userService;
        protected readonly IErrorService _errorService;



        private readonly ShortUserReturnFactory _shortUserReturnFactory;

        public UsersController(IJWTService jwtService, IApiHelper apiHealper
            , ILogger<UsersController> logger, IUserService userService
            , IErrorService errorService)
        {
            _apiHealper = apiHealper;
            _logger = logger;
            _jwtService = jwtService;
            _userService = userService;
            _shortUserReturnFactory = new ShortUserReturnFactory();
            _errorService = errorService;
        }


        [Route("get-shortest-user-info")]
        [HttpGet]
        public async Task GetShortestUSerInfo()
        {
            //TODO тут сейчас получаем полную версию user и ее мапим на short модель, так лучше не делать, пока ок
            await _apiHealper.DoStandartSomething(
              async () =>
              {
                  var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                  var res = await _userService.GetShortInfoAsync(userInfo.UserId);
                  await _apiHealper.WriteResponseAsync(Response, _shortUserReturnFactory.GetObjectReturn(res));

              }, Response, _logger);

        }

        [Route("change-user-password")]
        [HttpPatch]
        public async Task ChangeUserPassword([FromForm] string oldPassword, [FromForm] string newPassword)
        {
            await _apiHealper.DoStandartSomething(
              async () =>
              {
                  var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                  var res = await _userService.ChangePasswordAsync(userInfo.UserId, oldPassword, newPassword);
                  await _apiHealper.WriteResponseAsync(Response, new BoolResultFactory().GetObjectReturn(new BoolResult(res)));

              }, Response, _logger);

        }

        [Route("change-user-name")]
        [HttpPatch]
        public async Task ChangeUserName([FromForm] string newName)
        {
            await _apiHealper.DoStandartSomething(
              async () =>
              {
                  var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                  var res = await _userService.ChangeNameAsync(userInfo.UserId, newName);
                  await _apiHealper.WriteResponseAsync(Response, new BoolResultFactory().GetObjectReturn(new BoolResult(res)));

              }, Response, _logger);

        }

        [Route("change-user-image")]
        [HttpPatch]
        public async Task ChangeUserImage([FromForm] IFormFile image)
        {
            await _apiHealper.DoStandartSomething(
              async () =>
              {
                  _apiHealper.FileValidator(image, ModelState);
                  _apiHealper.ErrorsFromModelState(ModelState);
                  if (_errorService.HasError())
                  {
                      throw new StopException();
                  }

                  var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                  var res = await _userService.ChangeImageAsync(userInfo.UserId, image);
                  if(image != null && string.IsNullOrEmpty(res))
                  {
                      throw new SomeCustomException(ErrorConsts.SomeError);
                  }

                  await _apiHealper.WriteResponseAsync(Response, new StringResultReturn(res));

              }, Response, _logger);

        }
    }
}
