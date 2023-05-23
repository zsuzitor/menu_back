using BO.Models.VaultApp.Dal;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VaultApp.Models.Repositories
{
    internal interface ISecretRepository : IGeneralRepository<Secret, long>
    {
        Task DeleteExpiredSecrets();
        Task<List<Secret>> GetByVaultIdAsync(long vaultId);
    }
}
