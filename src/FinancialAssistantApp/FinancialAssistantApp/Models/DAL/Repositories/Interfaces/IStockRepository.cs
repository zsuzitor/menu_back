using BO.Models.FinancialAssistant.DAL;
using DAL.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.DAL.Repositories.Interfaces
{
    public interface IStockRepository : IGeneralRepository<Stock, long>
    {

        Task<List<Stock>> FindAsync( string text, long? userId);
        Task<List<Stock>> GetForUserAsync( long? userId);
        Task<List<Stock>> GetCurrencyAsync(long? userId);
        Task<List<Stock>> GetGlobalAsync();
        Task<List<Stock>> GetGlobalForActualiztionAsync(DateTime date);
        Task<List<Stock>> GetGlobalByCodesNoTrack(IEnumerable<string> codes);
    }
}
