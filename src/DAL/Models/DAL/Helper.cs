
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DAL.Models.DAL
{
    public interface IDBHelper
    {
        Task ActionInTransaction(MenuDbContext db, Func<Task> action);
        Task<T> ActionInTransaction<T>(MenuDbContext db, Func<Task<T>> action);
    }


    public sealed class InMemoryDBHelper : IDBHelper
    {
        public async Task ActionInTransaction(MenuDbContext db, Func<Task> action)
        {
            await action();
        }

        public async Task<T> ActionInTransaction<T>(MenuDbContext db, Func<Task<T>> action)
        {
            return await action();
        }
    }

    public sealed class DBHelper : IDBHelper
    {

        public DBHelper()
        {
        }
        //private readonly MenuDbContext _db;
        //public DBHelper(MenuDbContext db)
        //{
        //    _db = db;
        //}

        public async Task ActionInTransaction(MenuDbContext db, Func<Task> action)
        {
            var transaction = db.Database.CurrentTransaction;
            if (transaction == null)
            {
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
            else
            {
                await action();
            }
        }

        public async Task<T> ActionInTransaction<T>(MenuDbContext db, Func<Task<T>> action)
        {
            var transaction = db.Database.CurrentTransaction;
            if (transaction == null)
            {
                using (var tr = await db.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var res = await action();
                        await tr.CommitAsync();
                        return res;
                    }
                    catch
                    {
                        await tr.RollbackAsync();
                        throw;
                    }
                }
            }
            else
            {
                return await action();
            }
        }
    }
}
