using BL.Models.Services.Interfaces;
using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    public class ConfigurationService : IConfigurationService
    {
        protected readonly IConfigurationRepository _configurationRepository;
        protected readonly ICacheService _cacheService;

        private const string ConfigurationCacheKey = "confg_";
        public ConfigurationService(IConfigurationRepository repo, ICacheService cahce)
        {
            _configurationRepository = repo;
            _cacheService = cahce;
        }

        public async Task AddIfNotExistAsync(string key, string value, string group, string type, bool isPublic = false)
        {
            if (await _configurationRepository.ExistsByKey(key))
            {
                return;
            }

            await _configurationRepository.AddAsync(new BO.Models.DAL.Domain.Configuration()
            { Key = key, Value = value, Group = group, Type = type , Public = isPublic });
            _cacheService.Set(ConfigurationCacheKey + key, value, TimeSpan.FromHours(1));
        }

        public async Task<Configuration> GetAsync(string key)
        {
            var res = await _cacheService.GetOrSetAsync(ConfigurationCacheKey + key, async () =>
            {
                return await _configurationRepository.GetByKey(key);
            }, TimeSpan.FromHours(1));
            return res.Item2;
        }

        public async Task<Configuration> GetPublicAsync(string key)
        {
            var res = await _cacheService.GetOrSetAsync(ConfigurationCacheKey + key, async () =>
            {
                return await _configurationRepository.GetByKey(key);
            }, TimeSpan.FromHours(1));

            if(res.Item2 != null && res.Item2.Public)
            {
                return res.Item2;
            }

            return null;
        }
    }
}
