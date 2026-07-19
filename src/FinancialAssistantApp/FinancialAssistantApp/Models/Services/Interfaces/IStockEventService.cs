using BO.Models.FinancialAssistant.DAL;

namespace FinancialAssistantApp.Models.Services.Interfaces
{
    public interface IStockEventService
    {

        Task<StockEvent> CreateEventAsync(StockEvent obj, long userId);
        Task<StockEvent> DeleteEventAsync(long id, long userId);
        Task<StockEvent> UpdateEventAsync(StockEvent obj, long userId);

    }
}
