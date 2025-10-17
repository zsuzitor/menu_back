using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.TaskManagementApp.Mappers;
using Menu.Models.TaskManagementApp.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services;
using TaskManagementApp.Models.Services.Interfaces;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.TaskManagementApp
{

    [Route("api/taskmanagement/[controller]")]
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
        public async Task CreateLabel([FromBody] long projectId)
        {


            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _labelService.Get(projectId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res.Select(x=> new WorkTaskLabelReturn(x)));

                }, Response, _logger);

        }

        [Route("create")]
        [HttpPut]
        public async Task CreateLabel([FromBody] AddNewLabelRequest request)
        {

            request.Name = _apiHealper.StringValidator(request.Name);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _labelService.Create(request.Map(), userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new WorkTaskLabelReturn(res));

                }, Response, _logger);

        }

        [Route("update")]
        [HttpPatch]
        public async Task UpdateLabel([FromBody] UpdateLabelRequest request)
        {

            request.Name = _apiHealper.StringValidator(request.Name);

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _labelService.Update(request.Map(), userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new WorkTaskLabelReturn(res));

                }, Response, _logger);

        }

        [Route("delete")]
        [HttpDelete]
        public async Task DeleteLabel([FromBody] long id)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _labelService.Delete(id, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultReturn(res));

                }, Response, _logger);

        }


        [Route("update-task-labels")]
        [HttpPost]
        public async Task UpdateTaskLabels([FromBody] UpdateTaskLabels req)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _labelService.UpdateTaskLabels(req.LabelId, req.TaskId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultReturn(res));

                }, Response, _logger);

        }

        [Route("add-to-task")]
        [HttpPost]
        public async Task Add([FromBody] AddLabelToTask req)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _labelService.AddToTask(req.LabelId, req.TaskId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultReturn(res));

                }, Response, _logger);

        }

        [Route("delete-from-task")]
        [HttpPost]
        public async Task Remove([FromBody] RemoveLabelFromTask req)
        {

            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    //throw new NotAuthException();

                    var res = await _labelService.RemoveFromTask(req.LabelId, req.TaskId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultReturn(res));

                }, Response, _logger);

        }

    }
}
