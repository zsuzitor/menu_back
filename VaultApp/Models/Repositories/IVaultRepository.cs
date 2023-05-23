using BO.Models.VaultApp.Dal;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VaultApp.Models.Repositories
{
    internal interface IVaultRepository : IGeneralRepository<Vault, long>
    {
        Task<List<VaultUser>> GetUsers(long vaultId);
        Task<List<VaultUser>> LoadUsers(Vault vault);
        Task<List<Vault>> GetList(long userId);
        Task<bool> UserInVaultAsync(long vaultId, long userId);
    }
}
