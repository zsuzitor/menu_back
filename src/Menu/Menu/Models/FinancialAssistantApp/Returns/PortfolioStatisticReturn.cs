using System.Collections.Generic;

namespace Menu.Host.Models.FinancialAssistantApp.Returns
{
    public class PortfolioStatisticReturn
    {
        public decimal CashReplenishmentSum { get; set; }
        public Dictionary<long, decimal> ReplenishmentsByCurrency { get; set; }
        public decimal WithdrawalCashSum { get; set; }
        public Dictionary<long, decimal> WithdrawalCashByCurrency { get; set; }
        public decimal DividendsCashSum { get; set; }
        public Dictionary<long, decimal> DividendsCashByCurrency { get; set; }
    }
}
