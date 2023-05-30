using BO.Models.VaultApp.Dal;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaultApp.Models.Entity;

namespace VaultApp.Models.Repositories.Implementation
{
    internal sealed class VaultRepository : GeneralRepository<Vault, long>, IVaultRepository
    {
        public VaultRepository(MenuDbContext db) : base(db)
        {

        }

        public async Task<List<Vault>> GetFullListNoTrackAsync(long userId)
        {
            return await _db.VaultUsers.AsNoTracking().Where(x => x.UserId == userId)
                .Join(_db.Vaults, (x1 => x1.VaultId), (x2 => x2.Id), (x3, x4) => x4).ToListAsync();
        }

        public async Task<List<VaultUser>> GetUsersAsync(long vaultId)
        {
            return await _db.VaultUsers.Where(x => x.VaultId == vaultId)
                .Join(_db.Users, x1 => x1.UserId, x2 => x2.Id,
                (x3, x4) => new VaultUser() { Id = x3.Id, UserId = x3.UserId, Email = x4.Email }).ToListAsync();
        }

        public async Task<List<VaultUserDal>> LoadUsersAsync(Vault vault)
        {
            await _db.Entry(vault).Collection(x => x.Users).LoadAsync();
            return vault.Users;
        }

        public async Task<List<Secret>> LoadSecretsAsync(Vault vault)
        {
            await _db.Entry(vault).Collection(x => x.Secrets).LoadAsync();
            return vault.Secrets;
        }

        public async Task<bool> UserInVaultAsync(long vaultId, long userId)
        {
            return (await _db.VaultUsers.Where(x => x.VaultId == vaultId && x.UserId == userId)
                .Select(x => x.Id).FirstOrDefaultAsync()) != 0;
        }

        public async Task<bool> ExistVaultAsync(long vaultId, string passwordHash)
        {
            return (await _db.Vaults.AsNoTracking().Where(x => x.Id == vaultId && x.PasswordHash == passwordHash)
                 .Select(x => x.Id).FirstOrDefaultAsync()) != 0;
        }
    }
}
