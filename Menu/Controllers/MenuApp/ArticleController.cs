﻿
using System;
using System.Threading.Tasks;
using jwtLib.JWTAuth.Interfaces;
using Common.Models.Error.services.Interfaces;
using Common.Models.Exceptions;
using WEB.Common.Models.Helpers.Interfaces;
using Common.Models.Entity;
using Common.Models.Return;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Menu.Models.InputModels.MenuApp;
using Menu.Models.Returns.Types.MenuApp;
using BL.Models.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Menu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {

        //private readonly IArticleRepository _articleRepository;
        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly IArticleService _articleService;
        private readonly IErrorService _errorService;
        private readonly IConfigurationService _configurationService;
        private readonly ILogger _logger;
        //private readonly IWebHostEnvironment _webHostEnvironment;

        //private readonly ErrorObjectReturnFactory _errRetrunFactory;
        private readonly ArticleShortReturnFactory _articleShortReturnFactory;
        private readonly ArticleReturnFactory _articleReturnFactory;
        //private readonly BoolResultFactory _boolResultFactory;


        public ArticleController(
             IJWTService jwtService, IApiHelper apiHealper, IArticleService articleService,
             IErrorService errorService, IConfigurationService configurationService, ILogger<ArticleController> logger)
        {
            //_articleRepository = articleRepository;
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _articleService = articleService;
            _errorService = errorService;
            _configurationService = configurationService;
            _logger = logger;
            //_webHostEnvironment = webHostEnvironment;

            //_errRetrunFactory = new ErrorObjectReturnFactory();
            _articleShortReturnFactory = new ArticleShortReturnFactory();
            _articleReturnFactory = new ArticleReturnFactory();
            //_boolResultFactory = new BoolResultFactory();
        }

        [Route("get-all-short-for-user")]
        [HttpGet]
        public async Task GetAllShortForUser()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _articleService.GetAllUsersArticlesShort(userInfo);

                    await _apiHealper.WriteResponseAsync(Response, _articleShortReturnFactory.GetObjectReturn(res));

                }, Response, _logger);

        }

        // GET: api/<ArticleController>
        [Route("get-all-for-user")]
        [HttpGet]
        public async Task GetAllForUser()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _articleService.GetAllUsersArticles(userInfo);
                    await _apiHealper.WriteResponseAsync(Response, _articleReturnFactory.GetObjectReturn(res));
                }, Response, _logger);

        }

        [Route("detail")]
        [HttpGet]
        public async Task Detail(long id)//[FromQuery]
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                   var res = await _articleService.GetFullByIdIfAccess(id, userInfo);
                   await _apiHealper.WriteResponseAsync(Response, _articleReturnFactory.GetObjectReturn(res));
               }, Response, _logger);

        }

        [Route("follow")]
        [HttpPatch]
        public async Task Follow([FromForm] long id)
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                   bool res = await _articleService.ChangeFollowStatus(id, userInfo);

                   await _apiHealper.WriteResponseAsync(Response, new BoolResultFactory().GetObjectReturn(new BoolResult(res)));
               }, Response, _logger);
        }

        [Route("create")]
        [HttpPut]
        public async Task Create([FromForm] ArticleInputModelApi newData)
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   newData.Validate(_apiHealper.StringValidator, _apiHealper.FileValidator, ModelState);
                   _apiHealper.ErrorsFromModelState(ModelState);
                   if (_errorService.HasError())
                   {
                       throw new StopException();
                   }

                   var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                  
                   var newArticle = await _articleService.Create(newData.GetModel(), userInfo);
                   await _apiHealper.WriteResponseAsync(Response, _articleReturnFactory.GetObjectReturn(newArticle));
               }, Response, _logger);

        }

        [Route("edit")]
        [HttpPatch]
        public async Task Edit([FromForm] ArticleInputModelApi newData)
        {
            //ArticleInputModel newData = null;
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   if (newData.Id == null)
                   {
                       ModelState.AddModelError("id_is_required", "не передано id");//TODO подумать как вынести в общий кусок все такие штуки
                   }

                   newData.Validate(_apiHealper.StringValidator, _apiHealper.FileValidator, ModelState);
                   _apiHealper.ErrorsFromModelState(ModelState);
                   if (_errorService.HasError())
                   {
                       throw new StopException();
                   }

                   var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                   
                   var res = await _articleService.Update(newData.GetModel(), userInfo);
                   await _apiHealper.WriteResponseAsync(Response, _articleReturnFactory.GetObjectReturn(res));
               }, Response, _logger);

        }
    }
}
