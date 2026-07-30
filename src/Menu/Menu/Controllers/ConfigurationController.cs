using Auth.Models.Auth;
using BL.Models.Services.Cache;
using BL.Models.Services.Interfaces;
using Common.Models.Exceptions;
using Common.Models.Return;
using Menu.Host.Infrastructure;
using Menu.Host.Models.Returns.Types;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace Menu.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private readonly IConfigurationService _config;
        private readonly IUserService _userService;

        public ConfigurationController(IConfigurationService config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }

        [Route("get")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<ConfigurationReturn>> ClearCache(string configurationKey)
        {
            var userId = User.GetUserId();
            var user = await _userService.GetUserByIdAsync(userId);
            if(user == null || !user.IsAdmin)
            {
                throw new SomeCustomNotAllowedException();
            }

            var res = await _config.GetPublicAsync(configurationKey);

            return new JsonResult(new ConfigurationReturn(res), GetJsonOptions());
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
