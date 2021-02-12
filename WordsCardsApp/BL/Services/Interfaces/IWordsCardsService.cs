

using BO.Models.Auth;
using BO.Models.WordsCardsApp.DAL.Domain;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using WordsCardsApp.BO.Input;

namespace WordsCardsApp.BL.Services.Interfaces
{
    public interface IWordsCardsService
    {
        Task<WordCard> GetByIdIfAccess(long id, UserInfo userInfo);
        Task<List<WordCard>> GetAllForUser(UserInfo userInfo);
        Task<WordCard> Create(WordCardInputModel input, UserInfo userInfo );
        Task<WordCard> Update(WordCardInputModel input, UserInfo userInfo);
        Task<bool> ChangeHideStatus(long id, UserInfo userInfo);
        Task<WordCard> Delete(long id, UserInfo userInfo);
        Task<List<WordCard>> CreateFromFile(IFormFile file, UserInfo userInfo);
    }
}
