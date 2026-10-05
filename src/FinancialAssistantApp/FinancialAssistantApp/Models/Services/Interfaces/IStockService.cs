using BO.Models.FinancialAssistant.DAL;
using FinancialAssistantApp.Models.DTO;

namespace FinancialAssistantApp.Models.Services.Interfaces
{
    public interface IStockService
    {
        Task<Stock> CreateAsync(CreateStock obj, long userId);
        Task<Stock> DeleteAsync(long id, long userId);
        Task<(List<StockHistory>,long)> GetHistoryAsync(long id, long userId, int pageSize, int pageNum);
        Task<StockHistory> CreateHistoryAsync(StockHistory req, long userId);
        Task<StockHistory> DeleteHistoryAsync(long historyId, long userId);
        Task<Stock> GetAsync(long id, long userId);
        Task<Stock> UpdateAsync(CreateStock obj, long userId);
        Task<List<Stock>> FindAsync( string text, long userId);
        Task<List<Stock>> GetAsync(  long userId);
        Task<List<Stock>> GetCurrencyAsync( long userId);
        Task GlobalActualizeAsync(long userId);
        Task FillHistoryAsync(long stockId,long userId);

    }
}
