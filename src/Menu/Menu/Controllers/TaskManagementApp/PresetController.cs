using Auth.Models.Auth;
using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Host.Infrastructure;
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
    [Tags("taskmanagement")]
    public class PresetController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly IPresetService _presetService;

        public PresetController(
         IJWTService jwtService, IApiHelper apiHealper
        , IPresetService presetService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;

            _presetService = presetService;
        }

        [Route("get-all")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<List<PresetReturn>>> GetLabels(long projectId)
        {
            var userId = User.GetUserId();
            var res = await _presetService.GetAllAsync(projectId, userId);
            return new JsonResult(res.Select(x => new PresetReturn(x)).ToList(), GetJsonOptions());
        }

        [Route("delete")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> Delete([FromForm] long presetId)
        {
            var userId = User.GetUserId();
            var res = await _presetService.DeleteAsync(presetId, userId);
            return new JsonResult(new BoolResultNewReturn(res!=null), GetJsonOptions());
        }

        [Route("create")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> Create(CreatePresetRequest req)
        {
            req.Name = _apiHealper.StringValidator(req.Name);
            var userId = User.GetUserId();
            var res = await _presetService.CreateAsync(req.ProjectId, req.Name, userId);
            return new JsonResult(new PresetReturn(res), GetJsonOptions());
        }

        [Route("update")]
        [HttpPatch]
        [CustomAuthorize]
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

            var userId = User.GetUserId();
            var res = await _presetService.EditAsync(req.Map(), userId);
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
