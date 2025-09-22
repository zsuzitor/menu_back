using BO.Models.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{

    public class GeneralRepositoryStrategy : IGeneralRepositoryStrategy
    {
        protected readonly MenuDbContext _db;

        public GeneralRepositoryStrategy(MenuDbContext db)
        {
            _db = db;
        }

        public async Task<TType> AddAsync<TType>(TType newRecord) where TType : class, IDomainRecord
        {
            _db.Set<TType>().Add(newRecord);
            await _db.SaveChangesAsync();
            return newRecord;
        }


        public virtual async Task<IEnumerable<TType>> AddAsync<TType>(IEnumerable<TType> newRecords) where TType : class, IDomainRecord
        {
            if (newRecords == null || newRecords.Count() == 0)
                return newRecords;
            _db.Set<TType>().AddRange(newRecords);
            await _db.SaveChangesAsync();
            return newRecords;
        }



        public virtual async Task<IEnumerable<TType>> DeleteAsync<TType>(IEnumerable<TType> records) where TType : class, IDomainRecord
        {
            if (records == null || records.Count() == 0)
                return records;
            _db.Set<TType>().AttachRange(records);//?
            _db.Set<TType>().RemoveRange(records);
            await _db.SaveChangesAsync();
            return records;
        }

        public virtual async Task<TType> DeleteAsync<TType>(TType record) where TType : class, IDomainRecord
        {
            _db.Set<TType>().Attach(record);//?
            _db.Set<TType>().Remove(record);
            await _db.SaveChangesAsync();
            return record;
        }

        public virtual async Task<TType> DeleteAsync<TType, TId>(TId recordId) where TType : class, IDomainRecord<TId>
        {
            var recordForDel = await GetAsync<TType, TId>(recordId);
            if (recordForDel != null)
            {
                _db.Remove(recordForDel);
                await _db.SaveChangesAsync();
            }

            return recordForDel;
        }

        public virtual async Task<TType> GetAsync<TType, TId>(TId id) where TType : class, IDomainRecord<TId>
        {
            return await _db.Set<TType>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<List<TType>> GetAsync<TType, TId>(List<TId> ids) where TType : class, IDomainRecord<TId>
        {
            return await _db.Set<TType>().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public virtual async Task<TType> GetNoTrackAsync<TType, TId>(TId id) where TType : class, IDomainRecord<TId>
        {
            return await _db.Set<TType>().AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public virtual async Task<List<TType>> GetNoTrackAsync<TType, TId>(List<TId> ids) where TType : class, IDomainRecord<TId>
        {
            return await _db.Set<TType>().AsNoTracking().Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public virtual async Task<bool> ExistAsync<TType, TId>(TId id) where TType : class, IDomainRecord<TId>
        {
            return (await _db.Set<TType>().Where(x => x.Id.Equals(id)).Select(x => x.Id).FirstOrDefaultAsync()) != null;
        }

        public virtual async Task<TType> UpdateAsync<TType>(TType record) where TType : class, IDomainRecord
        {
            await _db.SaveChangesAsync();
            return record;
        }

        public virtual async Task<IEnumerable<TType>> UpdateAsync<TType>(IEnumerable<TType> records) where TType : class, IDomainRecord
        {
            if (records == null || records.Count() == 0)
                return records;
            //_db.Set<TType>().AttachRange(records);
            await _db.SaveChangesAsync();
            return records;
        }




    }
}
