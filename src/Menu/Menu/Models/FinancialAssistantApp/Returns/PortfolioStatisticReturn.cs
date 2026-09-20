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
        
        //на данный момент сумма
        public decimal SumNow { get; set; }


        //на начало периода сумма
        public decimal SumOnStartPeriod { get; set; }
        //на конец периода сумма(именно всего, не только что что прибавилось)
        public decimal SumOnEndPeriod { get; set; }
    }
}
