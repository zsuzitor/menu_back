using BO.Models.FinancialAssistant.DAL;
using FinancialAssistantApp.Models.DTO;

namespace FinancialAssistantApp.Models.Services.Interfaces
{
    public interface IStockService
    {
        Task<Stock> CreateAsync(CreateStock obj, long userId);
        Task<Stock> DeleteAsync(long id, long userId);
        Task<Stock> UpdateAsync(CreateStock obj, long userId);
        Task<List<Stock>> FindAsync(long? portfolioId, string text, long userId);
        Task<List<Stock>> GetCurrencyAsync(long? portfolioId, long userId);
        Task GlobalActualizeAsync(long userId);

    }
}
