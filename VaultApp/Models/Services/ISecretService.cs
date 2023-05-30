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
        Task<bool> DeleteSecretAsync(long secretId, UserInfo userInfo);
        Task<Secret> UpdateSecretAsync(UpdateSecret secret, UserInfo userInfo, string passwordForCoded);
        Task<Secret> CreateSecretAsync(CreateSecret secret, UserInfo userInfo, string passwordForCoded);
        Task<Secret> GetSecretAsync(long secretId, UserInfo userInfo);
        Task<List<Secret>> GetSecretsAsync(long vaultId, UserInfo userInfo, string passwordForCoded);


        Task DeleteExpiredSecrets();

    }
}
