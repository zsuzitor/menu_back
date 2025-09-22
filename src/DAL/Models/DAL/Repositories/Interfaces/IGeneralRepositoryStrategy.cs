using BO.Models.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface IGeneralRepositoryStrategy
    {
        Task<TType> AddAsync<TType>(TType newRecord) where TType : class, IDomainRecord;
        Task<IEnumerable<TType>> AddAsync<TType>(IEnumerable<TType> newRecords) where TType : class, IDomainRecord;

        Task<TType> UpdateAsync<TType>(TType record) where TType : class, IDomainRecord;
        Task<IEnumerable<TType>> UpdateAsync<TType>(IEnumerable<TType> records) where TType : class, IDomainRecord;

        Task<TType> GetAsync<TType, TId>(TId id) where TType : class, IDomainRecord<TId>;
        Task<List<TType>> GetAsync<TType, TId>(List<TId> ids) where TType : class, IDomainRecord<TId>;
        Task<TType> GetNoTrackAsync<TType, TId>(TId id) where TType : class, IDomainRecord<TId>;
        Task<List<TType>> GetNoTrackAsync<TType, TId>(List<TId> ids) where TType : class, IDomainRecord<TId>;
        Task<bool> ExistAsync<TType, TId>(TId id) where TType : class, IDomainRecord<TId>;
        Task<IEnumerable<TType>> DeleteAsync<TType>(IEnumerable<TType> records) where TType : class, IDomainRecord;
        Task<TType> DeleteAsync<TType>(TType record) where TType : class, IDomainRecord;
        Task<TType> DeleteAsync<TType, TId>(TId recordId) where TType : class, IDomainRecord<TId>;
    }
}
