using Common.Models.Error.services.Interfaces;
using jwtLib.JWTAuth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using VaultApp.Models.Entity.Input;
using VaultApp.Models.Services;
using WEB.Common.Models.Helpers.Interfaces;

namespace Menu.Controllers.VaultApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaultController : ControllerBase
    {
        private readonly IApiHelper _apiHealper;
        private readonly ILogger _logger;

        private readonly IJWTService _jwtService;
        private readonly IErrorService _errorService;

        private readonly IVaultService _vaultService;



        public VaultController(IApiHelper apiHealper,
            ILogger<VaultController> logger, IJWTService jwtService,
        IErrorService errorService, IVaultService vaultService
        )
        {
            _apiHealper = apiHealper;
            _logger = logger;
            _errorService = errorService;
            _jwtService = jwtService;
            _vaultService = vaultService;
        }

        [Route("get-my-vaults")]
        [HttpGet]
        public async Task GetMyVaults()
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _vaultService.GetUserVaultsAsync(userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res);

                }, Response, _logger);
        }



        [Route("get-vault")]
        [HttpGet]
        public async Task GetVault(long vaultId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _vaultService.GetVaultAsync(vaultId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res);

                }, Response, _logger);
        }

        [Route("get-vault-people")]
        [HttpGet]
        public async Task GetVaultPeople(long vaultId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _vaultService.GetPeopleAsync(vaultId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res);

                }, Response, _logger);
        }

        [Route("update-vault")]
        [HttpPatch]
        public async Task UpdateVault([FromForm] UpdateVault vault)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _vaultService.UpdateVaultAsync(vault, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res);

                }, Response, _logger);
        }

        [Route("create-vault")]
        [HttpPatch]
        public async Task CreateVault([FromForm] CreateVault vault)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _vaultService.CreateVaultAsync(vault, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res);

                }, Response, _logger);
        }

        [Route("delete-vault")]
        [HttpDelete]
        public async Task DeleteVault([FromForm] long vaultId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _vaultService.DeleteVaultAsync(vaultId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, res);

                }, Response, _logger);
        }
    }
}
