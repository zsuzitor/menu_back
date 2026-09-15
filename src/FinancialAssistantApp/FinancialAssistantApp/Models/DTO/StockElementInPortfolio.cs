

namespace FinancialAssistantApp.Models.DTO
{
    public class StockElementInPortfolio
    {
        public long Id { get; set; }
        public long StockId { get; set; }
        public string StockName { get; set; }
        public decimal Count { get; set; }
        public long? CurrencyId { get; set; }
        public string CurrencyName { get; set; }

        public long PortfolioId { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get; set; }
    }
}
