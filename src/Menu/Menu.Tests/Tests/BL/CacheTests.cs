using BL.Models.Services.Cache;
using Menu.Tests.Models.Fake;
using System;
using Xunit;

namespace Menu.Tests.Tests.BL
{
    public class CacheTests
    {

        [Fact]
        public void CacheService_Set_Remove_Success()
        {
            var cacheService = new CacheService(new FakeCacheAccessor());
            var key = "test";
            var val = "test_val";
            cacheService.Set(key, val, TimeSpan.FromSeconds(10));
            var existedAfterAdding = cacheService.Exist(key);
            cacheService.Remove(key);
            var existedAfterDeleting = cacheService.Exist(key);


            Assert.True(existedAfterAdding);
            Assert.False(existedAfterDeleting);
        }

        [Fact]
        public void CacheService_GetOrSet_Setted()
        {
            var cacheService = new CacheService(new FakeCacheAccessor());
            var key = "test";
            int num = 10;

            cacheService.GetOrSet(key, out int res, () => num, TimeSpan.FromSeconds(10));
            var existedAfterAdding = cacheService.Get(key, out int numRes);

            Assert.Equal(num, numRes);
        }

        [Fact]
        public void CacheService_GetOrSet_NotSetted()
        {
            var cacheService = new CacheService(new FakeCacheAccessor());
            var key = "test";
            int num1 = 10;
            int num2 = 20;

            cacheService.Set(key, num1, TimeSpan.FromSeconds(10));
            cacheService.GetOrSet(key, out int res, () => num2, TimeSpan.FromSeconds(10));
            var existedAfterAdding = cacheService.Get(key, out int numRes);

            Assert.Equal(num1, numRes);
        }
    }
}
