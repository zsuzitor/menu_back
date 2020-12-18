
using System.Threading.Tasks;
using Menu.Models.Auth.InputModels;
using Menu.Models.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Menu.Models.Healpers.Interfaces;
using Menu.Models.Error.services.Interfaces;
using Microsoft.Extensions.Logging;
//using NLog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Menu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthService _authSrvice;
        private readonly IApiHealper _apiHealper;
        private readonly IErrorService _errorService;
        private readonly ILogger _logger;

        public AuthenticateController(IErrorService errorService, IApiHealper apiHealper, IAuthService authSrvice, ILogger<AuthenticateController> logger)
        {
            _authSrvice = authSrvice;
            _apiHealper = apiHealper;
            _errorService = errorService;
            _logger = logger;

        }


        // POST api/<AuthenticateController>/5
        [Route("login")]
        [HttpPost]
        public async Task Login([FromForm] LoginModel loginModel)//[FromBody] LoginModel loginModel
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   if (_errorService.ErrorsFromModelState(ModelState))
                   {
                       await _apiHealper.WriteReturnResponseAsync(Response, _errorService.GetErrorsObject());
                       return;
                   }

                   var tokens = await _authSrvice.Login(loginModel);
                   if (tokens == null)
                   {
                       return;
                   }

                   await _apiHealper.WriteReturnResponseAsync(Response, tokens);
               }, Response, _logger);


        }

        // PUT api/<AuthenticateController>
        [Route("register")]
        [HttpPut]
        public async Task Register([FromForm] RegisterModel registerModel)
        {
            await _apiHealper.DoStandartSomething(
              async () =>
              {
                  //RegisterModel registerModel=null;
                  if (_errorService.ErrorsFromModelState(ModelState))
                  {
                      await _apiHealper.WriteReturnResponseAsync(Response, _errorService.GetErrorsObject());
                      return;
                  }

                  var tokens = await _authSrvice.Register(registerModel);
                  if (tokens == null)
                  {
                      return;
                  }

                  await _apiHealper.WriteReturnResponseAsync(Response, tokens);
              }, Response, _logger);

        }

        [Route("logout")]
        [HttpGet]
        public async Task LogOut()
        {
            await _apiHealper.DoStandartSomething(
              async () =>
              {
                  var accessToken = _apiHealper.GetAccessTokenFromRequest(Request);
                  var logout = await _authSrvice.LogOut(accessToken);
                  if (logout)
                  {
                      _apiHealper.ClearUsersTokens(Response);
                  }

                  await _apiHealper.WriteReturnResponseAsync(Response, logout);
              }, Response, _logger);

        }

        [Route("refresh-access-token")]
        [HttpGet]
        public async Task RefreshAccessToken()
        {
            await _apiHealper.DoStandartSomething(
              async () =>
              {
                  var accessToken = _apiHealper.GetAccessTokenFromRequest(Request);
                  var refreshToken = _apiHealper.GetRefreshTokenFromRequest(Request);

                  var allTokens = await _authSrvice.Refresh(accessToken, refreshToken);
                  await _apiHealper.WriteReturnResponseAsync(Response, allTokens);
              }, Response, _logger);

        }


    }


}
