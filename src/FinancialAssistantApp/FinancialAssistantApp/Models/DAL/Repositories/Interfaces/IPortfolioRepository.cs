
using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL.Repositories.Interfaces;

namespace TaskManagementApp.Models.DAL.Repositories.Interfaces
{
    public interface IPortfolioCachedRepository : IPortfolioRepository;
    public interface IPortfolioRepository : IGeneralRepository<Portfolio, long>
    {
        Task<List<Portfolio>> GetAllAsync(long userId);
        Task<Portfolio> GetAsync(long id, long userId);
        Task<bool> ExistAsync(long id, long userId);
        //Task<Portfolio> GetWithElementsAsync(long presetId);
    }
}
