
using System.Threading.Tasks;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Helpers.Interfaces;
using Menu.Models.InputModels.WordsCardsApp;
using Microsoft.AspNetCore.Http;
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

        public WordsCardsController(IJWTService jwtService, IApiHelper apiHealper, ILogger<WordsCardsController> logger, IWordsCardsService wordsCardsService)
        {
            _apiHealper = apiHealper;
            _jwtService = jwtService;
            _logger = logger;
            _wordsCardsService = wordsCardsService;
        }

        [Route("get-all-for-user")]
        [HttpGet]
        public async Task GetAllForUser()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsCardsService.GetAllForUser(userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }

        [Route("create")]
        [HttpPut]
        public async Task Create(WordCardInputModelApi newData)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsCardsService.Create(newData.GetModel(),userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }

        [Route("get-all-for-user")]
        [HttpDelete]
        public async Task Delete(long id)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsCardsService.Delete(id, userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }


        [Route("get-all-for-user")]
        [HttpPatch]
        public async Task Update(WordCardInputModelApi newData)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsCardsService.Update(newData.GetModel(), userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }

        [Route("create-from-file")]
        [HttpPut]
        public async Task CreateFromFile(IFormFile file)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsCardsService.CreateFromFile(file, userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }

    }
}
