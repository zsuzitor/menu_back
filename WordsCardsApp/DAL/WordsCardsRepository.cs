
using BO.Models.WordsCardsApp.DAL.Domain;
using DAL.Models.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsCardsApp.DAL.Repositories.Interfaces;

namespace WordsCardsApp.DAL
{
    public class WordsCardsRepository : IWordsCardsRepository
    {
        private readonly MenuDbContext _db;

        public WordsCardsRepository(MenuDbContext db)
        {
            _db = db;
        }

        public async Task<bool?> ChangeHideStatus(long id, long userId)
        {
            var wordCard = await GetByIdIfAccess(id, userId);
            if (wordCard == null)
            {
                return null;
            }

            wordCard.Hided = !wordCard.Hided;
            await _db.SaveChangesAsync();
            return wordCard.Hided;
        }

        public async Task<WordCard> Create(WordCard newData)
        {
            _db.Add(newData);
            await _db.SaveChangesAsync();
            return newData;
        }

        public async Task<List<WordCard>> CreateList(List<WordCard> newArr)
        {
            _db.AddRange(newArr);
            await _db.SaveChangesAsync();
            return newArr;
        }

        public async Task<WordCard> Delete(long id, long userId)
        {
            var wordCard = await GetByIdIfAccess(id, userId);
            if (wordCard == null)
            {
                return null;
            }

            _db.Remove(wordCard);
            await _db.SaveChangesAsync();
            return wordCard;
        }

        public async Task Edit(WordCard newData)
        {
            _db.Attach(newData);
            await _db.SaveChangesAsync();
        }

        public async Task<List<WordCard>> GetAllUsersWordCards(long userId)
        {
            return await _db.WordsCards.Where(x => x.UserId == userId).ToListAsync();
        }

        //todo возможно есть случаи когда вызывается этот метод хотя нам надо просто узнать если ли доступ вообще
        public async Task<WordCard> GetByIdIfAccess(long id, long userId)
        {
            return await _db.WordsCards.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);
        }
    }
}
