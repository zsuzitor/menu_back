using BO.Models.FinancialAssistant.DAL;
using System.Collections.Generic;

namespace Menu.Host.Models.FinancialAssistantApp.Returns
{
    public class StockElementReturn
    {
        public long Id { get; set; }

        public long StockId { get; set; }
        public decimal Count { get; set; }

        public long PortfolioId { get; set; }

    }
}
