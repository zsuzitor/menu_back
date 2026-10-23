using System;
using System.Collections.Generic;

namespace Menu.Host.Models.FinancialAssistantApp.Returns
{
    public class PortfolioStatisticReturn
    {
        public class StockShort
        {
            public long StockId { get; set; }
            public string StockName { get; set; }
            public decimal Sum { get; set; }
        }

        public class PeriodSum
        {
            public DateTime Date { get; set; }
            public decimal Sum { get; set; }
        }

        /// <summary>
        /// пополнения
        /// </summary>
        public decimal CashReplenishmentSum { get; set; }
        public List<StockShort> ReplenishmentsByCurrency { get; set; }

        /// <summary>
        /// вывод
        /// </summary>
        public decimal WithdrawalCashSum { get; set; }
        public List<StockShort> WithdrawalCashByCurrency { get; set; }
        public decimal DividendsCashSum { get; set; }
        public List<StockShort> DividendsCashByCurrency { get; set; }      
        
        //на данный момент сумма
        public decimal SumNow { get; set; }


        //на начало периода сумма
        public decimal SumOnStartPeriod { get; set; }
        //на конец периода сумма(именно всего, не только что что прибавилось)
        public decimal SumOnEndPeriod { get; set; }

        public List<StockShort> TotalStockCount { get; set; }


        public List<PortfolioStatisticReturn.PeriodSum> PeriodSums { get; set; }
    }
}
