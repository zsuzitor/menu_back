using System.Collections.Generic;

namespace Menu.Host.Models.FinancialAssistantApp.Returns
{
    public class GetPortfolioEventsResponse
    {
        public List<StockEventReturn> Data { get; set; }
        public long CountTotal { get; set; }
    }
}
