using BO.Models.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{
    public abstract class GeneralRepository<T1, T2> : IGeneralRepository<T1, T2> where T1 : class,  IDomainRecord<T2>
    {
        private readonly MenuDbContext _db;

        public GeneralRepository(MenuDbContext db)
        {
            _db = db;
        }

        public async virtual Task<T1> Add(T1 newRecord)
        {
            _db.Set<T1>().Add(newRecord);
            await _db.SaveChangesAsync();
            return newRecord;
        }

        public async virtual Task<List<T1>> Add(List<T1> newRecords)
        {
            _db.Set<T1>().AddRange(newRecords);
            await _db.SaveChangesAsync();
            return newRecords;
        }

        public async virtual Task<List<T1>> Delete(List<T1> records)
        {
            _db.Set<T1>().AttachRange(records);
            _db.Set<T1>().RemoveRange(records);
            await _db.SaveChangesAsync();
            return records;
        }

        public async virtual Task<T1> Delete(T1 record)
        {
            _db.Set<T1>().Attach(record);
            _db.Set<T1>().Remove(record);
            await _db.SaveChangesAsync();
            return record;
        }

        public async virtual Task<T1> Get(T2 id)
        {
            return await _db.Set<T1>().FirstOrDefaultAsync(x=>x.Id.Equals(id));
        }

        public async virtual Task<List<T1>> Get(List<T2> ids)
        {
            return await _db.Set<T1>().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async virtual Task<T1> Update(T1 record)
        {
            _db.Set<T1>().Attach(record);
            await _db.SaveChangesAsync();
            return record;
        }
    }
}
