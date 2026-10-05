using BO.Models.FinancialAssistant.DAL;
using FinancialAssistantApp.Models.DTO;

namespace FinancialAssistantApp.Models.Services.Interfaces
{
    public interface IStockEventService
    {

        Task<StockEvent> CreateEventAsync(StockEventCreate obj, long userId);
        Task<(List<StockEvent>, long)> GetForPortfolioAsync(GetPortfolioEvents req);
        Task<(List<StockEvent>, long)> GetForStockAsync(long portfolioId, long stockId, long userId, int pageSize, int pageNum);
        //Task<StockEvent> DeleteEventAsync(long id, long userId);//мне кажется лишнее и как это делать? просто удалять? откатвать?
        //Task<StockEvent> UpdateEventAsync(StockEvent obj, long userId);//тоже в долгий ящик

    }
}
