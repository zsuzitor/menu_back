
using System.Linq;
using System.Threading.Tasks;
using Menu.Models.Auth.InputModels;
using Menu.Models.Auth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using jwtLib.JWTAuth.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;
using Menu.Models.Healpers.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Menu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthService _authSrvice;
        private readonly IApiHealper _apiHealper;

        public AuthenticateController(IAuthService authSrvice, IApiHealper apiHealper)
        {
            _authSrvice = authSrvice;
            _apiHealper = apiHealper;
        }


        // POST api/<AuthenticateController>/5
        [HttpPost]
        public async Task Login([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var errors = ModelState.ToList();
                await _apiHealper.WriteResponse(Response, errors);
                return;
            }

            var tokens = await _authSrvice.Login(loginModel);
            if (tokens == null)
            {
                return;
            }

            await _apiHealper.WriteResponse(Response, tokens);

        }

        // PUT api/<AuthenticateController>
        [HttpPut]
        public async Task Register([FromBody] RegisterModel registerModel)
        {

            if (ModelState.IsValid)
            {
                var errors = ModelState.ToList();
                await _apiHealper.WriteResponse(Response, errors);
                return;
            }

            var tokens = await _authSrvice.Register(registerModel);
            if (tokens == null)
            {
                return;
            }

            await _apiHealper.WriteResponse(Response, tokens);
        }


        [HttpGet]
        public Task LogOut()
        {
        }


        [HttpGet]
        public Task RefreshAccessToken()
        {
        }


    }
}
