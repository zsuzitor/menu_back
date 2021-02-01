using DAL.Models.DAL;
using Microsoft.EntityFrameworkCore;
using System;

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


    }
}
