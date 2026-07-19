using BO.Models.FinancialAssistant.DAL;

namespace FinancialAssistantApp.Models.Services.Interfaces
{
    public interface IPortfolioService
    {
        Task<List<Portfolio>> GetAllAsync(long userId);
        Task<Portfolio> CreateAsync(Portfolio obj, long userId);
        Task<Portfolio> DeleteAsync(long id, long userId);
    }
}
