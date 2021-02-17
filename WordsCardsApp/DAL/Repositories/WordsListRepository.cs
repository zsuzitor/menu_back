

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
    public class WordsListRepository : GeneralRepository<WordsList, long>, IWordsListRepository
    {
        private readonly MenuDbContext _db;
        public WordsListRepository(MenuDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<WordsList>> GetAllForUser(long userId)
        {
            return await _db.WordsLists.Where(x => x.UserId == userId).ToListAsync();
        }

        public async Task<WordsList> GetByIdIfAccess(long id, long userId)
        {
            return await _db.WordsLists.FirstOrDefaultAsync(x => x.UserId == userId && x.Id == id);
        }
    }
}
