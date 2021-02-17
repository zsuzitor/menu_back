

using BO.Models.WordsCardsApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsCardsApp.DAL.Repositories.Interfaces
{
   public  interface IWordsListRepository : IGeneralRepository<WordsList, long>
    {
        Task<WordsList> GetByIdIfAccess(long id, long userId);
        Task<List<WordsList>> GetAllForUser(long userId);
    }
}
