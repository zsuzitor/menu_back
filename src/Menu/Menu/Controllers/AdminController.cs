using Auth.Models.Auth;
using BL.Models.Services.Interfaces;
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

        public AdminController(ICacheService cacheService)
        {
            _cacheService = cacheService;
        }


        [Route("clear-cache")]
        [HttpGet]
        public async Task ClearCache(string cacheKey)
        {
            var userId = User.GetUserId();
            //todo проверить как то права
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
