using Common.Models.Error.services.Interfaces;
using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models.Returns.Types.VaultApp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
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

                    var res = (await _vaultService.GetUserVaultsAsync(userInfo))
                        .Select(x => new VaultInListReturn().Fill(x));
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

                    var res = await _vaultService.GetVaultWithSecretAsync(vaultId, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new SingleVaultReturn().Fill(res, null));

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

                    var res = (await _vaultService.GetUsersAsync(vaultId, userInfo))
                        .Select(x => new VaultUserReturn().Fill(x));
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
                    var users = await _vaultService.GetUsersAsync(res.Id, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new SingleVaultReturn().Fill(res, users));

                }, Response, _logger);
        }

        [Route("create-vault")]
        [HttpPut]
        public async Task CreateVault([FromForm] CreateVault vault)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _vaultService.CreateVaultAsync(vault, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new SingleVaultReturn().Fill(res, null));

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
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultReturn(res));

                }, Response, _logger);
        }

        [Route("authorize")]
        [HttpPost]
        public async Task AuthorizeVault([FromForm] long vaultId, [FromForm] string password)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);

                    var res = await _vaultService.ExistVaultAsync(vaultId, password, userInfo);
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultReturn(res));
                    Response.Cookies.Append("Auth_Vault"+ vaultId, password, new CookieOptions() { HttpOnly = true });
                }, Response, _logger);
        }
    }
}
