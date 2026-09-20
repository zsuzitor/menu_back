
namespace FinancialAssistantApp.Models.DTO
{
    public class PortfolioStatistic
    {
        //на начало периода сумма
        //разбивка по месяцам или по каким то датам - динамика, рост падение. на конец месяца?


        //Пополнений за период
        public decimal CashReplenishmentSum { get; set; }
        public Dictionary<long, decimal> ReplenishmentsByCurrency { get; set; }


        //Выводов за период
        public decimal WithdrawalCashSum { get; set; }
        public Dictionary<long, decimal> WithdrawalCashByCurrency { get; set; }

        //дивиденды за период
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
