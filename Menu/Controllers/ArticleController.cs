
using System;
using System.Threading.Tasks;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Error.Interfaces;
using Menu.Models.Error.services.Interfaces;
using Menu.Models.Exceptions;
using Menu.Models.Helpers.Interfaces;
using Menu.Models.InputModels;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        private readonly IErrorContainer _errorContainer;
        private readonly ILogger _logger;
        //private readonly IWebHostEnvironment _webHostEnvironment;


        public ArticleController(
             IJWTService jwtService, IApiHelper apiHealper, IArticleService articleService,
             IErrorService errorService, IErrorContainer errorContainer, ILogger<ArticleController> logger)
        {
            //_articleRepository = articleRepository;
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _articleService = articleService;
            _errorService = errorService;
            _errorContainer = errorContainer;
            _logger = logger;
            //_webHostEnvironment = webHostEnvironment;
        }

        [Route("get-all-short-for-user")]
        [HttpGet]
        public async Task GetAllShortForUser()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _articleService.GetAllUsersArticlesShort(userInfo);

                    await _apiHealper.WriteReturnResponseAsync(Response, res);

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
                    await _apiHealper.WriteReturnResponseAsync(Response, res);
                }, Response, _logger);

        }

        [Route("detail")]
        [HttpGet]
        public async Task Detail(long articleId)
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                   var res = await _articleService.GetFullByIdIfAccess(articleId, userInfo);
                   await _apiHealper.WriteReturnResponseAsync(Response, res);
               }, Response, _logger);

        }

        [Route("follow")]
        [HttpPatch]
        public async Task Follow(long id)
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                   bool res = await _articleService.ChangeFollowStatus(id, userInfo);

                   await _apiHealper.WriteResponseAsync(Response, res);
               }, Response, _logger);


        }

        [Route("create")]
        [HttpPut]
        public async Task Create([FromForm] ArticleInputModel newData)
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   _errorService.ErrorsFromModelState(ModelState);
                   if (_errorService.HasError())
                   {
                       throw new StopException();
                   }

                   var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                   var newArticle = await _articleService.Create(newData, userInfo);
                   await _apiHealper.WriteReturnResponseAsync(Response, newArticle);
               }, Response, _logger);

        }

        [Route("edit")]
        [HttpPatch]
        public async Task Edit([FromForm] ArticleInputModel newData)
        {
            await _apiHealper.DoStandartSomething(
               async () =>
               {
                   if (newData.Id == null)
                   {
                       ModelState.AddModelError("id_is_required", "не передано id");//TODO подумать как вынести в общий кусок все такие штуки
                   }

                   _errorService.ErrorsFromModelState(ModelState);
                   if (_errorService.HasError())
                   {
                       throw new StopException();
                   }

                   var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                   bool res = await _articleService.Edit(newData, userInfo);
                   await _apiHealper.WriteResponseAsync(Response, res);
               }, Response, _logger);

        }
    }
}
