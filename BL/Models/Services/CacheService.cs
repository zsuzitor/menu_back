using BL.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheAccessor _cacheAccessor;
        public CacheService(ICacheAccessor cacheAccessor)
        {
            _cacheAccessor = cacheAccessor;
        }

        public bool Exist(string key)
        {
            return _cacheAccessor.Exist(key);
        }

        public void Remove(string key)
        {
            _cacheAccessor.Remove(key);
        }

        public bool Get<T>(string key, out T res)
        {
            return _cacheAccessor.Get(key,out res);
        }

        public void Set<T>(string key, T value, TimeSpan time)
        {
            _cacheAccessor.Set(key, value, time);
        }

        public bool GetOrSet<T>(string key, out T res, Func<T> act, TimeSpan time)
        {
            if (Get(key, out res))
            {
                return true;
            }

            var result = act();
            Set(key, result, time);
            res = result;
            return true;
        }

        public async Task<(bool, T)> GetOrSet<T>(string key, Func<Task<T>> act, TimeSpan time)
        {
            if (Get(key, out T res))
            {
                return (true, res);
            }

            var result = await act();
            Set(key, result, time);
            
            return (true, result);
        }
    }
}
