using BL.Models.Services.Interfaces;
using DAL.Models.DAL;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;

namespace Menu.Tests.Models
{
    public class TestBase
    {
        public MenuDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<MenuDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
            return new MenuDbContext(options);
        }

        public IDBHelper GetDBHelper()
        {
            var dbHelperMoq = new Mock<IDBHelper>();
            dbHelperMoq
                .Setup(x
                    => x.ActionInTransaction(It.IsAny<MenuDbContext>(), It.IsAny<Func<Task>>()))
                .Returns<MenuDbContext, Func<Task>>((db, f) => f());
            return dbHelperMoq.Object;
        }

        public IHasher GetHasher()
        {
            var hasher = new Mock<IHasher>();
            hasher.Setup(x => x.GetHash(It.IsAny<string>())).Returns<string>(s => $"__{s}__with_hash__");
            return hasher.Object;
        }


    }
}
