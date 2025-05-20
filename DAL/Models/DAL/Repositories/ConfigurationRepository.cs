using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{
    public class ConfigurationRepository : GeneralRepository<Configuration, long>, IConfigurationRepository
    {
        public ConfigurationRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {

        }

        public async Task<bool> ExistsByKey(string key)
        {
            return await _db.Configurations.Where(x => x.Key == key).AnyAsync();
        }

        public async Task<Configuration> GetByKey(string key)
        {
            return await _db.Configurations.Where(x => x.Key == key).FirstOrDefaultAsync();

        }
    }
}
