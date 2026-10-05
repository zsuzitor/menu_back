
namespace Menu.Host.Models.FinancialAssistantApp.Requests
{
    public class GetStockHistoryRequest
    {
        public long StockId { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
