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

        public async Task<List<Portfolio>> GetAllAsync(long userId)
        {
            return await _db.Portfolio.AsNoTracking().Where(x => x.UserId == userId).ToListAsync();

        }
    }
}
