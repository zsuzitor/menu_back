using BO.Models.TaskManagementApp.DAL.Domain;
using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services.Interfaces;
using Common.Models.Exceptions;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using WEB.Common.Models.Helpers.Interfaces;
using Common.Models.Return;
using System.Text.Json;
using System.Collections.Generic;
using Common.Models;

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ProjectController : ControllerBase
    {
        private readonly IJWTService _jwtService;
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IProjectService _projectService;
        private readonly IWorkTaskStatusService _statusService;
        private readonly IProjectUserService _projectUserService;
        private readonly ISprintService _sprintService;
        private readonly ILabelService _labelService;
        private readonly IPresetService _presetService;


        public ProjectController(
             IJWTService jwtService, IApiHelper apiHealper
            , ILoggerFactory loggerFactory, IProjectService projectService,
             IProjectUserService projectUserService,
             IWorkTaskStatusService statusService, ISprintService sprintService, ILabelService labelService, IPresetService presetService)
        {
            _jwtService = jwtService;
            _apiHealper = apiHealper;
            _logger = loggerFactory.CreateLogger(Constants.Loggers.MenuApp);

            _projectService = projectService;
            _projectUserService = projectUserService;
            _statusService = statusService;
            _sprintService = sprintService;
            _labelService = labelService;
            _presetService = presetService;
        }

        [Route("get-projects")]
        [HttpGet]
        public async Task<ActionResult<List<ProjectInList>>> GetProjects()
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _projectService.GetProjectsByMainAppUserIdAsync(userInfo.UserId);
            res = res ?? new System.Collections.Generic.List<Project>();
            return new JsonResult(res.Select(x => new ProjectInList(x)).ToList(), GetJsonOptions());

        }

        [Route("add-new-project")]
        [HttpPut]
        public async Task<ActionResult<ProjectInList>> AddProject([FromForm] string projectName)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            projectName = _apiHealper.StringValidator(projectName);

            var res = await _projectService.CreateAsync(projectName, userInfo);
            return new JsonResult(new ProjectInList(res), GetJsonOptions());
        }

        [Route("get-project-info")]
        [HttpGet]
        public async Task<ActionResult<ProjectFullInfoReturn>> GetProjectInfo(long projectId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            _logger.LogDebug("ProjectController GetProjectInfo projectId={projectId}", projectId);

            var res = await _projectService.GetByIdIfAccessAsync(projectId, userInfo);
            if (res == null)
            {
                throw new SomeCustomException("project_not_found");
            }


            //todo много запросов
            var statuses = (await _statusService.GetStatusesAsync(projectId)).Select(x => new WorkTaskStatusReturn(x)).ToList();
            var sprints = (await _sprintService.GetForProject(projectId)).Select(x => new ProjectSprintReturn(x)).ToList();
            var labels = (await _labelService.Get(projectId)).Select(x => new ProjectLabelReturn(x)).ToList();
            var presets = (await _presetService.GetAllWithLabelsAsync(projectId)).Select(x => new PresetReturn(x)).ToList();

            var users = await _projectUserService.GetProjectUsersAsync(projectId, userInfo);
            var usersReturn = users.Select(x => new ProjectUserReturn(x)).ToList();

            return new JsonResult(new ProjectFullInfoReturn() { Users = usersReturn, Statuses = statuses, Sprints = sprints, Labels = labels, Presets = presets }, GetJsonOptions());

        }




        [Route("delete-project")]
        [HttpDelete]
        public async Task<ActionResult<BoolResultNewReturn>> DeleteProject([FromForm] long projectId)
        {
            var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
            var res = await _projectService.DeleteAsync(projectId, userInfo);
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
