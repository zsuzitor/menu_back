

using BO.Models.WordsCardsApp.DAL.Domain;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WordsCardsApp.DAL.Repositories.Interfaces;

namespace WordsCardsApp.DAL.Repositories
{
    public sealed class WordsListRepository : GeneralRepository<WordsList, long>, IWordsListRepository
    {
        public WordsListRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public async Task<WordCardWordList> AddToList(WordCardWordList relation)
        {
            _db.WordCardWordList.Add(relation);
            await _db.SaveChangesAsync();
            return relation;
        }

        public async Task<WordCardWordList> Get(long cardId, long listId)
        {
            return await _db.WordCardWordList.Where(x => x.WordCardId == cardId && x.WordsListId == listId).FirstOrDefaultAsync();
        }

        public async Task<List<WordsList>> GetAllForUser(long userId)
        {
            return await _db.WordsLists.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<WordsList> GetByIdIfAccess(long id, long userId)
        {
            return await _db.WordsLists.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);
        }

        public async Task<WordsList> GetByIdIfAccessNoTrack(long id, long userId)
        {
            return await _db.WordsLists.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);
        }

        public async Task<List<WordsList>> GetByIdIfAccess(List<long> id, long userId)
        {
            if (id == null || id.Count == 0)
            {
                return new List<WordsList>();
            }
            return await _db.WordsLists.Where(x => x.UserId == userId && id.Contains(x.Id)).ToListAsync();
        }

        public async Task<List<WordsList>> GetByIdIfAccessNoTrack(List<long> id, long userId)
        {
            if (id == null || id.Count == 0)
            {
                return new List<WordsList>();
            }
            return await _db.WordsLists.AsNoTracking().Where(x => x.UserId == userId && id.Contains(x.Id)).ToListAsync();
        }


        public async Task<WordCardWordList> RemoveFromList(WordCardWordList delObj)
        {
            _db.Attach(delObj);
            _db.WordCardWordList.Remove(delObj);
            await _db.SaveChangesAsync();
            return delObj;
        }
    }
}
