
using BO.Models.Auth;
using BO.Models.WordsCardsApp.DAL.Domain;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WordsCardsApp.BL.Services.Interfaces;
using WordsCardsApp.BO.Input;
using WordsCardsApp.DAL.Repositories.Interfaces;

namespace WordsCardsApp.BL.Services
{
    public class WordsCardsService : IWordsCardsService
    {
        private readonly IWordsCardsRepository _repository;
        public WordsCardsService(IWordsCardsRepository repository)
        {
            _repository = repository;
        }

        public async Task<WordCard> Create(UserInfo userInfo, WordCardInputModel input)
        {
            throw new System.NotImplementedException();
        }

        public async Task<WordCard> CreateFromFile(UserInfo userInfo, IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                string strFile = await reader.ReadToEndAsync();
            }
        }

        public async Task<WordCard> Delete(UserInfo userInfo, long id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<WordCard>> GetAllForUsers(UserInfo userInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}
