using BL.Models.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Models.Services
{
    public class MemoryCacheAccessor : ICacheAccessor
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheAccessor(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool Get<T>(string key, out T result)
        {
            return _memoryCache.TryGetValue(key, out result);
        }

        public bool Exist(string key)
        {
             return _memoryCache.TryGetValue(key, out _);
        }

        public void Set<T>(string key, T value, TimeSpan time)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(time)
                .SetAbsoluteExpiration(time);//todo может расширить потом
            _memoryCache.Set(key, value, cacheEntryOptions);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
