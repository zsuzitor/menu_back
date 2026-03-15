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
    public class LabelController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILabelService _labelService;


        public LabelController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILabelService labelService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;

            _labelService = labelService;
        }


        [Route("get-all")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<List<WorkTaskLabelReturn>>> GetLabels(long projectId)
        {
            var userId = User.GetUserId();
            var res = await _labelService.Get(projectId, userId);
            return new JsonResult(res.Select(x => new WorkTaskLabelReturn(x)).ToList(), GetJsonOptions());
        }

        [Route("create")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTaskLabelReturn>> CreateLabel([FromBody] AddNewLabelRequest request)
        {
            var userId = User.GetUserId();

            request.Name = _apiHealper.StringValidator(request.Name);
            var res = await _labelService.Create(request.Map(), userId);
            return new JsonResult(new WorkTaskLabelReturn(res), GetJsonOptions());
        }

        [Route("update")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTaskLabelReturn>> UpdateLabel([FromBody] UpdateLabelRequest request)
        {

            request.Name = _apiHealper.StringValidator(request.Name);
            var userId = User.GetUserId();
            var res = await _labelService.Update(request.Map(), userId);
            return new JsonResult(new WorkTaskLabelReturn(res), GetJsonOptions());
        }

        [Route("delete")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> DeleteLabel([FromBody] DeleteTaskLabels req)
        {
            var userId = User.GetUserId();
            var res = await _labelService.Delete(req.Id, userId);
            return new JsonResult(new BoolResultNewReturn(res), GetJsonOptions());
        }


        [Route("update-task-labels")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> UpdateTaskLabels([FromBody] UpdateTaskLabels req)
        {
            var userId = User.GetUserId();
            var res = await _labelService.UpdateTaskLabels(req.LabelId, req.TaskId, userId);
            return new JsonResult(new BoolResultNewReturn(res), GetJsonOptions());
        }

        [Route("add-to-task")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> Add([FromBody] AddLabelToTask req)
        {
            var userId = User.GetUserId();
            var res = await _labelService.AddToTask(req.LabelId, req.TaskId, userId);
            return new JsonResult(new BoolResultNewReturn(res), GetJsonOptions());
        }

        [Route("delete-from-task")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> Remove([FromBody] RemoveLabelFromTask req)
        {
            var userId = User.GetUserId();
            var res = await _labelService.RemoveFromTask(req.LabelId, req.TaskId, userId);
            return new JsonResult(new BoolResultNewReturn(res), GetJsonOptions());
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
