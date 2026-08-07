using Auth.Models.Auth;
using FinancialAssistantApp.Models.Services.Interfaces;
using Menu.Host.Infrastructure;
using Menu.Host.Models.FinancialAssistantApp;
using Menu.Host.Models.FinancialAssistantApp.Requests;
using Menu.Host.Models.FinancialAssistantApp.Returns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
