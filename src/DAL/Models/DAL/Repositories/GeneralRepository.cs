using BO.Models.DAL;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{
   

    public abstract class GeneralRepository<TType, TId> : IGeneralRepository<TType, TId> where TType : class, IDomainRecord<TId>
    {
        protected readonly MenuDbContext _db;
        protected readonly IGeneralRepositoryStrategy _repo;

        protected GeneralRepository(MenuDbContext db, IGeneralRepositoryStrategy repo)
        {
            _db = db;
            _repo = repo;
        }

        public virtual async Task<TType> AddAsync(TType newRecord)
        {
            return await _repo.AddAsync(newRecord);
        }

        public virtual async Task<IEnumerable<TType>> AddAsync(IEnumerable<TType> newRecords)
        {
            return await _repo.AddAsync(newRecords);
        }



        public virtual async Task<IEnumerable<TType>> DeleteAsync(IEnumerable<TType> records)
        {
            return await _repo.DeleteAsync(records);
        }

        public virtual async Task<TType> DeleteAsync(TType record)
        {
            return await _repo.DeleteAsync(record);
        }

        public virtual async Task<TType> DeleteAsync(TId recordId)
        {
            return await _repo.DeleteAsync<TType,TId>(recordId);
        }

        public virtual async Task<TType> GetAsync(TId id)
        {
            return await _repo.GetAsync<TType,TId>(id);
        }

        public virtual async Task<List<TType>> GetAsync(List<TId> ids)
        {
            return await _repo.GetAsync<TType, TId>(ids);
        }

        public virtual async Task<TType> GetNoTrackAsync(TId id)
        {
            return await _repo.GetNoTrackAsync<TType, TId>(id);
        }

        public virtual async Task<List<TType>> GetNoTrackAsync(List<TId> ids)
        {
            return await _repo.GetNoTrackAsync<TType, TId>(ids);
        }

        public virtual async Task<bool> ExistAsync(TId id)
        {
            return await _repo.ExistAsync<TType, TId>(id);
        }

        public virtual async Task<TType> UpdateAsync(TType record)
        {
            return await _repo.UpdateAsync<TType>(record);
        }

        public virtual async Task<IEnumerable<TType>> UpdateAsync(IEnumerable<TType> records)
        {
            return await _repo.UpdateAsync<TType>(records);
        }
    }
}
