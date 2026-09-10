using BL.Models.Services.Interfaces;
using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ICacheService _cacheService;
        private readonly IDateTimeProvider _dateTimeProvider;

        private const string ConfigurationCacheKey = "AppConfigurations";
        public ConfigurationService(IConfigurationRepository repo, ICacheService cahce, IDateTimeProvider dateTimeProvider)
        {
            _configurationRepository = repo;
            _cacheService = cahce;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task AddIfNotExistAsync(string key, string value, string group, string type, bool isPublic = false)
        {
            var now = _dateTimeProvider.CurrentDateTime();
            if (await _configurationRepository.ExistsByKey(key, now))
            {
                return;
            }

            await _configurationRepository.AddAsync(new BO.Models.DAL.Domain.Configuration()
            { Key = key, Value = value, Group = group, Type = type, Public = isPublic , Start = now });
            //_cacheService.Set(ConfigurationCacheKey + key, value, TimeSpan.FromHours(1));
            _cacheService.Remove(ConfigurationCacheKey);
        }

        public async Task<Configuration> GetAsync(string key)
        {
            var now = _dateTimeProvider.CurrentDateTime();
            var res = await _cacheService.GetOrSetAsync(ConfigurationCacheKey, async () =>
            {
                return await _configurationRepository.GetAll(now);
            }, TimeSpan.FromHours(1));

            if (res.TryGetValue(key, out var elem) && elem != null
            && (elem.Start <= now && (elem.End == null || elem.End >= now))
            )
            {
                return elem;
            }

            return null;
        }

        public async Task<Configuration> GetPublicAsync(string key)
        {
            var now = _dateTimeProvider.CurrentDateTime();
            var res = await _cacheService.GetOrSetAsync(ConfigurationCacheKey, async () =>
            {
                return await _configurationRepository.GetAll(now);
            }, TimeSpan.FromHours(1));

            if (res.TryGetValue(key, out var elem) && elem != null && elem.Public
            && (elem.Start <= now && (elem.End == null || elem.End >= now))
            )
            {
                return elem;
            }

            return null;
        }
    }
}
