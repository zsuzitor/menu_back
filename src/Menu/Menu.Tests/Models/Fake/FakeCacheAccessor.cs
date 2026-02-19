using BL.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Menu.Tests.Models.Fake
{
    public class FakeCacheAccessor : ICacheAccessor
    {
        private Dictionary<string,object> _cache = new Dictionary<string,object>();

        public bool Exist(string key)
        {
            return _cache.ContainsKey(key);
        }

        public Task<bool> ExistAsync(string key)
        {
            return Task.FromResult(_cache.ContainsKey(key));
        }

        public bool Get<T>(string key, out T result)
        {
            result = default;
            if (Exist(key))
            {
                result = (T)_cache[key];
                return true;
            }

            return false;
        }

        public Task<(bool, T result)> GetAsync<T>(string key)
        {
            if (Exist(key))
            {
                var result = (T)_cache[key];
                return Task.FromResult((true, result));
            }

            return Task.FromResult((false, (T)default));
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }

        public void Set<T>(string key, T value, TimeSpan time)
        {
            _cache[key] = value;
        }

        public Task SetAsync<T>(string key, T value, TimeSpan time)
        {
            _cache[key] = value;
            return Task.CompletedTask;

        }
    }
}
