
using System.Threading.Tasks;
using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using Common.Models.Poco;
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

        private readonly IErrorService _errorService;
        private readonly IWordsCardsService _wordsCardsService;




        public WordsCardsController(IJWTService jwtService, IApiHelper apiHealper,
            ILogger<WordsCardsController> logger, IWordsCardsService wordsCardsService, IErrorService errorService)
        {
            _apiHealper = apiHealper;
            _jwtService = jwtService;
            _logger = logger;
            _wordsCardsService = wordsCardsService;
            _errorService = errorService;


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
        public async Task Create([FromForm] WordCardInputModelApi newData)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    newData.Validate(_apiHealper.StringValidator, _apiHealper.FileValidator, ModelState);
                    _apiHealper.ErrorsFromModelState(ModelState);
                    if (_errorService.HasError())
                    {
                        throw new StopException();
                    }

                    var res = await _wordsCardsService.Create(newData.GetModel(), userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }

        [Route("delete")]
        [HttpDelete]
        public async Task Delete([FromForm] long id)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsCardsService.Delete(id, userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }


        [Route("update")]
        [HttpPatch]
        public async Task Update([FromForm] WordCardInputModelApi newData)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    newData.Validate(_apiHealper.StringValidator, _apiHealper.FileValidator, ModelState);
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    _apiHealper.ErrorsFromModelState(ModelState);
                    if (_errorService.HasError())
                    {
                        throw new StopException();
                    }

                    var res = await _wordsCardsService.Update(newData.GetModel(), userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }

        [Route("hide")]
        [HttpPatch]
        public async Task Hide([FromForm] long id)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsCardsService.ChangeHideStatus(id, userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, new BoolResult(res));

                }, Response, _logger);

        }

        [Route("create-from-file")]
        [HttpPut]
        public async Task CreateFromFile([FromForm] IFormFile file)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsCardsService.CreateFromFile(file, userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }

        [Route("donwload-all-words-file")]
        [HttpPut]
        public async Task DownloadAllWordsFile()
        {//TODO
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    //var res = await _wordsCardsService.CreateFromFile(file, userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, false);

                }, Response, _logger);

        }

    }
}
