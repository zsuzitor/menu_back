using Auth.Models.Auth;
using Common.Models;
using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Host.Infrastructure;
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

    public class CommentController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;

        private readonly IWorkTaskService _workTaskService;
        private readonly IWorkTaskCommentService _workTaskCommentService;


        public CommentController(
             IJWTService jwtService, IApiHelper apiHealper
            , IWorkTaskService workTaskService,
             IWorkTaskCommentService workTaskCommentService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;

            _workTaskService = workTaskService;
            _workTaskCommentService = workTaskCommentService;
        }


        [Route("create-comment")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<WorkTaskCommentReturn>> CreateComment([FromForm] long taskId, [FromForm] string text)
        {
            var userId = User.GetUserId();
            text = _apiHealper.StringValidator(text);
            var res = await _workTaskService.CreateCommentAsync(taskId, text, userId);
            return new JsonResult(new WorkTaskCommentReturn(res), GetJsonOptions());

        }

        [Route("delete-comment")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> DeleteComment([FromForm] long commentId)
        {
            var userId = User.GetUserId();

            var res = await _workTaskCommentService.DeleteAsync(commentId, userId);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());

        }

        [Route("get-comments")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<List<WorkTaskCommentReturn>>> GetComments(long taskId)
        {
            var userId = User.GetUserId();
            var res = await _workTaskService.GetCommentsAsync(taskId, userId);
            return new JsonResult(res.Select(x => new WorkTaskCommentReturn(x)).ToList(), GetJsonOptions());

        }

        [Route("edit-comment")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> EditComment([FromForm] long commentId, [FromForm] string text)
        {
            var userId = User.GetUserId();
            text = _apiHealper.StringValidator(text);
            var res = await _workTaskCommentService.EditAsync(commentId, text, userId);
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
