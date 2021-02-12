
using System.Threading.Tasks;
using Menu.Models.InputModels.Auth;
using Auth.Models.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Menu.Models.Helpers.Interfaces;
using Common.Models.Error.services.Interfaces;
using Microsoft.Extensions.Logging;
using Common.Models.Exceptions;
using Common.Models.Error;
//using NLog;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Menu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthService _authSrvice;
        private readonly IApiHelper _apiHealper;
        private readonly IErrorService _errorService;
        private readonly ILogger _logger;

        public AuthenticateController(IErrorService errorService, IApiHelper apiHealper, IAuthService authSrvice, ILogger<AuthenticateController> logger)
        {
            _authSrvice = authSrvice;
            _apiHealper = apiHealper;
            _errorService = errorService;
            _logger = logger;

        }


        // POST api/<AuthenticateController>/5
        [Route("login")]
        [HttpPost]
        public async Task Login([FromForm] LoginModelApi loginModel)
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   //throw new System.Exception();
                   if (_apiHealper.ErrorsFromModelState(ModelState))
                   {
                       await _apiHealper.WriteReturnResponseAsync(Response, _errorService.GetErrorsObject());
                       return;
                   }

                   var tokens = await _authSrvice.Login(loginModel.GetModel());
                   if (tokens == null)
                   {
                       throw new SomeCustomException(ErrorConsts.SomeError);
                   }

                   _apiHealper.SetUserTokens(Response, tokens);
                   await _apiHealper.WriteReturnResponseAsync(Response, tokens);
               }, Response, _logger);


        }

        // PUT api/<AuthenticateController>
        [Route("register")]
        [HttpPut]
        public async Task Register([FromForm] RegisterModelApi registerModel)
        {
            await _apiHealper.DoStandartSomething(
              async () =>
              {
                  //RegisterModel registerModel=null;
                  if (_apiHealper.ErrorsFromModelState(ModelState))
                  {
                      await _apiHealper.WriteReturnResponseAsync(Response, _errorService.GetErrorsObject());
                      return;
                  }

                  var tokens = await _authSrvice.Register(registerModel.GetModel());
                  if (tokens == null)
                  {
                      return;//что то пошло не так TODO
                  }

                  _apiHealper.SetUserTokens(Response, tokens);
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
        [HttpPost]
        public async Task RefreshAccessToken()
        {
            await _apiHealper.DoStandartSomething(
              async () =>
              {
                  //throw new SomeCustomException("assadads");
                  var accessToken = _apiHealper.GetAccessTokenFromRequest(Request);
                  var refreshToken = _apiHealper.GetRefreshTokenFromRequest(Request);

                  var allTokens = await _authSrvice.Refresh(accessToken, refreshToken);
                  if (allTokens == null)
                  {
                      throw new NotAuthException();
                  }
                  _apiHealper.SetUserTokens(Response, allTokens);
                  await _apiHealper.WriteReturnResponseAsync(Response, allTokens);
              }, Response, _logger);

        }


        

    }


}
