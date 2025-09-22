using System;
using System.Threading.Tasks;

namespace BL.Models.Services.Interfaces
{
    public interface ICacheAccessor
    {
        bool Get<T>(string key, out T result);
        Task<(bool, T result)> GetAsync<T>(string key);
        void Set<T>(string key, T value, TimeSpan time);
        Task SetAsync<T>(string key, T value, TimeSpan time);
        void Remove(string key);
        Task RemoveAsync(string key);
        bool Exist(string key);
        Task<bool> ExistAsync(string key);
    }
}
