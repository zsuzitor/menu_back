using BO.Models.Auth;
using BO.Models.VaultApp.Dal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VaultApp.Models.Entity.Input;

namespace VaultApp.Models.Services
{
    public interface IVaultService
    {
        Task<List<Vault>> GetUserVaultsAsync(UserInfo userInfo);
        Task<Vault> GetVaultAsync(long vaultId, UserInfo userInfo);
        Task<List<VaultUser>> GetPeopleAsync(long vaultId, UserInfo userInfo);
        Task<Vault> UpdateVaultAsync(UpdateVault vault, UserInfo userInfo);
        Task<Vault> CreateVaultAsync(CreateVault vault, UserInfo userInfo);
        Task<bool> DeleteVaultAsync(long vaultId, UserInfo userInfo);
        Task HasAccessToVaultWithError(long vaultId, UserInfo userInfo);

    }
}
