
using System.Collections.Generic;
using System.Threading.Tasks;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.DAL.Domain;
using Menu.Models.Healpers.Interfaces;
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
        


        public ArticleController(
             IJWTService jwtService, IApiHealper apiHealper, IArticleService articleService)
        {
            //_articleRepository = articleRepository;
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _articleService = articleService;

        }

        // GET: api/<ArticleController>
        [HttpGet]
        public async Task GetAllForUser()
        {
            List<Article> res = null;
            var accessToken = _apiHealper.GetAccessTokenFromRequest(Request);
            var userId= _jwtService.GetUserIdFromAccessToken(accessToken);

            if (long.TryParse(userId,out long userIdLong))
            {

               res=await _articleService.GetAllUsersArticles(userIdLong);
            }

            await _apiHealper.WriteResponse(Response, res);
        }

        [HttpGet]
        public async Task Detail(long articleId)
        {
            Article res = null;

            var accessToken = _apiHealper.GetAccessTokenFromRequest(Request);
            var userId = _jwtService.GetUserIdFromAccessToken(accessToken);

            
                res = await _articleService.GetByIdIfAccess(articleId, userId);
            

            await _apiHealper.WriteResponse(Response, res);
        }

        [HttpPatch]
        public async Task Follow(long id)
        {
            var accessToken = _apiHealper.GetAccessTokenFromRequest(Request);
            var userId = _jwtService.GetUserIdFromAccessToken(accessToken);


            bool? res = await _articleService.ChangeFollowStatus(id, userId);


            await _apiHealper.WriteResponse(Response, res);

        }

        [HttpPut]
        public async Task Create(Article newData)
        {
            //todo validate
        }

            [HttpPatch]
        public async Task Edit(Article newData)
        {
            //todo validate
            var accessToken = _apiHealper.GetAccessTokenFromRequest(Request);
            var userId = _jwtService.GetUserIdFromAccessToken(accessToken);

            bool? res = await _articleService.ChangeFollowStatus(id, userId);

        }

    }
}
