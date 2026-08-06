using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistantApp.Models.DAL.Repositories
{
    public class StockRepository : GeneralRepository<Stock, long>, IStockRepository
    {
        public StockRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public async Task<List<Stock>> FindAsync(long? portfolioId, string text)
        {
            return await _db.Stock
                .Where(x =>
                ((portfolioId == null || x.PortfolioId==portfolioId)
                    || x.IsGlobal)
                && (EF.Functions.Like(x.Code, $"%{text}%")
                    || EF.Functions.Like(x.Name, $"%{text}%"))
                ).ToListAsync();
        }

        public async Task<List<Stock>> GetGlobalByCodesNoTrack(IEnumerable<string> codes)
        {
            return await _db.Stock.AsNoTracking().Where(x => x.IsGlobal && codes.Contains(x.Code)).ToListAsync();
        }

        public async Task<List<Stock>> GetGlobalAsync()
        {
            return await _db.Stock.AsNoTracking().Where(x => x.IsGlobal).ToListAsync();
        }

        public async Task<List<Stock>> GetGlobalForActualiztionAsync(DateTime date)
        {
            return await _db.Stock.AsNoTracking().Where(x => x.IsGlobal && x.ActualizationTime<date).ToListAsync();
        }
    }
}
