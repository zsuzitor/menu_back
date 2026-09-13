

namespace BO.Models.FinancialAssistant.Enums
{
    public enum StockEventEnum
    {
        /// <summary>
        /// пополнение
        /// </summary>
        CashReplenishment = 1,
        Buy = 2,
        Sell = 3,
        Dividends = 4,
        /// <summary>
        /// вывод
        /// </summary>
        WithdrawalCash = 5,
        //изменение количества, это костыль что бы подбить портфель под реальные данные
        CountChange = 6
    }
}
