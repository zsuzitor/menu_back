using BO.Models.Auth;
using BO.Models.VaultApp.Dal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VaultApp.Models.Entity;
using VaultApp.Models.Entity.Input;

namespace VaultApp.Models.Services
{
    public interface IVaultService
    {
        Task<List<Vault>> GetUserVaultsAsync(UserInfo userInfo);
        Task<Vault> GetVaultAsync(long vaultId, UserInfo userInfo);
        Task<Vault> GetVaultWithSecretAsync(long vaultId, UserInfo userInfo, string vaultPassword);
        Task<List<VaultUser>> GetUsersAsync(long vaultId, UserInfo userInfo);
        Task<Vault> UpdateVaultAsync(UpdateVault vault, UserInfo userInfo, string vaultPassword);
        Task<Vault> CreateVaultAsync(CreateVault vault, UserInfo userInfo);
        Task<bool> DeleteVaultAsync(long vaultId, UserInfo userInfo);
        Task<bool> ExistVaultAsync(long vaultId, string password, UserInfo userInfo);
        Task<bool> ExistVaultOrNullPasswordAsync(long vaultId, string password, UserInfo userInfo);
        Task HasAccessToVaultWithError(long vaultId, UserInfo userInfo);
        Task<bool> ChangePasswordAsync(long vaultId, string oldPassword, string newPassword, UserInfo userInfo);


    }
}
