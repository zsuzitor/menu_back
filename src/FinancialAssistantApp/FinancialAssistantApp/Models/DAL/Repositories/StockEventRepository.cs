using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.DAL.Repositories
{
    public class StockEventRepository : GeneralRepository<StockEvent, long>, IStockEventRepository
    {
        public StockEventRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }
    }
}
