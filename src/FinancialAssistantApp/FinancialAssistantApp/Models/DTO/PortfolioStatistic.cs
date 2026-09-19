
namespace FinancialAssistantApp.Models.DTO
{
    public class PortfolioStatistic
    {
        //на данный момент сумма
        //на начало периода сумма
        //разбивка по месяцам или по каким то датам - динамика, рост падение. на конец месяца?


        //Пополнений
        public decimal CashReplenishmentSum { get; set; }
        public Dictionary<long, decimal> ReplenishmentsByCurrency { get; set; }


        //Выводов
        public decimal WithdrawalCashSum { get; set; }
        public Dictionary<long, decimal> WithdrawalCashByCurrency { get; set; }

        //дивиденды
        public decimal DividendsCashSum { get; set; }
        public Dictionary<long, decimal> DividendsCashByCurrency { get; set; }

    }
}
