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
    public class StockController : ControllerBase
    {
        private readonly IApiHelper _apiHealper;
        private readonly IStockService _stockService;

        public StockController(IApiHelper apiHealper, IStockService stockService)
        {
            _apiHealper = apiHealper;
            _stockService = stockService;
        }

        [Route("update-global")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> Get()
        {
            var userId = User.GetUserId();
            await _stockService.GlobalActualizeAsync(userId);
            return new JsonResult(new BoolResultNewReturn(true), GetJsonOptions());
        }

        [Route("create")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<StockReturn>> Create(CreateStockRequest req)
        {
            var userId = User.GetUserId();
            var res = await _stockService.CreateAsync(req.Map(), userId);
            return new JsonResult(res.Map(), GetJsonOptions());
        }

        [Route("delete")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> Delete(DeleteStockRequest req)
        {
            var userId = User.GetUserId();
            var res = await _stockService.DeleteAsync(req.Id, userId);
            return new JsonResult(new BoolResultNewReturn(res!=null), GetJsonOptions());
        }

        [Route("update")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<StockReturn>> Update(CreateStockRequest req)
        {
            var userId = User.GetUserId();
            var res = await _stockService.UpdateAsync(req.Map(), userId);
            return new JsonResult(res.Map(), GetJsonOptions());
        }

        [Route("find")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<List<StockReturn>>> Find(FindStockRequest req)
        {
            var userId = User.GetUserId();
            var res = await _stockService.FindAsync(req.PortfolioId, req.Text, userId);
            return new JsonResult(res.Select(x=>x.Map()), GetJsonOptions());
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
