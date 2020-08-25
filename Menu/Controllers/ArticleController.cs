using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Auth.Services.Interfaces;
using Menu.Models.DAL.Repositories.Interfaces;
using Menu.Models.Healpers.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Menu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {

        private readonly IArticleRepository _articleRepository;
        private readonly IJWTService _jwtService;

        public ArticleController(IArticleRepository articleRepository
            , IJWTService jwtService)
        {
            _articleRepository = articleRepository;
            _jwtService = jwtService;


        }

        // GET: api/<ArticleController>
        [HttpGet]
        public async Task GetAllForUser()
        {
            _jwtService.GeUserByAccessTokenAsync();
            _articleRepository.GetAllUsersArticles();

        }

        [HttpGet]
        public async Task Detail()
        {
            

        }

        [HttpPatch]
        public async Task Follow()
        {


        }

        [HttpPatch]
        public async Task Edit()
        {


        }

    }
}
