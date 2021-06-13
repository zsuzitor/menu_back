
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using Common.Models.Poco;
using jwtLib.JWTAuth.Interfaces;
using WEB.Common.Models.Helpers.Interfaces;
using Menu.Models.InputModels.WordsCardsApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WordsCardsApp.BL.Services.Interfaces;

namespace Menu.Controllers.WordsCardsApp
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

                    var res = await _wordsCardsService.GetAllForUserForView(userInfo);

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

        [Route("create-list")]
        [HttpPut]
        public async Task CreateList([FromForm] List<WordCardInputModelApi> newData)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    newData.ForEach(x => x.Validate(_apiHealper.StringValidator, _apiHealper.FileValidator, ModelState));
                    _apiHealper.ErrorsFromModelState(ModelState);
                    if (_errorService.HasError())
                    {
                        throw new StopException();
                    }

                    var res = await _wordsCardsService.Create(newData.Select(x => x.GetModel()).ToList(), userInfo);
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

        [Route("download-all-words-file")]
        [HttpGet]
        public async Task DownloadAllWordsFile()//FileStreamResult
        {//TODO
            // MemoryStream mr = new MemoryStream();
            // TextWriter tw = new StreamWriter(mr);


            //tw.WriteLine("111;");
            //tw.WriteLine("222;");

            //tw.Flush();
            //mr.Position = 0; ;
            ////new FileInfo();
            //var file = File(mr, "application/force-download", "myFile.txt");
            //Response.ContentType = "application/force-download";
            //Response.Headers.Add("content-disposition", string.Format("inline;FileName=\"{0}\"", "myFile.csv"));
            //await Response.Body.WriteAsync(mr.ToArray(),0, (int)mr.Length);
            //return await Task.FromResult(file);


            //return;




            await _apiHealper.DoStandartSomething(
                async () =>
                {

                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _wordsCardsService.GetAllRecordsStringForSave(userInfo);
                    MemoryStream mr = new MemoryStream();
                    TextWriter tw = new StreamWriter(mr);
                    res.ForEach(x => tw.WriteLine(x));
                    tw.Flush();
                    await _apiHealper.SendFile(mr, Response, "myWords.csv");//TODO проверить мб стримы выше можно закрыть
                    //await _apiHealper.WriteReturnResponseAsync(Response, res);

                }, Response, _logger);

        }

    }
}
