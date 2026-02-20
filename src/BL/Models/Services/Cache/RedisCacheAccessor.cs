using BL.Models.Services.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace BL.Models.Services.Cache
{
    public class RedisCacheAccessor : ICacheAccessor
    {
        private readonly IDistributedCache _distributedCache;//потокобезопасный
        public RedisCacheAccessor(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }


        public bool Exist(string key)
        {
            var str = _distributedCache.GetString(key);
            return !string.IsNullOrEmpty(str);

        }

        public async Task<bool> ExistAsync(string key)
        {
            var str = await _distributedCache.GetStringAsync(key);
            return !string.IsNullOrEmpty(str);
        }

        public bool Get<T>(string key, out T result)
        {
            var resStr = _distributedCache.GetString(key);
            if (string.IsNullOrEmpty(resStr))
            {
                result = default(T);
                return false;
            }

            result = JsonSerializer.Deserialize<T>(resStr);
            return true;
        }

        public async Task<(bool, T result)> GetAsync<T>(string key)
        {
            var resStr = await _distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(resStr))
            {
                return (false, default(T));
            }

            var result = JsonSerializer.Deserialize<T>(resStr);
            return (true, result);
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _distributedCache.RemoveAsync(key);

        }

        public void Set<T>(string key, T value, TimeSpan time)
        {
            var val = JsonSerializer.Serialize(value);
            _distributedCache.SetString(key, val, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = time,
            });
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan time)
        {
            var val = JsonSerializer.Serialize(value);
            await _distributedCache.SetStringAsync(key, val, new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = time,
            });
        }
    }
}
