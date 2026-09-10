using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{
    public class ConfigurationRepository : GeneralRepository<Configuration, long>, IConfigurationRepository
    {
        public ConfigurationRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {

        }

        public async Task<bool> ExistsByKey(string key, DateTime? now)
        {
            return await _db.Configurations.Where(x => x.Key == key
            && ((now == null) || (x.Start <= now && (x.End == null || x.End >= now)))
            ).AnyAsync();
        }

        public async Task<Dictionary<string, Configuration>> GetAll(DateTime? now)
        {
            return (await _db.Configurations
                .Where(x=>
                    ((now == null) || (x.Start <= now && (x.End == null || x.End >= now))))
                .ToListAsync()).ToDictionary(x => x.Key);
        }

        public async Task<Configuration> GetByKey(string key, DateTime? now)
        {
            return await _db.Configurations.Where(x => x.Key == key
                && ((now == null) || (x.Start <= now && (x.End == null || x.End >= now)))
            ).FirstOrDefaultAsync();

        }
    }
}
