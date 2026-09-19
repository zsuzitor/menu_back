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

        public async Task<List<StockEvent>> GetEvents(List<long> elementId, DateTime start, DateTime end)
        {
            return await _db.StockEvent
                .AsNoTracking()
                .Where(x => elementId.Any(e => x.MainElementId == e) || elementId.Any(e => x.SubElementId == e))
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<StockEvent>> GetForPortfolioAsync(long portfolioId)
        {
            return await _db.StockEvent
                .AsNoTracking()
                .Include(x => x.MainElement)
                .ThenInclude(x => x.Stock)
                .Include(x => x.SubElement)
                .ThenInclude(x => x.Stock)
                .Where(x => x.PortfolioId == portfolioId)
                .OrderByDescending(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<StockEvent>> GetForStockAsync(long portfolioId, long stockId)
        {
            return await _db.StockEvent
                .AsNoTracking()
                .Include(x => x.SubElement)
                .ThenInclude(x => x.Stock)
                .Include(x => x.MainElement).ThenInclude(x => x.Stock)
                .Where(x => x.PortfolioId == portfolioId && (x.MainElement.StockId == stockId || x.SubElement.StockId == stockId))
                .OrderByDescending(x => x.Date).ToListAsync();

        }

        public async Task<List<StockEvent>> GetLastActualEvents(long portfolioId, DateTime time)
        {
            //тут не только по основному надо, возможно переписать, получать полный список подгружать туда, отсекать по датам
            //тогда придется вооще всю историю грузить с самых первых дат до нужной. подумать
            //мб делать 2 запроса, второй по .GroupBy(e => e.SubElementId)
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
