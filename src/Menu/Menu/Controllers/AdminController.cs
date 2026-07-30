using Auth.Models.Auth;
using BL.Models.Services.Interfaces;
using Common.Models.Exceptions;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menu.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        protected readonly ICacheService _cacheService;
        private readonly IUserService _userService;

        public AdminController(ICacheService cacheService, IUserService userService)
        {
            _cacheService = cacheService;
            _userService = userService;
        }


        [Route("clear-cache")]
        [HttpGet]
        public async Task ClearCache(string cacheKey)
        {
            var userId = User.GetUserId();

            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null || !user.IsAdmin)
            {
                throw new SomeCustomNotAllowedException();
            }

            await _cacheService.RemoveAsync(cacheKey);
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
