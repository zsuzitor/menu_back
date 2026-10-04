using BO.Models.FinancialAssistant.Enums;

namespace FinancialAssistantApp.Models.DTO
{
    public class GetPortfolioEvents
    {
        public long PortfolioId { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public long UserId { get; set; }
        public StockEventEnum? Type { get; set; }
    }
}
