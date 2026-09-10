using BO.Models.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface IConfigurationRepository : IGeneralRepository<Configuration, long>
    {
        Task<bool> ExistsByKey(string key, DateTime? now);
        Task<Configuration> GetByKey(string key, DateTime? now);
        Task<Dictionary<string, Configuration>> GetAll(DateTime? now);
    }
}
