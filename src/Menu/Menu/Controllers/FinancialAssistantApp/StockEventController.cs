using Auth.Models.Auth;
using Common.Models.Return;
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


        [Route("delete")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> Delete([FromBody] DeleteStockEventRequest req)
        {
            var userId = User.GetUserId();
            var res = await _stockEventService.DeleteEventAsync(req.Id,req.Force, userId);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
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
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult<GetPortfolioEventsResponse>> GetHistory([FromBody] GetPortfolioEventsRequest req)
        {
            var userId = User.GetUserId();
            var request = req.Map();
            request.UserId = userId;
            var res = await _stockEventService.GetForPortfolioAsync(request);
            return new JsonResult(res.Map(), GetJsonOptions());
        }

        [Route("get-events-for-stock")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult<GetStockEventsResponse>> GetStockHistory([FromBody] GetStockEventsRequest req)
        {
            var userId = User.GetUserId();
            var res = await _stockEventService.GetForStockAsync(req.PortfolioId, req.StockId, userId, req.PageSize, req.Page);
            return new JsonResult(res.MapGetStockEventsResponse(), GetJsonOptions());
        }

        [Route("portfolio-recalculate")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult<GetStockEventsResponse>> PortfolioRecalculate([FromBody] PortfolioRecalculateRequest req)
        {
            var userId = User.GetUserId();
             await _stockEventService.PortfolioRecalculate(req.PortfolioId,userId);
            return new JsonResult(new BoolResultNewReturn(true), GetJsonOptions());
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
