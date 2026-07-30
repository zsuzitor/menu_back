using BO.Models.FinancialAssistant.DAL;
using FinancialAssistantApp.Models.DTO;

namespace FinancialAssistantApp.Models.Services.Interfaces
{
    public interface IStockEventService
    {

        Task<StockEvent> CreateEventAsync(StockEventCreate obj, long userId);
        //Task<StockEvent> DeleteEventAsync(long id, long userId);//мне кажется лишнее и как это делать? просто удалять? откатвать?
        //Task<StockEvent> UpdateEventAsync(StockEvent obj, long userId);//тоже в долгий ящик

    }
}
