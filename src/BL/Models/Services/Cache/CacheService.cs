using BL.Models.Services.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace BL.Models.Services.Cache
{
    public class CacheService : ICacheService
    {
        private readonly ICacheAccessor _cacheAccessor;

        //лучше конечно чистить, но без гонки почистить сложно, нейронка говорит лучше оставить - на 1к ключей будет 1мб данных
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _locks = new();


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

        public async Task RemoveAsync(string key)
        {
            await _cacheAccessor.RemoveAsync(key);
        }

        public bool Get<T>(string key, out T res)
        {
            return _cacheAccessor.Get(key, out res);
        }

        public async Task<(bool, T)> GetAsync<T>(string key)
        {
            return await _cacheAccessor.GetAsync<T>(key);
        }

        public void Set<T>(string key, T value, TimeSpan time)
        {
            _cacheAccessor.Set(key, value, time);
        }

        public bool GetOrSet<T>(string key, out T res, Func<T> factory, TimeSpan time, bool force = false)
        {
            if (!force && Get(key, out res))
            {
                return true;
            }

            var semaphore = _locks.GetOrAdd(key, new SemaphoreSlim(1, 1));
            semaphore.Wait();
            try
            {
                if (!force && Get(key, out res))
                {
                    return true;
                }

                var result = factory();
                Set(key, result, time);
                res = result;
                return true;
            }
            finally
            {
                semaphore.Release();
            }

        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan time, bool force = false)
        {
            if (!force && Get(key, out T res))
            {
                return res;
            }

            var semaphore = _locks.GetOrAdd(key, new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();
            try
            {
                if (!force && Get(key, out res))
                {
                    return res;
                }

                var result = await factory();
                Set(key, result, time);
                return result;
            }
            finally
            {
                semaphore.Release();
            }
        }





        //если будет поднято несколько экземпляров, можно вот так через редис синхронизировать кеш
        //public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan time, bool force = false)
        //{
        //    if (!force && Get(key, out T res))
        //    {
        //        return res;
        //    }

        //    var lockKey = $"lock:{key}";
        //    var lockValue = Guid.NewGuid().ToString();
        //    var lockTimeout = TimeSpan.FromSeconds(5);

        //    // Пытаемся захватить блокировку в Redis
        //    var acquired = await _redisDatabase.LockTakeAsync(lockKey, lockValue, lockTimeout);

        //    if (acquired)
        //    {
        //        try
        //        {
        //            // Double-check
        //            if (!force && Get(key, out res))
        //            {
        //                return res;
        //            }

        //            var result = await factory();
        //            Set(key, result, time);
        //            return result;
        //        }
        //        finally
        //        {
        //            await _redisDatabase.LockReleaseAsync(lockKey, lockValue);
        //        }
        //    }
        //    else
        //    {
        //        // Ждем, пока другой поток заполнит кеш
        //        var delay = TimeSpan.FromMilliseconds(50);
        //        var maxAttempts = 10;

        //        for (int i = 0; i < maxAttempts; i++)
        //        {
        //            await Task.Delay(delay);
        //            if (Get(key, out res))
        //            {
        //                return res;
        //            }
        //        }

        //        // Таймаут — идем в БД (редкий случай)
        //        return await factory();
        //    }
        //}





    }
}
