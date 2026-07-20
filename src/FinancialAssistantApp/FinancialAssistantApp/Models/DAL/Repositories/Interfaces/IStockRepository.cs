using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.DAL.Repositories.Interfaces
{
    public interface IStockRepository : IGeneralRepository<Stock, long>
    {

        Task<List<Stock>> FindAsync(long? portfolioId, string text);
        Task<List<Stock>> GetGlobalAsync();
        Task<List<Stock>> GetGlobalForActualiztionAsync(DateTime date);
    }
}
