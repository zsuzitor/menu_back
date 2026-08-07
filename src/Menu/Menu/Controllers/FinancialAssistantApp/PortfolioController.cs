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
    public class PortfolioController : ControllerBase
    {
        private readonly IApiHelper _apiHealper;
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IApiHelper apiHealper, IPortfolioService portfolioService)
        {
            _apiHealper = apiHealper;
            _portfolioService = portfolioService;
        }

        [Route("get-for-user")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<List<PortfolioReturn>>> GetForUser()
        {
            var userId = User.GetUserId();
            var res = await _portfolioService.GetAllAsync(userId);
            return new JsonResult(res.Select(x => x.Map()), GetJsonOptions());
        }

        [Route("create")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<PortfolioReturn>> Create([FromBody] CreatePortfolioRequest req)
        {
            var userId = User.GetUserId();
            var res = await _portfolioService.CreateAsync(req.Map(), userId);
            return new JsonResult(res.Map(), GetJsonOptions());
        }

        [Route("update")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<PortfolioReturn>> Update([FromBody] CreatePortfolioRequest req)
        {
            var userId = User.GetUserId();
            var res = await _portfolioService.UpdateAsync(req.Map(), userId);
            return new JsonResult(res.Map(), GetJsonOptions());
        }

        [Route("delete")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> Delete([FromBody] DeletePortfolioRequest req)
        {
            var userId = User.GetUserId();
            var res = await _portfolioService.DeleteAsync(req.Id, userId);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());
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
