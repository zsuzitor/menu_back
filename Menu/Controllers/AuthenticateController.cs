
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

        public AuthenticateController(IErrorService errorService, IApiHealper apiHealper, IAuthService authSrvice,ILogger<AuthenticateController> logger)
        {
            _authSrvice = authSrvice;
            _apiHealper = apiHealper;
            _errorService = errorService;
            _logger = logger;


            int g_ = 10;
        }


        // POST api/<AuthenticateController>/5
        [Route("Login")]
        [HttpPost]
        public async Task Login([FromForm] LoginModel loginModel)
        {
            _logger.LogDebug("TEST2_LOGIN");
            if (_errorService.ErrorsFromModelState(ModelState))
            {
                await _apiHealper.WriteResponseAsync(Response, _errorService.GetErrorsObject());
                return;
            }

            
            var tokens = await _authSrvice.Login(loginModel);
            if (tokens == null)
            {
                return;
            }

            await _apiHealper.WriteResponseAsync(Response, tokens);

        }

        // PUT api/<AuthenticateController>
        [Route("Register")]
        [HttpPut]
        public async Task Register([FromForm] RegisterModel registerModel)
        {
            //RegisterModel registerModel=null;
            if (_errorService.ErrorsFromModelState(ModelState))
            {
                await _apiHealper.WriteResponseAsync(Response, _errorService.GetErrorsObject());
                return;
            }

            var tokens = await _authSrvice.Register(registerModel);
            if (tokens == null)
            {
                return;
            }

            await _apiHealper.WriteResponseAsync(Response, tokens);
        }

        [Route("LogOut")]
        [HttpGet]
        public async Task LogOut()
        {
            var accessToken = _apiHealper.GetAccessTokenFromRequest(Request);
            var logout = await _authSrvice.LogOut(accessToken);
            if (logout)
            {
                _apiHealper.ClearUsersTokens(Response);
            }

            await _apiHealper.WriteResponseAsync(Response, logout);
        }

        [Route("RefreshAccessToken")]
        [HttpGet]
        public async Task RefreshAccessToken()
        {
            var accessToken = _apiHealper.GetAccessTokenFromRequest(Request);
            var refreshToken = _apiHealper.GetRefreshTokenFromRequest(Request);

            var allTokens = await _authSrvice.Refresh(accessToken, refreshToken);
            await _apiHealper.WriteResponseAsync(Response, allTokens);
        }


    }
}
