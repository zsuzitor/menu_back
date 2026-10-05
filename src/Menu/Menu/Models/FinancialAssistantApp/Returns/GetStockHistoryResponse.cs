using System.Collections.Generic;

namespace Menu.Host.Models.FinancialAssistantApp.Returns
{
    public class GetStockHistoryResponse
    {
        public List<StockHistoryReturn> Data { get; set; }
        public long CountTotal { get; set; }
    }
}
