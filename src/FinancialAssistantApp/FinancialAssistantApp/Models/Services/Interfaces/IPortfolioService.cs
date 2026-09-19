using BO.Models.FinancialAssistant.DAL;
using FinancialAssistantApp.Models.DTO;

namespace FinancialAssistantApp.Models.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<List<Portfolio>> GetAllAsync(long userId);
        Task<Portfolio> GetAsync(long  id, long userId);
        Task<PortfolioStatistic> GetStatisticAsync(PortfolioStatisticRequestDto req, long userId);
        Task<Portfolio> CreateAsync(PortfolioCreate obj, long userId);
        Task<Portfolio> DeleteAsync(long id, long userId);
        //Task<List<StockEvent>> GetEventsAsync(long id, long userId);
        Task<Portfolio> UpdateAsync(PortfolioCreate obj, long userId);
    }
}
