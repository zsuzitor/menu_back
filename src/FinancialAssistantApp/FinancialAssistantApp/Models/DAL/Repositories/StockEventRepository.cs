using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using Google.Api;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

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
                .OrderByDescending(x => x.EventDateTime)
                .ToListAsync();
        }

        public async Task<List<StockEvent>> GetForPortfolioNoTrackAsync(long portfolioId)
        {
            return await _db.StockEvent
                .AsNoTracking()
                .Include(x => x.MainElement)
                .ThenInclude(x => x.Stock)
                .Include(x => x.SubElement)
                .ThenInclude(x => x.Stock)
                .Where(x => x.PortfolioId == portfolioId)
                .OrderByDescending(x => x.EventDateTime)
                .ToListAsync();
        }

        public async Task<List<StockEvent>> GetForPortfolioAsync(long portfolioId)
        {
            return await _db.StockEvent
                .Where(x => x.PortfolioId == portfolioId)
                .OrderBy(x => x.EventDateTime)
                .ToListAsync();
        }

        public async Task<List<StockEvent>> GetForPortfolioAsync(long portfolioId, int pageSize, int page, StockEventEnum? type)
        {
            if (page > 0)
            {
                page--;
            }
            var skipCount = page * pageSize;

            return await _db.StockEvent
                .AsNoTracking()
                .Include(x => x.MainElement)
                .ThenInclude(x => x.Stock)
                .Include(x => x.SubElement)
                .ThenInclude(x => x.Stock)
                .Where(x => x.PortfolioId == portfolioId && (type == null || x.Type==type))
                .OrderByDescending(x => x.EventDateTime)
                .Skip(skipCount).Take(pageSize).ToListAsync();


        }

        public async Task<long> GetCountForPortfolioAsync(long portfolioId, StockEventEnum? type)
        {

            return await _db.StockEvent
                .AsNoTracking()
                .Where(x => x.PortfolioId == portfolioId && (type == null || x.Type == type))
                .OrderByDescending(x => x.EventDateTime).CountAsync();

        }



        public async Task<List<StockEvent>> GetForStockAsync(long portfolioId, long stockId, int pageSize, int page)
        {
            if (page > 0)
            {
                page--;
            }
            var skipCount = page * pageSize;
            return await _db.StockEvent
                .AsNoTracking()
                .Include(x => x.SubElement)
                .ThenInclude(x => x.Stock)
                .Include(x => x.MainElement).ThenInclude(x => x.Stock)
                .Where(x => x.PortfolioId == portfolioId && (x.MainElement.StockId == stockId || x.SubElement.StockId == stockId))
                .OrderByDescending(x => x.EventDateTime)
                .Skip(skipCount).Take(pageSize).ToListAsync();

        }

        public async Task<long> GetForStockCountAsync(long portfolioId, long stockId)
        {
            return await _db.StockEvent
                .AsNoTracking()
                .Include(x => x.SubElement)
                .ThenInclude(x => x.Stock)
                .Include(x => x.MainElement).ThenInclude(x => x.Stock)
                .Where(x => x.PortfolioId == portfolioId && (x.MainElement.StockId == stockId || x.SubElement.StockId == stockId))
                .OrderByDescending(x => x.EventDateTime).CountAsync();

        }

        public async Task<List<StockEvent>> GetLastForStockAsync(long portfolioId, List<long> stockId)
        {
            return await _db.StockEvent
                .AsNoTracking()
                .Where(e => stockId.Contains(e.Id) && e.PortfolioId == portfolioId)
                .GroupBy(e => e.Id)
                .Select(g => g
                    .OrderByDescending(e => e.EventDateTime)
                    .First())
                .ToListAsync();


        }

        public async Task<StockEvent> GetLastActualEvent(long elementId, DateTime time)
        {
            return await _db.StockEvent
                .Where(e => e.EventDateTime < time && (e.MainElementId == elementId || e.SubElementId == elementId)).OrderByDescending(x => x.EventDateTime).FirstOrDefaultAsync();
        }

        public async Task<StockEvent> GetLastActualEvent(long elementId)
        {
            return await _db.StockEvent
                .Where(e => (e.MainElementId == elementId || e.SubElementId == elementId)).OrderByDescending(x => x.EventDateTime).FirstOrDefaultAsync();
        }

        public async Task<List<StockEvent>> GetLastActualEvents(long portfolioId, DateTime time)
        {
            //тут не только по основному надо, возможно переписать, получать полный список подгружать туда, отсекать по датам
            //тогда придется вооще всю историю грузить с самых первых дат до нужной. подумать
            //мб делать 2 запроса, второй по .GroupBy(e => e.SubElementId)
            return await _db.StockEvent
                .Where(e => e.EventDateTime < time && e.PortfolioId == portfolioId)
                .GroupBy(e => e.MainElementId)
                .Select(g => g
                    .OrderByDescending(e => e.EventDateTime)
                    .First())
                .ToListAsync();

        }

        public async Task<List<StockEvent>> GetLastActualEventsForMainElement(List<long> portfolioId, DateTime time)
        {
            //тут не только по основному надо, возможно переписать, получать полный список подгружать туда, отсекать по датам
            //тогда придется вооще всю историю грузить с самых первых дат до нужной. подумать
            //мб делать 2 запроса, второй по .GroupBy(e => e.SubElementId)
            return await _db.StockEvent
                .Where(e => e.EventDateTime < time && portfolioId.Contains(e.PortfolioId))
                .GroupBy(e => e.MainElementId)
                .Select(g => g
                    .OrderByDescending(e => e.EventDateTime)
                    .First())
                .ToListAsync();

        }

        public async Task<List<StockEvent>> GetLastActualEventsForSubElement(List<long> portfolioId, DateTime time)
        {
            //тут не только по основному надо, возможно переписать, получать полный список подгружать туда, отсекать по датам
            //тогда придется вооще всю историю грузить с самых первых дат до нужной. подумать
            //мб делать 2 запроса, второй по .GroupBy(e => e.SubElementId)
            return await _db.StockEvent
                .Where(e => e.EventDateTime < time && portfolioId.Contains(e.PortfolioId)
                && e.SubElementId != null)
                .GroupBy(e => e.SubElementId)
                .Select(g => g
                    .OrderByDescending(e => e.EventDateTime)
                    .First())
                .ToListAsync();

        }


    }
}
