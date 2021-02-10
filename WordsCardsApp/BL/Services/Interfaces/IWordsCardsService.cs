

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
        Task<List<WordCard>> GetAllForUsers(UserInfo userInfo);
        Task<WordCard> Create(UserInfo userInfo, WordCardInputModel input);
        Task<WordCard> Delete(UserInfo userInfo, long id);
        Task<WordCard> CreateFromFile(UserInfo userInfo , IFormFile file);
    }
}
