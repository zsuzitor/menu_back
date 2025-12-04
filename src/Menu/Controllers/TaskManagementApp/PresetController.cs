using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services;
using TaskManagementApp.Models.Services.Interfaces;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PresetController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;
        private readonly IPresetService _presetService;

        public PresetController(
         IJWTService jwtService, IApiHelper apiHealper
        , ILogger<ProjectController> logger, IPresetService presetService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = logger;

            _presetService = presetService;
        }

        [Route("get-all")]
        [HttpGet]
        public async Task<ActionResult<List<PresetWorkTaskLabelReturn>>> GetLabels(long projectId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _labelService.Get(projectId, userInfo);
            return new JsonResult(res.Select(x => new PresetWorkTaskLabelReturn(x)).ToList(), GetJsonOptions());
        }

    }
}
