using Auth.Models.Auth;
using BO.Models.TaskManagementApp.DAL.Domain;
using Common.Models;
using Common.Models.Exceptions;
using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Host.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using TaskManagementApp.Models.Returns;
using TaskManagementApp.Models.Services.Interfaces;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.TaskManagementApp
{
    [Route("api/taskmanagement/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Tags("taskmanagement")]
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
        [CustomAuthorize]
        public async Task<ActionResult<List<ProjectInList>>> GetProjects()
        {
            var userId = User.GetUserId();
            var res = await _projectService.GetProjectsByMainAppUserIdAsync(userId);
            res = res ?? new System.Collections.Generic.List<Project>();
            return new JsonResult(res.Select(x => new ProjectInList(x)).ToList(), GetJsonOptions());

        }

        [Route("add-new-project")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<ProjectInList>> AddProject([FromForm] string projectName)
        {
            var userId = User.GetUserId();
            projectName = _apiHealper.StringValidator(projectName);

            var res = await _projectService.CreateAsync(projectName, userId);
            return new JsonResult(new ProjectInList(res), GetJsonOptions());
        }

        [Route("get-project-info")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<ProjectFullInfoReturn>> GetProjectInfo(long projectId)
        {
            var userId = User.GetUserId();
            _logger.LogDebug("ProjectController GetProjectInfo projectId={projectId}", projectId);

            var res = await _projectService.GetByIdIfAccessAsync(projectId, userId);
            if (res == null)
            {
                throw new SomeCustomException("project_not_found");
            }


            //todo много запросов
            var statuses = (await _statusService.GetStatusesAsync(projectId)).Select(x => new WorkTaskStatusReturn(x)).ToList();
            var sprints = (await _sprintService.GetForProject(projectId)).Select(x => new ProjectSprintReturn(x)).ToList();
            var labels = (await _labelService.Get(projectId)).Select(x => new ProjectLabelReturn(x)).ToList();
            var presets = (await _presetService.GetAllWithLabelsAsync(projectId)).Select(x => new PresetReturn(x)).ToList();

            var users = await _projectUserService.GetProjectUsersAsync(projectId, userId);
            var usersReturn = users.Select(x => new ProjectUserReturn(x)).ToList();

            return new JsonResult(new ProjectFullInfoReturn() { Users = usersReturn, Statuses = statuses, Sprints = sprints, Labels = labels, Presets = presets }, GetJsonOptions());

        }




        [Route("delete-project")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultNewReturn>> DeleteProject([FromForm] long projectId)
        {
            var userId = User.GetUserId();
            var res = await _projectService.DeleteAsync(projectId, userId);
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
