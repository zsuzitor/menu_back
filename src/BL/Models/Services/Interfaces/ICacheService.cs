using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BL.Models.Services.Interfaces
{
    public interface ICacheService
    {
        bool Get<T>(string key, out T res);
        bool GetOrSet<T>(string key, out T res, Func<T> act, TimeSpan time, bool force = false);
        Task<(bool, T)> GetOrSetAsync<T>(string key, Func<Task<T>> act, TimeSpan time, bool force = false);
        bool Exist(string key);
        void Set<T>(string key, T value, TimeSpan time);
        void Remove(string key);
    }
}
