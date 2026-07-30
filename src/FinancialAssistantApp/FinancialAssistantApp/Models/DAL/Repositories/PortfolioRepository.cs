using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.DAL.Repositories
{
    public class PortfolioRepository : GeneralRepository<Portfolio, long>, IPortfolioRepository
    {
        public PortfolioRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public async Task<bool> ExistAsync(long id, long userId)
        {
            return await _db.Portfolio.AnyAsync(x => x.UserId == userId && x.Id == id);
        }

        public async Task<List<Portfolio>> GetAllAsync(long userId)
        {
            return await _db.Portfolio.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();

        }

        public async Task<Portfolio> GetAsync(long id, long userId)
        {
            return await _db.Portfolio.FirstOrDefaultAsync(x => x.UserId == userId && x.Id==id);
        }
    }
}
