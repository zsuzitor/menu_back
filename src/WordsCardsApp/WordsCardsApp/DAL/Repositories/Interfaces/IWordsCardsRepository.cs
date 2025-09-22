

using BO.Models.WordsCardsApp.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WordsCardsApp.DAL.Repositories.Interfaces
{
    public interface IWordsCardsRepository : IGeneralRepository<WordCard, long>
    {
        Task<WordCard> GetByIdIfAccessAsync(long id, long userId);
        Task<WordCard> GetByIdIfAccessNoTrackAsync(long id, long userId);

        Task<WordCard> DeleteAsync(long id, long userId);

        Task<List<WordCard>> GetAllUsersWordCardsAsync(long userId);

        Task<List<WordCard>> LoadWordListsIdAsync(List<WordCard> words);

        Task<bool?> ChangeHideStatusAsync(long id, long userId);

        
    }
}
