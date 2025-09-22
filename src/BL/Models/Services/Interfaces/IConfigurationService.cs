using BO.Models.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models.Services.Interfaces
{
    public interface IConfigurationService
    {
        Task AddIfNotExistAsync(string key, string value, string group, string type);
        Task<Configuration> GetAsync(string key);
    }
}
