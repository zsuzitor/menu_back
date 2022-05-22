using CodeReviewApp.Models.Returns;
using CodeReviewApp.Models.Services.Interfaces;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.CodeReviewApp
{
    [Route("api/codereview/[controller]")]

    public class CommentController : ControllerBase
    {

        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly ITaskReviewService _taskReviewService;
        private readonly ITaskReviewCommentService _taskReviewCommentService;


        public CommentController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILogger<ProjectController> logger, ITaskReviewService taskReviewService,
             ITaskReviewCommentService taskReviewCommentService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = logger;

            _taskReviewService = taskReviewService;
            _taskReviewCommentService = taskReviewCommentService;
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

                    var res = await _taskReviewService.CreateCommentAsync(taskId, text, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new CommentReviewReturn(res));

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

                    var res = await _taskReviewCommentService.DeleteAsync(commentId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res != null,
                        });

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

                    var res = await _taskReviewService.GetCommentsAsync(taskId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res.Select(x => new CommentReviewReturn(x)));

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

                    var res = await _taskReviewCommentService.EditAsync(commentId, text, userInfo);
                    await _apiHealper.WriteResponseAsync(Response
                        , new
                        {
                            result = res != null,
                        });

                }, Response, _logger);

        }
    }
}
