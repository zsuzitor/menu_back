
using System;
using System.Threading.Tasks;
using Menu.Models.InputModels.Auth;
using Auth.Models.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WEB.Common.Models.Helpers.Interfaces;
using Common.Models.Error.services.Interfaces;
using Microsoft.Extensions.Logging;
using Common.Models.Exceptions;
using Common.Models.Error;
using Common.Models.Return;
using Menu.Models.Returns.Types;
using Common.Models.Poco;
using System.ComponentModel.DataAnnotations;

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

        private readonly ErrorObjectReturnFactory _errRetrunFactory;
        private readonly TokensReturnFactory _tokensReturnFactory;
        private readonly BoolResultFactory _boolResultFactory;

        public AuthenticateController(IErrorService errorService, IApiHelper apiHealper, IAuthService authSrvice, ILogger<AuthenticateController> logger)
        {
            _authSrvice = authSrvice;
            _apiHealper = apiHealper;
            _errorService = errorService;
            _logger = logger;

            _errRetrunFactory = new ErrorObjectReturnFactory();
            _tokensReturnFactory = new TokensReturnFactory();
            _boolResultFactory = new BoolResultFactory();
        }


        // POST api/<AuthenticateController>/5
        [Route("login")]
        [HttpPost]
        public async Task Login([FromForm] LoginModelApi loginModel)
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   //throw new Exception("test exc");

                   //throw new System.Exception();
                   if (_apiHealper.ErrorsFromModelState(ModelState))
                   {
                       await _apiHealper.WriteResponseAsync(Response, _errRetrunFactory.GetObjectReturn((_errorService.GetErrorsObject())));
                       return;
                   }

                   var tokens = await _authSrvice.LoginAsync(loginModel.GetModel());
                   if (tokens == null)
                   {
                       throw new SomeCustomException(ErrorConsts.SomeError);
                   }

                   _apiHealper.SetUserTokens(Response, tokens);
                   await _apiHealper.WriteResponseAsync(Response, _tokensReturnFactory.GetObjectReturn(tokens));
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
                      await _apiHealper.WriteResponseAsync(Response, _errRetrunFactory.GetObjectReturn(_errorService.GetErrorsObject()));
                      return;
                  }

                  var tokens = await _authSrvice.RegisterAsync(registerModel.GetModel());
                  if (tokens == null)
                  {
                      return;//что то пошло не так TODO
                  }

                  _apiHealper.SetUserTokens(Response, tokens);
                  await _apiHealper.WriteResponseAsync(Response, _tokensReturnFactory.GetObjectReturn(tokens));
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
                  var logout = await _authSrvice.LogOutAsync(accessToken);
                  if (logout)
                  {
                      _apiHealper.ClearUsersTokens(Response);
                  }


                  await _apiHealper.WriteResponseAsync(Response, _boolResultFactory.GetObjectReturn(logout));
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
                  var allTokens = await _authSrvice.RefreshAsync(accessToken, refreshToken);
                  if (allTokens == null)
                  {
                      throw new NotAuthException();
                  }
                  _apiHealper.SetUserTokens(Response, allTokens);
                  await _apiHealper.WriteResponseAsync(Response, _tokensReturnFactory.GetObjectReturn(allTokens));
              }, Response, _logger);

        }


        [Route("SendMessageForgotPassword")]
        [HttpPost]
        public async Task SendMessageForgotPassword([FromForm][EmailAddress] string email)
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   if (_apiHealper.ErrorsFromModelState(ModelState))
                   {
                       await _apiHealper.WriteResponseAsync(Response
                           , _errRetrunFactory.GetObjectReturn((_errorService.GetErrorsObject())));
                       return;
                   }

                   //всегда отдаем true что бы не палить отсутствие пользака в бд
                   _ = await _authSrvice.SendMessageForgotPasswordAsync(email);
                   //if (!res)
                   //{
                   //    throw new SomeCustomException(ErrorConsts.SomeError);
                   //}

                   await _apiHealper.WriteResponseAsync(Response
                       , _boolResultFactory.GetObjectReturn(new BoolResult(true)));
               }, Response, _logger);
        }


        [Route("CheckRecoverPasswordCode")]
        [HttpPost]
        public async Task CheckRecoverPasswordCode([FromForm] string code)
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   code = code?.Trim();
                   var res = await _authSrvice.CheckRecoverPasswordCodeAsync(code);
                   if (!res)
                   {
                       throw new SomeCustomException(ErrorConsts.SomeError);
                   }

                   await _apiHealper.WriteResponseAsync(Response
                       , _boolResultFactory.GetObjectReturn(new BoolResult(res)));
               }, Response, _logger);
        }

        [Route("RecoverPassword")]
        [HttpPost]
        public async Task RecoverPassword([FromForm] string code, [FromForm] string password)
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   var res = await _authSrvice.RecoverPasswordAsync(code, password);
                   if (!res)
                   {
                       throw new SomeCustomException(ErrorConsts.SomeError);
                   }

                   await _apiHealper.WriteResponseAsync(Response
                       , _boolResultFactory.GetObjectReturn(new BoolResult(res)));
               }, Response, _logger);
        }
        


    }

}
