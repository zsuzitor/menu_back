using BL.Models.Services.Interfaces;
using Common.Models.Error.services.Interfaces;
using Common.Models.Return;
using jwtLib.JWTAuth.Interfaces;
using Menu.Models;
using Menu.Models.VaultApp.Returns;
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
        private readonly IHasher _hasher;



        public VaultController(IApiHelper apiHealper,
            ILogger<VaultController> logger, IJWTService jwtService,
            IErrorService errorService, IVaultService vaultService,
            IHasher hasher
        )
        {
            _apiHealper = apiHealper;
            _logger = logger;
            _errorService = errorService;
            _jwtService = jwtService;
            _vaultService = vaultService;
            _hasher = hasher;
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
                    var vaultAuthPassword = Request.Cookies[Constants.VaultAuthCookie];
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, false);

                    var res = await _vaultService.GetVaultWithSecretAsync(vaultId, userInfo, vaultAuthPassword);
                    var isAuth = false;
                    if (string.IsNullOrWhiteSpace(res.PasswordHash) 
                        || res.PasswordHash.Equals(_hasher.GetHash(vaultAuthPassword)))
                    {
                        isAuth = true;
                    }

                    var result = new SingleVaultReturn().Fill(res, null);
                    result.IsAuth = isAuth;
                    await _apiHealper.WriteResponseAsync(Response, result);

                }, Response, _logger);
        }

        [Route("get-vault-people")]
        [HttpGet]
        public async Task GetVaultPeople(long vaultId)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, false);

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
                    var vaultAuthPassword = Request.Cookies[Constants.VaultAuthCookie];
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    vault.Password = _hasher.GetHash(vault.Password);

                    var res = await _vaultService.UpdateVaultAsync(vault, userInfo, vaultAuthPassword);
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
                    vault.Password = _hasher.GetHash(vault.Password);

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

        [Route("change-password")]
        [HttpPatch]
        public async Task ChangePassword([FromForm] long vaultId, [FromForm] string password)
        {
            await _apiHealper.DoStandartSomething(
                async () =>
                {
                    var vaultAuthPassword = Request.Cookies[Constants.VaultAuthCookie];
                    var userInfo = _apiHealper.CheckAuthorized(Request, _jwtService, true);
                    var hashPass = _hasher.GetHash(password);
                    var res = await _vaultService.ChangePasswordAsync(vaultId, vaultAuthPassword, hashPass, userInfo);
                    if (res)
                    {
                        Response.Cookies.Append(Constants.VaultAuthCookie, hashPass, new CookieOptions() { HttpOnly = true });
                    }

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
                    var hashPass = _hasher.GetHash(password);
                    var res = await _vaultService.ExistVaultOrNullPasswordAsync(vaultId, hashPass, userInfo);
                    if (res)
                    {
                        Response.Cookies.Append(Constants.VaultAuthCookie, hashPass, new CookieOptions() { HttpOnly = true });
                    }
                    await _apiHealper.WriteResponseAsync(Response, new BoolResultReturn(res));
                }, Response, _logger);
        }
    }
}
