
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DAL.Models.DAL
{
    public interface IDBHelper
    {
        Task ActionInTransaction(MenuDbContext db, Func<Task> action);
    }

    public sealed class DBHelper : IDBHelper
    {
        private readonly IConfiguration _configuration;

        public DBHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        //private readonly MenuDbContext _db;
        //public DBHelper(MenuDbContext db)
        //{
        //    _db = db;
        //}

        public async Task ActionInTransaction(MenuDbContext db, Func<Task> action)
        {
            //в inmemory нет транзакций, это можно сделать отдельной реализацией но пока пусть так
            if (bool.Parse(_configuration["UseInMemoryDataProvider"]))
            {
                await action();
                return;
            }

            using (var tr = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    await action();
                    await tr.CommitAsync();
                }
                catch
                {
                    await tr.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
