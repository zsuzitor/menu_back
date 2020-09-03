
using System.Linq;
using System.Threading.Tasks;
using Menu.Models.Auth.InputModels;
using Menu.Models.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Menu.Models.Healpers.Interfaces;
using Menu.Models.Error.services.Interfaces;
using jwtLib.JWTAuth.Interfaces;

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

        public AuthenticateController(IErrorService errorService, IApiHealper apiHealper, IAuthService authSrvice)
        {
            _authSrvice = authSrvice;
            _apiHealper = apiHealper;
            _errorService = errorService;

            int g_ = 10;
        }


        // POST api/<AuthenticateController>/5
        [Route("Login")]
        [HttpPost]
        public async Task Login([FromForm] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToList();//TODO докинуть в _errorService
                await _apiHealper.WriteResponseAsync(Response, errors);
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
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToList();
                await _apiHealper.WriteResponseAsync(Response, errors);
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
