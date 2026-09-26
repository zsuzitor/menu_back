
namespace FinancialAssistantApp.Models.DTO
{
    public class CurrencyMoney
    {
        public long CurrencyId { get; set; }
        public decimal Money { get; set; }
    }


    public class PeriodSum
    {
        public DateTime Date { get; set; }
        public decimal Sum { get; set; }
    }



    public class PortfolioStatistic
    {
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

        public List<PeriodSum> PeriodSums { get; set; }

        public PortfolioStatistic()
        {
            PeriodSums = new List<PeriodSum>();
        }

    }
}
