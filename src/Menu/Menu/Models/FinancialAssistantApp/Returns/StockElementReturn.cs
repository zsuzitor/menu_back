

namespace Menu.Host.Models.FinancialAssistantApp.Returns
{
    public class StockElementReturn
    {
        public long Id { get; set; }

        public long StockId { get; set; }
        public string StockName { get; set; }
        public decimal Count { get; set; }

        public long PortfolioId { get; set; }

    }
}
