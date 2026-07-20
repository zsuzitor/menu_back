using BO.Models.FinancialAssistant.DAL;

namespace FinancialAssistantApp.Models.Services.Interfaces
{
    public interface IStockService
    {
        Task<Stock> CreateAsync(Stock obj, long userId);
        Task<Stock> DeleteAsync(long id, long userId);
        Task<Stock> UpdateAsync(Stock obj, long userId);
        Task<List<Stock>> FindAsync(long? portfolioId, string text, long userId);
        Task GlobalActualizeAsync(long userId);

    }
}
