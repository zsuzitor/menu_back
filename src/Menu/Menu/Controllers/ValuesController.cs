using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BL.Models.Services.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Menu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDistributedCache _cache;//enable AddStackExchangeRedisCache or another in startup
        private readonly ILogger _logger;
        public ValuesController(
            IDistributedCache cache
            //IWorker worker
            , ILoggerFactory loggerFactory
            )
        {
            //ch.Get("t3",out int t1);
            _cache = cache;
            //worker.Recurring("", "* * * * *");
            _logger = loggerFactory.CreateLogger(Constants.Loggers.MenuApp);
        }

        [HttpGet("cache-test")]
        public string CacheCheck()
        {
            _cache.SetString("test_key_1", "test_val", new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = new TimeSpan(0, 5, 0)
            });

            var val = _cache.GetString("test_key_1");
            return "cached";
        }

        [HttpGet("throw")]
        public string Throw_()
        {
            try
            {
                throw new Exception("тестовое исключение valuecontroller");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "2тестовое исключение valuecontroller");
                //throw;
            }

            return "throwed";
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
