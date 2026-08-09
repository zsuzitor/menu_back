using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.DAL.Repositories.Interfaces
{
    public interface IStockHistoryRepository : IGeneralRepository<StockHistory, long>
    {
        Task<List<StockHistory>> GetHistoryAsync(long stockId);
    }
}
