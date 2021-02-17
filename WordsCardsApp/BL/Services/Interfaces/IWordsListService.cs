using BO.Models.Auth;
using BO.Models.WordsCardsApp.DAL.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordsCardsApp.BO.Input;

namespace WordsCardsApp.BL.Services.Interfaces
{
    public interface IWordsListService
    {
        Task<WordsList> GetByIdIfAccess(long id, UserInfo userInfo);
        Task<List<WordsList>> GetAllForUser(UserInfo userInfo);
        Task<WordsList> Create(WordCardListInputModel input, UserInfo userInfo);
        Task<WordsList> Update(WordCardListInputModel input, UserInfo userInfo);
        Task<WordsList> Delete(long id, UserInfo userInfo);

    }
}
