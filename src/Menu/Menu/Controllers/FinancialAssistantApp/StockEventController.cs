using Auth.Models.Auth;
using FinancialAssistantApp.Models.Services;
using FinancialAssistantApp.Models.Services.Interfaces;
using Menu.Host.Infrastructure;
using Menu.Host.Models.FinancialAssistantApp;
using Menu.Host.Models.FinancialAssistantApp.Requests;
using Menu.Host.Models.FinancialAssistantApp.Returns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Host.Controllers.FinancialAssistantApp
{
    [Route("api/financialassistant/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Tags("financialassistant")]
    public class StockEventController : ControllerBase
    {
        private readonly IApiHelper _apiHealper;
        private readonly IStockEventService _stockEventService;

        public StockEventController(IApiHelper apiHealper, IStockEventService stockEventService)
        {
            _apiHealper = apiHealper;
            _stockEventService = stockEventService;
        }


        [Route("create")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<List<StockEventReturn>>> Create([FromBody] StockEventCreateRequest req)
        {
            var userId = User.GetUserId();
            var res = await _stockEventService.CreateEventAsync(req.Map(), userId);
            return new JsonResult(res.Map(), GetJsonOptions());
        }

        [Route("get-events-for-portfolio")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<List<StockEventReturn>>> GetHistory(long portfolioId)
        {
            var userId = User.GetUserId();
            var res = await _stockEventService.GetForPortfolioAsync(portfolioId, userId);
            return new JsonResult(res.Select(x => x.Map()), GetJsonOptions());
        }

        [Route("get-events-for-stock")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<List<StockEventReturn>>> GetStockHistory(long portfolioId, long stockId)
        {
            var userId = User.GetUserId();
            var res = await _stockEventService.GetForStockAsync(portfolioId, stockId, userId);
            return new JsonResult(res.Select(x => x.Map()), GetJsonOptions());
        }


        private JsonSerializerOptions GetJsonOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNamingPolicy = null, // PascalCase
                WriteIndented = true
            };
        }
    }
}
