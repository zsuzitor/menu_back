using BO.Models.FinancialAssistant.DAL;
using FinancialAssistantApp.Models.DTO;

namespace FinancialAssistantApp.Models.Services.Interfaces
{
    public interface IStockElementService
    {
        Task<List<StockElementInPortfolio>> Get(long portfolioId, long userId);
    }
}
