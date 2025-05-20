using BO.Models.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface IConfigurationRepository : IGeneralRepository<Configuration, long>
    {
        Task<bool> ExistsByKey(string key);
        Task<Configuration> GetByKey(string key);
    }
}
