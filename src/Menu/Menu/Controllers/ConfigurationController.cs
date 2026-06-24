using Auth.Models.Auth;
using BL.Models.Services.Cache;
using BL.Models.Services.Interfaces;
using Common.Models.Return;
using Menu.Host.Models.Returns.Types;
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

        public ConfigurationController(IConfigurationService config)
        {
            _config = config;
        }

        [Route("get")]
        [HttpGet]
        public async Task<ActionResult<ConfigurationReturn>> ClearCache(string configurationKey)
        {
            var userId = User.GetUserId();
            //todo проверить как то права
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
