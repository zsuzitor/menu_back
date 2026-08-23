using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistantApp.Models.DAL.Repositories
{
    public class StockEventRepository : GeneralRepository<StockEvent, long>, IStockEventRepository
    {
        public StockEventRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public async Task<List<StockEvent>> GetForPortfolioAsync(long portfolioId)
        {
            return await _db.StockEvent
                .AsNoTracking()
                .Include(x => x.Currency)
                .Include(x => x.StockElement).ThenInclude(x => x.Stock)
                .Where(x => x.PortfolioId == portfolioId).ToListAsync();
        }
    }
}
