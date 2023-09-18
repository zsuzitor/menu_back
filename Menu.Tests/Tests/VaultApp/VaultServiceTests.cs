using BL.Models.Services.Cache;
using BL.Models.Services.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Menu.Tests.Models;
using Menu.Tests.Models.Fake;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VaultApp.Models.Repositories.Implementation;
using VaultApp.Models.Services;
using VaultApp.Models.Services.Implementation;
using Xunit;

namespace Menu.Tests.Tests.VaultApp
{
    public class VaultServiceTests : TestBase
    {
        [Fact]
        public async Task CreateNewImageWithArticle_Created()
        {

            using var db = GetDbContext();
            var dbHelperMoq = GetDBHelper();
            var hasher = GetHasher();

            var coder = new FakeCoder();
            var service = new VaultService(new VaultRepository(db), dbHelperMoq, db
                , new UserRepository(db)
                , new CacheService(new FakeCacheAccessor())
                , hasher, coder, new SecretRepository(db));

            //Assert.NotNull(addedImg);
            //Assert.Equal(imgForAdd.Path, addedImg.Path);
            //Assert.Equal(imgForAdd.ArticleId, addedImg.ArticleId);

        }
    }
}
