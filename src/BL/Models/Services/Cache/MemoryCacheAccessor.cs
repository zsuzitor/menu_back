using BL.Models.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace BL.Models.Services.Cache
{
    public class MemoryCacheAccessor : ICacheAccessor
    {
        private readonly IMemoryCache _memoryCache;//потокобезопасный
        public MemoryCacheAccessor(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool Get<T>(string key, out T result)
        {
            return _memoryCache.TryGetValue(key, out result);
        }

        public async Task<(bool, T)> GetAsync<T>(string key)
        {
            var res = Get(key, out T result);
            return await Task.FromResult((res, result));
        }

        public bool Exist(string key)
        {
             return _memoryCache.TryGetValue(key, out _);
        }
        
        public async Task<bool> ExistAsync(string key)
        {
            var res = Exist(key);
            return await Task.FromResult(res);
        }

        public void Set<T>(string key, T value, TimeSpan time)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(time)
                .SetAbsoluteExpiration(time);//todo может расширить потом
            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan time)
        {
            Set(key, value, time);
            await Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public async Task RemoveAsync(string key)
        {
            Remove(key);
            await Task.CompletedTask;
        }





        
    }
}
