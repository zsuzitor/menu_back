using BO.Models.FinancialAssistant.Enums;

namespace Menu.Host.Models.FinancialAssistantApp.Requests
{
    public class GetPortfolioEventsRequest
    {
        public long PortfolioId { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public StockEventEnum? Type { get; set; }
    }
}
