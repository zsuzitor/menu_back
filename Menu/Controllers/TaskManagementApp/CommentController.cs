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

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]

    public class CommentController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IWorkTaskService _workTaskService;
        private readonly IWorkTaskCommentService _workTaskCommentService;


        public CommentController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILogger<ProjectController> logger, IWorkTaskService workTaskService,
             IWorkTaskCommentService workTaskCommentService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = logger;

            _workTaskService = workTaskService;
            _workTaskCommentService = workTaskCommentService;
        }


        [Route("create-comment")]
        [HttpPut]
        public async Task CreateComment([FromForm] long taskId, [FromForm] string text)
        {
            text = _apiHealper.StringValidator(text);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _workTaskService.CreateCommentAsync(taskId, text, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new WorkTaskCommentReturn(res));

                }, Response, _logger);

        }

        [Route("delete-comment")]
        [HttpDelete]
        public async Task DeleteComment([FromForm] long commentId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _workTaskCommentService.DeleteAsync(commentId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new BoolResultReturn(res != null));

                }, Response, _logger);

        }

        [Route("get-comments")]
        [HttpGet]
        public async Task GetComments(long taskId)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _workTaskService.GetCommentsAsync(taskId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res.Select(x => new WorkTaskCommentReturn(x)));

                }, Response, _logger);

        }

        [Route("edit-comment")]
        [HttpPatch]
        public async Task EditComment([FromForm] long commentId, [FromForm] string text)
        {
            text = _apiHealper.StringValidator(text);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _workTaskCommentService.EditAsync(commentId, text, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new BoolResultReturn(res != null));

                }, Response, _logger);

        }
    }
}
