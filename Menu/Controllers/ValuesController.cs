using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Models.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Menu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //IDistributedCache _cache;//enable AddStackExchangeRedisCache or another in startup

        public ValuesController(
            //IDistributedCache cache
            //IWorker worker
            )
        {
            //_cache = cache;
            //worker.Recurring("", "* * * * *");
        }

        [HttpGet("cache-test")]
        public void CacheCheck()
        {
            //_cache.SetString("test_key_1", "test_val", new DistributedCacheEntryOptions()
            //{
            //    AbsoluteExpirationRelativeToNow = new TimeSpan(0, 1, 0)
            //});

            //var val = _cache.GetString("test_key_1");
        }


        [HttpGet("get")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("get/{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpPost("post")]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("put/{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("delete/{id}")]
        public void Delete(int id)
        {
        }
    }
}
