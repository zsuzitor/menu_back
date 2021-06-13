using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using Common.Models.Poco;
using jwtLib.JWTAuth.Interfaces;
using WEB.Common.Models.Helpers.Interfaces;
using Menu.Models.InputModels.WordsCardsApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WordsCardsApp.BL.Services.Interfaces;

namespace Menu.Controllers.WordsCardsApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class WordsListController : ControllerBase
    {
        private readonly IApiHelper _apiHealper;
        private readonly IJWTService _jwtService;
        private readonly ILogger _logger;
        private readonly IWordsListService _wordsListService;
        private readonly IErrorService _errorService;

        public WordsListController(IJWTService jwtService, IApiHelper apiHealper,
            ILogger<WordsListController> logger, IErrorService errorService, IWordsListService wordsListService)
        {
            _apiHealper = apiHealper;
            _jwtService = jwtService;
            _logger = logger;
            _errorService = errorService;
            _wordsListService = wordsListService;

        }


        [Route("get-all-for-user")]
        [HttpGet]
        public async Task GetAllForUser()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsListService.GetAllForUser(userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }

        [Route("create")]
        [HttpPut]
        public async Task Create([FromForm] WordCardListInputModelApi newData)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    newData.Validate(_apiHealper.StringValidator, ModelState);
                    _apiHealper.ErrorsFromModelState(ModelState);
                    if (_errorService.HasError())
                    {
                        throw new StopException();
                    }

                    var res = await _wordsListService.Create(newData.GetModel(), userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }


        [Route("remove-from-list")]
        [HttpDelete]
        public async Task RemoveFromList([FromForm] long card_id, [FromForm] long list_id)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsListService.RemoveFromList(card_id, list_id, userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, new BoolResult(res != null));

                }, Response, _logger);

        }

        [Route("add-to-list")]
        [HttpPut]
        public async Task AddToList([FromForm] long card_id, [FromForm] long list_id)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsListService.AddToList(card_id, list_id, userInfo);

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

                    var res = await _wordsListService.Delete(id, userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }


        [Route("update")]
        [HttpPatch]
        public async Task Update([FromForm] WordCardListInputModelApi newData)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    newData.Validate(_apiHealper.StringValidator, ModelState);
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    _apiHealper.ErrorsFromModelState(ModelState);
                    if (_errorService.HasError())
                    {
                        throw new StopException();
                    }

                    var res = await _wordsListService.Update(newData.GetModel(), userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }



    }
}
