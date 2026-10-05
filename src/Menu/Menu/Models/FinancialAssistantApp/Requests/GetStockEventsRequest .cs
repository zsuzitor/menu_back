using BO.Models.FinancialAssistant.Enums;

namespace Menu.Host.Models.FinancialAssistantApp.Requests
{
    public class GetStockEventsRequest
    {
        public long PortfolioId { get; set; }
        public long StockId { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
