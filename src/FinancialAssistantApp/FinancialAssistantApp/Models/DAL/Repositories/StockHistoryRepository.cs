using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistantApp.Models.DAL.Repositories
{
    public class StockHistoryRepository : GeneralRepository<StockHistory, long>, IStockHistoryRepository
    {
        public StockHistoryRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {


        }

        public async Task<List<StockHistory>> GetHistoryAsync(long stockId)
        {
            return await _db.StockHistory.AsNoTracking().Where(x => x.StockId == stockId
            ).ToListAsync();

        }
    }
}
