
using BO.Models.WordsCardsApp.DAL.Domain;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsCardsApp.DAL.Repositories.Interfaces;

namespace WordsCardsApp.DAL.Repositories
{
    public class WordsCardsRepository : GeneralRepository<WordCard, long>, IWordsCardsRepository
    {
        //private readonly MenuDbContext _db;

        public WordsCardsRepository(MenuDbContext db):base(db)
        {
            //_db = db;
        }

        public async Task<bool?> ChangeHideStatusAsync(long id, long userId)
        {
            var wordCard = await GetByIdIfAccessAsync(id, userId);
            if (wordCard == null)
            {
                return null;
            }

            wordCard.Hided = !wordCard.Hided;
            await _db.SaveChangesAsync();
            return wordCard.Hided;
        }



        public async Task<WordCard> DeleteAsync(long id, long userId)
        {
            var wordCard = await GetByIdIfAccessAsync(id, userId);
            if (wordCard == null)
            {
                return null;
            }

            return await DeleteAsync(wordCard);
            //return wordCard;
        }

       
        public async Task<List<WordCard>> GetAllUsersWordCardsAsync(long userId)
        {
            return await GetAllForUser(userId).ToListAsync();
        }

        //todo возможно есть случаи когда вызывается этот метод хотя нам надо просто узнать если ли доступ вообще
        public async Task<WordCard> GetByIdIfAccessAsync(long id, long userId)
        {
            return await _db.WordsCards.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);
        }

        public async Task<List<WordCard>> LoadWordListsIdAsync(List<WordCard> words)
        {
            if (words == null || words.Count == 0)
            {
                return new List<WordCard>();
            }

            var wordsIds = words.Select(x => x.Id);
            //GetAllForUser(userId).Include(x=>x.WordCardWordList);
            var relations = await _db.WordCardWordList.Where(x => wordsIds.Contains(x.WordCardId)).ToListAsync();

            var grouped = relations.GroupBy(x => x.WordCardId);
            foreach (var gr in grouped)
            {
                var word = words.First(x => x.Id == gr.Key);
                word.WordCardWordList = gr.ToList();
            }

            //foreach (var word in words)
            //{
            //    word.WordCardWordList = relations.Where(x => x.WordCardId == word.Id).ToList();
            //}
            //await _db.WordCardWordList.Join(words, x => x.WordCardId, x => x.Id, (lst, word) => new { lst,word }).ToListAsync();
            return words;
        }



        private IQueryable<WordCard> GetAllForUser(long userId)
        {
            return _db.WordsCards.Where(x => x.UserId == userId);
        }
    }
}
