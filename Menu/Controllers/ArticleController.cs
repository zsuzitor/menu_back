
using System.Linq;
using System.Threading.Tasks;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Healpers.Interfaces;
using Menu.Models.InputModels;
using Menu.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Menu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {

        //private readonly IArticleRepository _articleRepository;
        private readonly IJWTService _jwtService;
        private readonly IApiHealper _apiHealper;
        private readonly IArticleService _articleService;
        //private readonly IWebHostEnvironment _webHostEnvironment;


        public ArticleController(
             IJWTService jwtService, IApiHealper apiHealper, IArticleService articleService)
        {
            //_articleRepository = articleRepository;
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _articleService = articleService;
            //_webHostEnvironment = webHostEnvironment;
        }

        // GET: api/<ArticleController>
        [HttpGet]
        public async Task GetAllForUser()
        {
            var userInfo = _apiHealper.GetUserInfoFromRequest(Request,_jwtService);


            var res = await _articleService.GetAllUsersArticles(userInfo);


            await _apiHealper.WriteResponse(Response, res);
        }

        [HttpGet]
        public async Task Detail(long articleId)
        {

            var userInfo = _apiHealper.GetUserInfoFromRequest(Request, _jwtService);


            var res = await _articleService.GetByIdIfAccess(articleId, userInfo);


            await _apiHealper.WriteResponse(Response, res);
        }

        [HttpPatch]
        public async Task Follow(long id)
        {
            var userInfo = _apiHealper.GetUserInfoFromRequest(Request, _jwtService);

            bool? res = await _articleService.ChangeFollowStatus(id, userInfo);


            await _apiHealper.WriteResponse(Response, res);

        }

        [HttpPut]
        public async Task Create(ArticleInputModel newData)
        {
            //todo validate
            if (ModelState.IsValid)
            {
                var errors = ModelState.ToList();//TODO докинуть в _errorService
                await _apiHealper.WriteResponse(Response, errors);
                return;
            }

            var userInfo = _apiHealper.GetUserInfoFromRequest(Request, _jwtService);

            await _articleService.Create(newData, userInfo);
        }

        [HttpPatch]
        public async Task Edit(ArticleInputModel newData)
        {
            if (newData.Id == null)
            {
                ModelState.AddModelError("not_filled_id","не передано id");
            }
            //todo validate +image
            if (ModelState.IsValid)
            {
                var errors = ModelState.ToList();//TODO докинуть в _errorService
                await _apiHealper.WriteResponse(Response, errors);
                return;
            }

            var userInfo = _apiHealper.GetUserInfoFromRequest(Request, _jwtService);

            bool res = await _articleService.Edit(newData, userInfo);

        }

    }
}
