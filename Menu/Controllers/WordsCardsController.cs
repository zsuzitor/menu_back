
using System.Threading.Tasks;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Helpers.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WordsCardsApp.BL.Services.Interfaces;

namespace Menu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsCardsController : ControllerBase
    {
        private readonly IApiHelper _apiHealper;
        private readonly IJWTService _jwtService;
        private readonly ILogger _logger;

        private readonly IWordsCardsService _wordsCardsService;

        public WordsCardsController(IJWTService jwtService, IApiHelper apiHealper, ILogger<WordsCardsController> logger)
        {
            _apiHealper = apiHealper;
            _jwtService = jwtService;
            _logger = logger;
        }

        [Route("get-all-for-user")]
        [HttpGet]
        public async Task GetAllForUser()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsCardsService.GetAllForUsers(userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }


    }
}
