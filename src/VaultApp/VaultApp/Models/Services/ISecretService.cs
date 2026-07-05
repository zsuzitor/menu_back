using BO.Models.Auth;
using BO.Models.VaultApp.Dal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VaultApp.Models.Entity.Input;

namespace VaultApp.Models.Services
{
    public interface ISecretService
    {
        Task<bool> DeleteSecretAsync(long secretId, long userId);
        Task<Secret> UpdateSecretAsync(UpdateSecret secret, long userId, string passwordForCoded);
        Task<Secret> CreateSecretAsync(CreateSecret secret, long userId, string passwordForCoded);
        Task<Secret> GetSecretAsync(long secretId, long userId, string passwordForCoded);
        Task<List<Secret>> GetSecretsAsync(long vaultId, long userId, string passwordForCoded);


        Task DeleteExpiredSecrets();

    }
}
