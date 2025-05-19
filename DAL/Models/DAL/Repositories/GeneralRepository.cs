using BO.Models.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{
    public abstract class GeneralRepository<T1, T2> : IGeneralRepository<T1, T2> where T1 : class, IDomainRecord<T2>
    {
        protected readonly MenuDbContext _db;

        protected GeneralRepository(MenuDbContext db)
        {
            _db = db;
        }

        public virtual async Task<T1> AddAsync(T1 newRecord)
        {
            _db.Set<T1>().Add(newRecord);
            await _db.SaveChangesAsync();
            return newRecord;
        }

        public virtual async Task<IEnumerable<T1>> AddAsync(IEnumerable<T1> newRecords)
        {
            if (newRecords == null || newRecords.Count() == 0)
                return newRecords;
            _db.Set<T1>().AddRange(newRecords);
            await _db.SaveChangesAsync();
            return newRecords;
        }



        public virtual async Task<IEnumerable<T1>> DeleteAsync(IEnumerable<T1> records)
        {
            if (records == null || records.Count() == 0)
                return records;
            _db.Set<T1>().AttachRange(records);//?
            _db.Set<T1>().RemoveRange(records);
            await _db.SaveChangesAsync();
            return records;
        }

        public virtual async Task<T1> DeleteAsync(T1 record)
        {
            _db.Set<T1>().Attach(record);//?
            _db.Set<T1>().Remove(record);
            await _db.SaveChangesAsync();
            return record;
        }

        public virtual async Task<T1> DeleteAsync(T2 recordId)
        {
            var recordForDel = await GetAsync(recordId);
            if (recordForDel != null)
            {
                _db.Remove(recordForDel);
                await _db.SaveChangesAsync();
            }

            return recordForDel;
        }

        public virtual async Task<T1> GetAsync(T2 id)
        {
            return await _db.Set<T1>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<List<T1>> GetAsync(List<T2> ids)
        {
            return await _db.Set<T1>().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public virtual async Task<T1> GetNoTrackAsync(T2 id)
        {
            return await _db.Set<T1>().AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<List<T1>> GetNoTrackAsync(List<T2> ids)
        {
            return await _db.Set<T1>().AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public virtual async Task<bool> ExistAsync(T2 id)
        {
            return (await _db.Set<T1>().Where(x => x.Id.Equals(id)).Select(x => x.Id).FirstOrDefaultAsync()) != null;
        }

        public virtual async Task<T1> UpdateAsync(T1 record)
        {
            //_db.Set<T1>().Attach(record);
            await _db.SaveChangesAsync();
            return record;
        }

        public virtual async Task<IEnumerable<T1>> UpdateAsync(IEnumerable<T1> records)
        {
            if (records == null || records.Count() == 0)
                return records;
            //_db.Set<T1>().AttachRange(records);
            await _db.SaveChangesAsync();
            return records;
        }
    }
}
