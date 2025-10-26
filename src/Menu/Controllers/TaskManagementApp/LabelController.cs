using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.TaskManagementApp.Mappers;
using Menu.Models.TaskManagementApp.Requests;
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
    public class LabelController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;
        private readonly ILabelService _labelService;


        public LabelController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILogger<ProjectController> logger, ILabelService labelService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = logger;

            _labelService = labelService;
        }


        [Route("get-all")]
        [HttpGet]
        public async Task<ActionResult<List<WorkTaskLabelReturn>>> GetLabels(long projectId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _labelService.Get(projectId, userInfo);
            return new JsonResult(res.Select(x => new WorkTaskLabelReturn(x)).ToList(), GetJsonOptions());
        }

        [Route("create")]
        [HttpPut]
        public async Task<ActionResult<WorkTaskLabelReturn>> CreateLabel([FromBody] AddNewLabelRequest request)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

            request.Name = _apiHealper.StringValidator(request.Name);
            var res = await _labelService.Create(request.Map(), userInfo);
            return new JsonResult(new WorkTaskLabelReturn(res), GetJsonOptions());
        }

        [Route("update")]
        [HttpPatch]
        public async Task<ActionResult<WorkTaskLabelReturn>> UpdateLabel([FromBody] UpdateLabelRequest request)
        {

            request.Name = _apiHealper.StringValidator(request.Name);
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _labelService.Update(request.Map(), userInfo);
            return new JsonResult(new WorkTaskLabelReturn(res), GetJsonOptions());
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<ActionResult<BoolResultReturn>> DeleteLabel([FromBody] DeleteTaskLabels req)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _labelService.Delete(req.Id, userInfo);
            return new JsonResult(new BoolResultReturn(res), GetJsonOptions());
        }


        [Route("update-task-labels")]
        [HttpPost]
        public async Task<ActionResult<BoolResultReturn>> UpdateTaskLabels([FromBody] UpdateTaskLabels req)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _labelService.UpdateTaskLabels(req.LabelId, req.TaskId, userInfo);
            return new JsonResult(new BoolResultReturn(res), GetJsonOptions());
        }

        [Route("add-to-task")]
        [HttpPost]
        public async Task<ActionResult<BoolResultReturn>> Add([FromBody] AddLabelToTask req)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _labelService.AddToTask(req.LabelId, req.TaskId, userInfo);
            return new JsonResult(new BoolResultReturn(res), GetJsonOptions());
        }

        [Route("delete-from-task")]
        [HttpPost]
        public async Task<ActionResult<BoolResultReturn>> Remove([FromBody] RemoveLabelFromTask req)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _labelService.RemoveFromTask(req.LabelId, req.TaskId, userInfo);
            return new JsonResult(new BoolResultReturn(res), GetJsonOptions());
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
