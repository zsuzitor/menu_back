using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services.Interfaces;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;
using TaskManagementApp.Models.Services;
using Common.Models.Return;
using System.Text.Json;
using System.Collections.Generic;
using Common.Models;

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]
    [ApiController]
    [Produces("application/json")]

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
        public async Task<ActionResult<WorkTaskCommentReturn>> CreateComment([FromForm] long taskId, [FromForm] string text)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            text = _apiHealper.StringValidator(text);
            var res = await _workTaskService.CreateCommentAsync(taskId, text, userInfo);
            return new JsonResult(new WorkTaskCommentReturn(res), GetJsonOptions());

        }

        [Route("delete-comment")]
        [HttpDelete]
        public async Task<ActionResult<BoolResultNewReturn>> DeleteComment([FromForm] long commentId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

            var res = await _workTaskCommentService.DeleteAsync(commentId, userInfo);
            return new JsonResult(new BoolResultNewReturn(res != null), GetJsonOptions());

        }

        [Route("get-comments")]
        [HttpGet]
        public async Task<ActionResult<List<WorkTaskCommentReturn>>> GetComments(long taskId)
        {

            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _workTaskService.GetCommentsAsync(taskId, userInfo);
            return new JsonResult(res.Select(x => new WorkTaskCommentReturn(x)).ToList(), GetJsonOptions());

        }

        [Route("edit-comment")]
        [HttpPatch]
        public async Task<ActionResult<BoolResultNewReturn>> EditComment([FromForm] long commentId, [FromForm] string text)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            text = _apiHealper.StringValidator(text);
            var res = await _workTaskCommentService.EditAsync(commentId, text, userInfo);
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
