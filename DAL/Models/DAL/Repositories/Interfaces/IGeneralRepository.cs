

using BO.Models.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface IGeneralRepository<TRecord, TId> where TRecord : class, IDomainRecord<TId>
    {
        Task<TRecord> AddAsync(TRecord newRecord);
        Task<List<TRecord>> AddAsync(List<TRecord> newRecords);

        Task<TRecord> UpdateAsync(TRecord record);
        Task<IEnumerable<TRecord>> UpdateAsync(IEnumerable<TRecord> records);

        Task<TRecord> GetAsync(TId id);
        Task<List<TRecord>> GetAsync(List<TId> ids);
        Task<TRecord> GetNoTrackAsync(TId id);
        Task<List<TRecord>> GetNoTrackAsync(List<TId> ids);
        Task<bool> ExistAsync(TId id);
        Task<List<TRecord>> DeleteAsync(List<TRecord> records);
        Task<TRecord> DeleteAsync(TRecord record);
        Task<TRecord> DeleteAsync(TId recordId);
    }
}
