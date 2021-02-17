

using BO.Models.DAL;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface IGeneralRepository<T1,T2> where T1 : class, IDomainRecord<T2>
    {
        Task<T1> Add(T1 newRecord);
        Task<T1> Update(T1 record);
        Task<List<T1>> Add(List<T1> newRecords);

        Task<T1> Get(T2 id);
        Task<List<T1>> Get(List<T2> ids);
        Task<List<T1>> Delete(List<T1> records);
        Task<T1> Delete(T1 record);
    }
}
