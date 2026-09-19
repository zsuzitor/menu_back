using System;
using System.Collections.Generic;

namespace Menu.Host.Models.FinancialAssistantApp.Requests
{
    public class PortfolioStatisticRequest
    {
        public List<long> PortfolioId { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public long CurrencyId { get; set; }
    }
}
