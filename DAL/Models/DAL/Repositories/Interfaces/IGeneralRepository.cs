

using BO.Models.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface IGeneralRepository<T1,T2> where T1 : class, IDomainRecord<T2>
    {
        Task<T1> AddAsync(T1 newRecord);
        Task<List<T1>> AddAsync(List<T1> newRecords);

        Task<T1> UpdateAsync(T1 record);
        Task<IEnumerable<T1>> UpdateAsync(IEnumerable<T1> records);

        Task<T1> GetAsync(T2 id);
        Task<List<T1>> GetAsync(List<T2> ids);
        Task<T1> GetNoTrackAsync(T2 id);
        Task<List<T1>> GetNoTrackAsync(List<T2> ids);
        Task<bool> ExistAsync(T2 id);
        Task<List<T1>> DeleteAsync(List<T1> records);
        Task<T1> DeleteAsync(T1 record);
        Task<T1> DeleteAsync(T2 recordId);
    }
}
