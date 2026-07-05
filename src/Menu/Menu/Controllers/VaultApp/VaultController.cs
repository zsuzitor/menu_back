using Auth.Models.Auth;
using BL.Models.Services.Interfaces;
using Common.Models.Return;
using Menu.Host.Infrastructure;
using Menu.Models;
using Menu.Models.VaultApp.Returns;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using VaultApp.Models.Entity.Input;
using VaultApp.Models.Services;


namespace Menu.Controllers.VaultApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaultController : ControllerBase
    {
        private readonly ILogger _logger;

        private readonly IVaultService _vaultService;
        private readonly IHasher _hasher;



        public VaultController(
            ILoggerFactory loggerFactory, IVaultService vaultService,
            IHasher hasher
        )
        {
            _logger = loggerFactory.CreateLogger(Common.Models.Constants.Loggers.MenuApp);
            _vaultService = vaultService;
            _hasher = hasher;
        }

        [Route("get-my-vaults")]
        [HttpGet]
        [CustomAuthorize]
        public async Task<ActionResult<IEnumerable<VaultInListReturn>>> GetMyVaults()
        {
            var userId = User.GetUserId();
            var res = (await _vaultService.GetUserVaultsAsync(userId))
                .Select(x => new VaultInListReturn().Fill(x));
            return new JsonResult(res, GetJsonOptions());
            
        }



        [Route("get-vault")]
        [HttpGet]
        [CustomAuthorize(withError:false)]
        public async Task<ActionResult<SingleVaultReturn>> GetVault(long vaultId)
        {
            var userId = User.GetUserId();
            var vaultAuthPassword = Request.Cookies[Constants.VaultAuthCookie];

            var res = await _vaultService.GetVaultWithSecretAsync(vaultId, userId, vaultAuthPassword);
            var isAuth = false;
            if (string.IsNullOrWhiteSpace(res.PasswordHash)
                || res.PasswordHash.Equals(_hasher.GetHash(vaultAuthPassword)))
            {
                isAuth = true;
            }

            var result = new SingleVaultReturn().Fill(res, null);
            result.IsAuth = isAuth;
            return new JsonResult(result, GetJsonOptions());
        }

        [Route("get-vault-people")]
        [HttpGet]
        [CustomAuthorize(withError: false)]
        public async Task<ActionResult<IEnumerable<VaultUserReturn>>> GetVaultPeople(long vaultId)
        {
            var userId = User.GetUserId();
            var res = (await _vaultService.GetUsersAsync(vaultId, userId))
                .Select(x => new VaultUserReturn().Fill(x));
            return new JsonResult(res, GetJsonOptions());
        }

        [Route("update-vault")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<SingleVaultReturn>> UpdateVault([FromForm] UpdateVault vault)
        {
            var userId = User.GetUserId();
            var vaultAuthPassword = Request.Cookies[Constants.VaultAuthCookie];
            vault.Password = _hasher.GetHash(vault.Password);

            var res = await _vaultService.UpdateVaultAsync(vault, userId, vaultAuthPassword);
            var users = await _vaultService.GetUsersAsync(res.Id, userId);
            return new JsonResult(new SingleVaultReturn().Fill(res, users), GetJsonOptions());
        }

        [Route("create-vault")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<SingleVaultReturn>> CreateVault([FromForm] CreateVault vault)
        {
            var userId = User.GetUserId();
            vault.Password = _hasher.GetHash(vault.Password);

            var res = await _vaultService.CreateVaultAsync(vault, userId);
            return new JsonResult(new SingleVaultReturn().Fill(res, null), GetJsonOptions());
        }

        [Route("delete-vault")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultReturn>> DeleteVault([FromForm] long vaultId)
        {
            var userId = User.GetUserId();
            var res = await _vaultService.DeleteVaultAsync(vaultId, userId);

            return new JsonResult(new BoolResultReturn(res), GetJsonOptions());
        }

        [Route("change-password")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultReturn>> ChangePassword([FromForm] long vaultId, [FromForm] string password)
        {
            var userId = User.GetUserId();
            var vaultAuthPassword = Request.Cookies[Constants.VaultAuthCookie];
            var hashPass = _hasher.GetHash(password);
            var res = await _vaultService.ChangePasswordAsync(vaultId, vaultAuthPassword, hashPass, userId);
            if (res)
            {
                Response.Cookies.Append(Constants.VaultAuthCookie, hashPass, new CookieOptions() { HttpOnly = true });
            }

            return new JsonResult(new BoolResultReturn(res), GetJsonOptions());
        }

        [Route("authorize")]
        [HttpPost]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultReturn>> AuthorizeVault([FromForm] long vaultId, [FromForm] string password)
        {
            var userId = User.GetUserId();
            var hashPass = _hasher.GetHash(password);
            var res = await _vaultService.ExistVaultOrNullPasswordAsync(vaultId, hashPass, userId);
            if (res)
            {
                Response.Cookies.Append(Constants.VaultAuthCookie, hashPass, new CookieOptions() { HttpOnly = true });
            }
            return new JsonResult(new BoolResultReturn(res), GetJsonOptions());
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
