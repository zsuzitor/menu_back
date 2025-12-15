using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.TaskManagementApp.Mappers;
using Menu.Models.TaskManagementApp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        public async Task<ActionResult<List<PresetReturn>>> GetLabels(long projectId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _presetService.GetAllAsync(projectId, userInfo);
            return new JsonResult(res.Select(x => new PresetReturn(x)).ToList(), GetJsonOptions());
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<ActionResult<BoolResultNewReturn>> Delete([FromForm] long presetId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _presetService.DeleteAsync(presetId, userInfo);
            return new JsonResult(new BoolResultNewReturn(res!=null), GetJsonOptions());
        }

        [Route("create")]
        [HttpPut]
        public async Task<ActionResult<BoolResultNewReturn>> Create(CreatePresetRequest req)
        {
            req.Name = _apiHealper.StringValidator(req.Name);
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _presetService.CreateAsync(req.ProjectId, req.Name, userInfo);
            return new JsonResult(new PresetReturn(res), GetJsonOptions());
        }

        [Route("update")]
        [HttpPatch]
        public async Task<ActionResult<BoolResultNewReturn>> Update(UpdatePresetRequest req)
        {
            req.Name = _apiHealper.StringValidator(req.Name);
            if (req.StatusId < 1)
            {
                req.StatusId = null;
            }

            if (req.CreatorId < 1)
            {
                req.CreatorId = null;
            }

            if (req.ExecutorId < 1)
            {
                req.ExecutorId = null;
            }

            if (req.SprintId < 1)
            {
                req.SprintId = null;
            }

            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _presetService.EditAsync(req.Map(), userInfo);
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
