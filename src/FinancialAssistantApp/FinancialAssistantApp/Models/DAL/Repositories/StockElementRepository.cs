using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistantApp.Models.DAL.Repositories
{
    public class StockElementRepository : GeneralRepository<StockElement, long>, IStockElementRepository
    {
        public StockElementRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public async Task<StockElement> Get(long portfolioId, long stockId)
        {
            return await _db.StockElement.FirstOrDefaultAsync(x => x.PortfolioId == portfolioId && x.StockId == stockId);
        }

        public async Task<List<StockElement>> Get(long portfolioId)
        {
            return await _db.StockElement.Where(x => x.PortfolioId == portfolioId && x.Count > 0).ToListAsync();

        }
    }
}
