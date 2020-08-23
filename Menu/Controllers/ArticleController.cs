using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Menu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        // GET: api/<ArticleController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            //HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationToken);
            HttpContext.Request.Cookies.TryGetValue("Authorization", out var authorizationToken);
            return new string[] { "value1", "value2" };
        }

       
    }
}
