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

        private const string ConfigurationCacheKey = "AppConfigurations";
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
            { Key = key, Value = value, Group = group, Type = type, Public = isPublic });
            //_cacheService.Set(ConfigurationCacheKey + key, value, TimeSpan.FromHours(1));
            _cacheService.Remove(ConfigurationCacheKey);
        }

        public async Task<Configuration> GetAsync(string key)
        {
            var res = await _cacheService.GetOrSetAsync(ConfigurationCacheKey, async () =>
            {
                return await _configurationRepository.GetAll();
            }, TimeSpan.FromHours(1));

            if (res.TryGetValue(key, out var elem) && elem != null)
            {
                return elem;
            }

            return null;
        }

        public async Task<Configuration> GetPublicAsync(string key)
        {
            var res = await _cacheService.GetOrSetAsync(ConfigurationCacheKey, async () =>
            {
                return await _configurationRepository.GetAll();
            }, TimeSpan.FromHours(1));

            if (res.TryGetValue(key, out var elem) && elem != null && elem.Public)
            {
                return elem;
            }

            return null;
        }
    }
}
