using Common.Models.Error.services.Interfaces;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.VaultApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaultSecretController : ControllerBase
    {
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;


        private readonly IJWTService _jwtService;

        private readonly IErrorService _errorService;


        public VaultSecretController(IApiHelper apiHealper,
            ILogger<VaultController> logger, IJWTService jwtService,
        IErrorService errorService
        )
        {
            _apiHealper = apiHealper;
            _logger = logger;
            _errorService = errorService;
            _jwtService = jwtService;
        }

        [Route("get-users-in-room")]
        [HttpGet]
        public async Task GetUsersIsRoom(string roomname, string userConnectionId)
        {

        }
    }
}
