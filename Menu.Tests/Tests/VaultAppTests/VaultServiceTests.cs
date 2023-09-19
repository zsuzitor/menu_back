using BL.Models.Services.Cache;
using BL.Models.Services.Interfaces;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Menu.Tests.Models;
using Menu.Tests.Models.Fake;
using Moq;
using System.Linq;
using System.Threading.Tasks;
using VaultApp.Models.Repositories.Implementation;
using VaultApp.Models.Services;
using VaultApp.Models.Services.Implementation;
using Xunit;

namespace Menu.Tests.Tests.VaultAppTests
{
    public class VaultServiceTests : TestBase
    {
        [Fact]
        public async Task CreateNewVault_Created()
        {
            var userId = 1;
            var (db, _, hasherMoq, _, service) = SetupVaultService();
            var vaultForCreate = new VaultApp.Models.Entity.Input.CreateVault()
            { Name = "name1", Password = "pwd1", IsPublic = true };
            var createdVault = await service.CreateVaultAsync(
                 vaultForCreate
                 , new BO.Models.Auth.UserInfo(userId));

            var createdVaultFromDb = db.Vaults.FirstOrDefault(x => x.Id == createdVault.Id);

            Assert.NotEqual(0, createdVault.Id);
            Assert.NotNull(createdVaultFromDb);
            Assert.Equal(vaultForCreate.Name, createdVaultFromDb.Name);
            Assert.Equal(vaultForCreate.IsPublic, createdVaultFromDb.IsPublic);
            Assert.Equal(hasherMoq.GetHash(vaultForCreate.Password), createdVaultFromDb.PasswordHash);
        }


        private (MenuDbContext, IDBHelper, IHasher, ICoder, IVaultService) SetupVaultService()
        {
            using var db = GetDbContext();
            var dbHelperMoq = GetDBHelper();
            var hasherMoq = GetHasher();
            var coder = new FakeCoder();
            var service = new VaultService(new VaultRepository(db), dbHelperMoq, db
                , new UserRepository(db)
                , new CacheService(new FakeCacheAccessor())
                , hasherMoq, coder, new SecretRepository(db));
            return (db, dbHelperMoq, hasherMoq, coder, service);
        }
    }
}
