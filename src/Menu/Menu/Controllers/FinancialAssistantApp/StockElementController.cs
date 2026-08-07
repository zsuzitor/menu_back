using Auth.Models.Auth;
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
    public class StockElementController : ControllerBase
    {
        private readonly IApiHelper _apiHealper;
        private readonly IStockElementService _stockElementService;

        public StockElementController(IApiHelper apiHealper, IStockElementService stockElementService)
        {
            _apiHealper = apiHealper;
            _stockElementService = stockElementService;
        }


        [Route("get")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<List<StockElementReturn>>> Get([FromBody] GetStockElementRequest req)
        {
            var userId = User.GetUserId();
            var res = await _stockElementService.Get(req.PortfolioId, userId);
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
