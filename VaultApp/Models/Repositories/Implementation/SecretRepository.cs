using BO.Models.VaultApp.Dal;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaultApp.Models.Repositories.Implementation
{
    public sealed class SecretRepository : GeneralRepository<Secret, long>, ISecretRepository
    {
        public SecretRepository(MenuDbContext db) : base(db)
        {

        }

        public async Task DeleteExpiredSecrets()
        {
            _db.Secrets.RemoveRange(_db.Secrets.Where(x => x.DieDate < DateTime.Now));
            await _db.SaveChangesAsync();

        }

        public async Task<List<Secret>> GetByVaultIdAsync(long vaultId)
        {
            return await _db.Secrets.Where(x => x.VaultId == vaultId).ToListAsync();
        }
    }
}
