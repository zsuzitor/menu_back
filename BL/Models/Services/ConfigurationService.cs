using BL.Models.Services.Interfaces;
using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    public class ConfigurationService : IConfigurationService
    {
        protected readonly IConfigurationRepository _configurationRepository;
        protected readonly ICacheService _cacheService;
        public ConfigurationService(IConfigurationRepository repo, ICacheService cahce)
        {
            _configurationRepository = repo;
            _cacheService = cahce;
        }

        public async Task AddIfNotExist(string key, string value)
        {
            if (await _configurationRepository.ExistsByKey(key))
            {
                return;
            }

            await _configurationRepository.AddAsync(new BO.Models.DAL.Domain.Configuration() { Key = key, Value = value, Group = "configuration" });
            _cacheService.Set(key, value, TimeSpan.FromHours(1));
        }

        public async Task<Configuration> Get(string key)
        {
            var res = await _cacheService.GetOrSet(key, async () =>
            {
                return await _configurationRepository.GetByKey(key);
            }, TimeSpan.FromHours(1));
            return res.Item2;
        }
    }
}
