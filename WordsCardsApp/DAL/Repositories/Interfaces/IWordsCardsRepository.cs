

using BO.Models.WordsCardsApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsCardsApp.DAL.Repositories.Interfaces
{
    public interface IWordsCardsRepository : IGeneralRepository<WordCard, long>
    {
        Task<WordCard> GetByIdIfAccess(long id, long userId);

        Task<WordCard> Delete(long id, long userId);

        Task<List<WordCard>> GetAllUsersWordCards(long userId);

        Task<bool?> ChangeHideStatus(long id, long userId);
    }
}
