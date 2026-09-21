using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.DAL.Repositories.Interfaces
{
    public interface IStockEventRepository : IGeneralRepository<StockEvent, long>
    {
        Task<List<StockEvent>> GetForPortfolioAsync(long portfolioId);
        Task<List<StockEvent>> GetForStockAsync(long portfolioId, long stockId);
        Task<List<StockEvent>> GetLastActualEvents(long portfolioId, DateTime time);
        Task<List<StockEvent>> GetLastActualEvents(List<long> portfolioId, DateTime time);
        Task<StockEvent> GetLastActualEvent(long elementId, DateTime time);
        Task<List<StockEvent>> GetEvents(List<long> elementId, DateTime start, DateTime end);
    }
}
