using Auth.Models.Auth;
using Common.Models.Return;
using Menu.Host.Infrastructure;
using Menu.Models;
using Menu.Models.VaultApp.Returns;
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
    public class VaultSecretController : ControllerBase
    {
        private readonly ILogger _logger;

        private readonly ISecretService _secretService;


        public VaultSecretController(
            ILoggerFactory loggerFactory, ISecretService secretService
        )
        {
            _logger = loggerFactory.CreateLogger(Common.Models.Constants.Loggers.MenuApp);
            _secretService = secretService;
        }

        [Route("get-vault-secrets")]
        [HttpGet]
        [CustomAuthorize(withError: false)]
        public async Task<ActionResult<IEnumerable<SecretReturn>>> GetVaultSecrets(long vaultId)
        {
            var userId = User.GetUserId();
            var vaultAuthPassword = Request.Cookies[Constants.VaultAuthCookie];

            var res = (await _secretService.GetSecretsAsync(vaultId, userId, vaultAuthPassword))
                .Select(x => new SecretReturn().Fill(x));
            return new JsonResult(res, GetJsonOptions());
        }

        [Route("delete-secret")]
        [HttpDelete]
        [CustomAuthorize]
        public async Task<ActionResult<BoolResultReturn>> DeleteSecret([FromForm] long secretId)
        {
            var userId = User.GetUserId();

            var res = await _secretService.DeleteSecretAsync(secretId, userId);
            return new JsonResult(new BoolResultReturn(res), GetJsonOptions());
        }

        [Route("create-secret")]
        [HttpPut]
        [CustomAuthorize]
        public async Task<ActionResult<SecretReturn>> CreateSecret([FromForm] CreateSecret secret)
        {
            var userId = User.GetUserId();
            var vaultAuthPassword = Request.Cookies[Constants.VaultAuthCookie];

            var res = await _secretService.CreateSecretAsync(secret, userId, vaultAuthPassword);
            return new JsonResult(new SecretReturn().Fill(res), GetJsonOptions());
        }


        [Route("update-secret")]
        [HttpPatch]
        [CustomAuthorize]
        public async Task<ActionResult<SecretReturn>> UpdateSecret([FromForm] UpdateSecret secret)
        {
            var userId = User.GetUserId();
            var vaultAuthPassword = Request.Cookies[Constants.VaultAuthCookie];

            var res = await _secretService.UpdateSecretAsync(secret, userId, vaultAuthPassword);
            return new JsonResult(new SecretReturn().Fill(res), GetJsonOptions());
        }

        [Route("get-secret")]
        [HttpGet]
        [CustomAuthorize(withError: false)]
        public async Task<ActionResult<SecretReturn>> GetSecret(long secretId)
        {
            var userId = User.GetUserId();
            var vaultAuthPassword = Request.Cookies[Constants.VaultAuthCookie];

            var res = await _secretService.GetSecretAsync(secretId, userId, vaultAuthPassword);
            return new JsonResult(new SecretReturn().Fill(res), GetJsonOptions());
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
