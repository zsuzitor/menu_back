using Menu.Models.Helpers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Menu.Controllers.PlanitPoker
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanitPokerController : ControllerBase
    {

        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        public PlanitPokerController(IApiHelper apiHealper, ILogger<PlanitPokerController> logger)
        {
            _apiHealper = apiHealper;
            _logger = logger;
        }

        [Route("create-room")]
        [HttpGet]
        public async Task CreateRoom([FromForm] string roomname)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {


                    //await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }

        [Route("get-my-room")]
        [HttpGet]
        public async Task GetMyRoom([FromForm] string connectionId)//можно достать на фронте
        {
            //todo наверное не нужно
        }
    }
}
