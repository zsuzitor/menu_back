using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.DAL.Repositories.Interfaces
{
    public interface IStockElementRepository : IGeneralRepository<StockElement, long>
    {
        Task<StockElement> Get(long portfolioId, long stockId);
        Task<List<StockElement>> Get(long portfolioId);
    }
}
