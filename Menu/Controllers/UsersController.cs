
using System.Threading.Tasks;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Returns.Types;
using WEB.Common.Models.Helpers.Interfaces;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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


        private readonly ShortUserReturnFactory _shortUserReturnFactory;

        public UsersController(IJWTService jwtService, IApiHelper apiHealper, ILogger<UsersController> logger, IUserService userService)
        {
            _apiHealper = apiHealper;
            _logger = logger;
            _jwtService = jwtService;
            _userService = userService;
            _shortUserReturnFactory = new ShortUserReturnFactory();
        }


        [Route("get-shortest-user-info")]
        [HttpGet]
        public async Task GetShortestUSerInfo()
        {
            //TODO тут сейчас получаем полную версию user и ее мапим на short модель, так лучше не делать, пока ок
            await _apiHealper.DoStandartSomething(
              async () =>
              {
                  await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _userService.GetShortInfoAsync(userInfo.UserId);

                    await _apiHealper.WriteResponseAsync(Response, _shortUserReturnFactory.GetObjectReturn(res));

                }, Response, _logger);
              }, Response, _logger);

        }
    }
}
