using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using Google.Api;
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
                .Include(x => x.StockElement)
                .ThenInclude(x => x.Stock)
                .Where(x => x.PortfolioId == portfolioId)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<StockEvent>> GetForStockAsync(long portfolioId, long stockId)
        {
            return await _db.StockEvent
                .AsNoTracking()
                .Include(x => x.Currency)
                .Include(x => x.StockElement).ThenInclude(x => x.Stock)
                .Where(x => x.PortfolioId == portfolioId && x.StockElement.StockId == stockId)
                .OrderByDescending(x => x.Date).ToListAsync();

        }

        public async Task<List<StockEvent>> GetLastActualEvents(long portfolioId, DateTime time)
        {
            return await _db.StockEvent
                .Where(e => e.Date < time && e.PortfolioId == portfolioId)
                .GroupBy(e => e.MainElementId)
                .Select(g => g
                    .OrderByDescending(e => e.Date)
                    .First())
                .ToListAsync();

        }
    }
}
