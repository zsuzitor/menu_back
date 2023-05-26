using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Models.Services.Interfaces
{
    public interface ICacheAccessor
    {
        bool Get<T>(string key, out T result);
        void Set<T>(string key, T value, TimeSpan time);
        void Remove(string key);
        bool Exist(string key);
    }
}
