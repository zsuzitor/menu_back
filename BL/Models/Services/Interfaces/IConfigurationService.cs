using BO.Models.DAL.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models.Services.Interfaces
{
    public interface IConfigurationService
    {
        Task AddIfNotExist(string key, string value);
        Task<Configuration> Get(string key);
    }
}
