using System.Collections.Generic;

namespace Menu.Host.Models.FinancialAssistantApp.Returns
{
    public class GetStockEventsResponse
    {
        public List<StockEventReturn> Data { get; set; }
        public long CountTotal { get; set; }
    }
}
