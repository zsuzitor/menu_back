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
    internal sealed class VaultRepository : GeneralRepository<Vault, long>, IVaultRepository
    {
        public VaultRepository(MenuDbContext db) : base(db)
        {

        }

        public async Task<List<Vault>> GetList(long userId)
        {
            return await _db.VaultUsers.Where(x => x.UserId == userId)
                .Join(_db.Vaults, (x1 => x1.VaultId), (x2 => x2.Id), (x3, x4) => x4).ToListAsync();
        }

        public async Task<List<VaultUser>> GetUsers(long vaultId)
        {
            return await _db.VaultUsers.Where(x => x.VaultId == vaultId).ToListAsync();
        }

        public async Task<List<VaultUser>> LoadUsers(Vault vault)
        {
            await _db.Entry(vault).Collection(x => x.Users).LoadAsync();
            return vault.Users;
        }

        public async Task<bool> UserInVaultAsync(long vaultId, long userId)
        {
            return (await _db.VaultUsers.Where(x => x.VaultId == vaultId && x.UserId == userId)
                .Select(x => x.Id).FirstOrDefaultAsync()) != 0;

        }
    }
}
