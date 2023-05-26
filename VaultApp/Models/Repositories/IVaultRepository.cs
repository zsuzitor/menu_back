using BO.Models.VaultApp.Dal;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VaultApp.Models.Entity;

namespace VaultApp.Models.Repositories
{
    internal interface IVaultRepository : IGeneralRepository<Vault, long>
    {
        Task<List<VaultUser>> GetUsers(long vaultId);
        Task<List<VaultUserDal>> LoadUsers(Vault vault);
        Task<List<Secret>> LoadSecrets(Vault vault);
        Task<List<Vault>> GetFullList(long userId);
        //Task<List<Vault>> GetShortList(long userId);
        Task<bool> UserInVaultAsync(long vaultId, long userId);
        Task<bool> ExistVaultAsync(long vaultId, string passwordHash);

    }
}
