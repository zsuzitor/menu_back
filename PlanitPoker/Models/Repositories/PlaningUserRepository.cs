using BO.Models.PlaningPoker.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using PlanitPoker.Models.Entity;
using PlanitPoker.Models.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Query;

namespace PlanitPoker.Models.Repositories
{
    public sealed class PlaningUserRepository : GeneralRepository<PlaningRoomUserDal, long>, IPlaningUserRepository
    {


        public PlaningUserRepository(MenuDbContext db) : base(db)
        {
        }

        public async Task<List<RoomShortInfo>> GetRoomsAsync(long userId)
        {
            var query = _db.PlaningRoomUsers.Where(x => x.MainAppUserId == userId)
                //.Select(x=>x.RoomId)
                //.AsQueryable()
                .Join(_db.PlaningRooms, x => x.RoomId, x => x.Id
                , (x, y) => new RoomShortInfo() { Name = y.Name, ImagePath = y.ImagePath });
            //var sql = query.ToSql();
            //var g = _db.PlaningRoomUsers.Where(x => x.MainAppUserId == userId);
            //g.joi
            return await query.ToListAsync();
            //.Select(x => new RoomShortInfo() { Name = x.Name, ImagePath = }).ToListAsync();
        }



    }
    public static class SqlScriptGetter
    {
        public static string ToSql<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            using var enumerator = query.Provider.Execute<IEnumerable<TEntity>>(query.Expression).GetEnumerator();
            var relationalCommandCache = enumerator.Private("_relationalCommandCache");
            var selectExpression = relationalCommandCache.Private<SelectExpression>("_selectExpression");
            var factory = relationalCommandCache.Private<IQuerySqlGeneratorFactory>("_querySqlGeneratorFactory");

            var sqlGenerator = factory.Create();
            var command = sqlGenerator.GetCommand(selectExpression);

            string sql = command.CommandText;
            return sql;
        }

        private static object Private(this object obj, string privateField) => obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);
        private static T Private<T>(this object obj, string privateField) => (T)obj?.GetType().GetField(privateField, BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(obj);

    }

}
