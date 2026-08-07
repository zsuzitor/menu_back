using BO.Models.FinancialAssistant.DAL;

namespace FinancialAssistantApp.Models.Services.Interfaces
{
    public interface IStockElementService
    {
        Task<List<StockElement>> Get(long portfolioId, long userId);
    }
}
