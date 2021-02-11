

using BO.Models.WordsCardsApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsCardsApp.DAL.Repositories.Interfaces
{
    public interface IWordsCardsRepository
    {
        Task<WordCard> GetByIdIfAccess(long id, long userId);

        Task<WordCard> Create(WordCard newData);
        Task Edit(WordCard newData);
        Task<List<WordCard>> CreateList(List<WordCard> newArr);
        Task<WordCard> Delete(long id, long userId);

        Task<List<WordCard>> GetAllUsersWordCards(long userId);
    }
}
