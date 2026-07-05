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
        Task<List<Vault>> GetUserVaultsAsync(long userId);
        Task<Vault> GetVaultAsync(long vaultId, long userId);
        Task<Vault> GetVaultWithSecretAsync(long vaultId, long userId, string vaultPassword);
        Task<List<VaultUser>> GetUsersAsync(long vaultId, long userId);
        Task<Vault> UpdateVaultAsync(UpdateVault vault, long userId, string vaultPassword);
        Task<Vault> CreateVaultAsync(CreateVault vault, long userId);
        Task<bool> DeleteVaultAsync(long vaultId, long userId);
        Task<bool> ExistVaultAsync(long vaultId, string password, long userId);
        Task<bool> ExistVaultOrNullPasswordAsync(long vaultId, string password, long userId);
        Task HasAccessToVaultWithError(long vaultId, long userId);
        Task HasAccessToReadVaultWithError(long vaultId, long userId);
        Task<bool> ChangePasswordAsync(long vaultId, string oldPassword, string newPassword, long userId);


    }
}
