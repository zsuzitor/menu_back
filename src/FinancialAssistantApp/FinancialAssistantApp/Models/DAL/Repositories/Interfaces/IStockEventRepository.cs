using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using DAL.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.DAL.Repositories.Interfaces
{
    public interface IStockEventRepository : IGeneralRepository<StockEvent, long>
    {
        Task<List<StockEvent>> GetForPortfolioAsync(long portfolioId);
        Task<List<StockEvent>> GetForPortfolioAsync(long portfolioId, int pageSize, int page, StockEventEnum? type);
        Task<long> GetCountForPortfolioAsync(long portfolioId, StockEventEnum? type);
        Task<List<StockEvent>> GetForStockAsync(long portfolioId, long stockId, int pageSize, int page);
        Task<long> GetForStockCountAsync(long portfolioId, long stockId);
        Task<List<StockEvent>> GetLastForStockAsync(long portfolioId, List<long> stockId);
        Task<List<StockEvent>> GetLastActualEvents(long portfolioId, DateTime time);
        Task<List<StockEvent>> GetLastActualEventsForMainElement(List<long> portfolioId, DateTime time);
        Task<List<StockEvent>> GetLastActualEventsForSubElement(List<long> portfolioId, DateTime time);
        Task<StockEvent> GetLastActualEvent(long elementId, DateTime time);
        Task<StockEvent> GetLastActualEvent(long elementId);
        Task<List<StockEvent>> GetEvents(List<long> elementId, DateTime start, DateTime end);
    }
}
