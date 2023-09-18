using BO.Models.VaultApp.Dal;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VaultApp.Models.Entity;

namespace VaultApp.Models.Repositories
{
    public interface IVaultRepository : IGeneralRepository<Vault, long>
    {
        Task<List<VaultUser>> GetUsersAsync(long vaultId);
        Task<List<VaultUserDal>> LoadUsersAsync(Vault vault);
        Task<List<Secret>> LoadSecretsAsync(Vault vault);
        Task<List<Vault>> GetFullListNoTrackAsync(long userId);
        //Task<List<Vault>> GetShortList(long userId);
        Task<bool> UserInVaultAsync(long vaultId, long userId);
        Task<bool> VaultIsPublicAsync(long vaultId);
        Task<bool> ExistVaultAsync(long vaultId, string passwordHash);
        Task<bool> ExistVaultOrNullPasswordAsync(long vaultId, string passwordHash);
        

    }
}
