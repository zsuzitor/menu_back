using System.Collections.Generic;

namespace Menu.Host.Models.FinancialAssistantApp.Returns
{
    public class PortfolioStatisticReturn
    {
        public class Currency
        {
            public long CurrencyId { get; set; }
            public string CurrencyName { get; set; }
            public decimal CurrencySum { get; set; }
        }

        /// <summary>
        /// пополнения
        /// </summary>
        public decimal CashReplenishmentSum { get; set; }
        public List<Currency> ReplenishmentsByCurrency { get; set; }

        /// <summary>
        /// вывод
        /// </summary>
        public decimal WithdrawalCashSum { get; set; }
        public List<Currency> WithdrawalCashByCurrency { get; set; }
        public decimal DividendsCashSum { get; set; }
        public List<Currency> DividendsCashByCurrency { get; set; }      
        
        //на данный момент сумма
        public decimal SumNow { get; set; }


        //на начало периода сумма
        public decimal SumOnStartPeriod { get; set; }
        //на конец периода сумма(именно всего, не только что что прибавилось)
        public decimal SumOnEndPeriod { get; set; }
    }
}
