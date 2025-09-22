using BL.Models.Services.Interfaces;
using BO.Models.VaultApp.Dal;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
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
        private readonly IDateTimeProvider _dateTimeProvider;

        public SecretRepository(MenuDbContext db, IGeneralRepositoryStrategy repo, IDateTimeProvider dateTimeProvider) : base(db, repo)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task DeleteExpiredSecrets()
        {
            _db.Secrets.RemoveRange(_db.Secrets.Where(x => x.DieDate != null && x.DieDate < _dateTimeProvider.CurrentDateTime()));
            await _db.SaveChangesAsync();
        }

        //public async Task<List<Secret>> GetByVaultIdAsync(long vaultId)
        //{
        //    return await _db.Secrets.Where(x => x.VaultId == vaultId).ToListAsync();
        //}

        public async Task<List<Secret>> GetByVaultIdNoTrackAsync(long vaultId)
        {
            return await _db.Secrets.AsNoTracking().Where(x => x.VaultId == vaultId).ToListAsync();

        }

        public async Task<List<Secret>> GetCodedByVaultIdAsync(long vaultId)
        {
            return await _db.Secrets.Where(x => x.VaultId == vaultId && x.IsCoded).ToListAsync();

        }

        public async Task<long> GetVaultIdAsync(long secretId)
        {
            return await _db.Secrets.Where(x => x.Id == secretId).Select(x => x.VaultId).FirstOrDefaultAsync();
        }
    }
}
