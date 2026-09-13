using BO.Models.FinancialAssistant.Enums;

namespace FinancialAssistantApp.Models.DTO
{
    public class StockEventCreate
    {
        public DateTime Date { get; set; }
        public decimal Count { get; set; }
        public StockEventEnum Type { get; set; }

        //public long StockElementId { get; set; }
        public long? StockId { get; set; }

        public decimal? Price { get; set; }
        public long? CurrencyId { get; set; }
        /// <summary>
        /// Действия с валютой, тоесть при продаже акций будут начислены деньги, при покупке списаны
        /// </summary>
        public bool CurrencyActions { get; set; }


        public long PortfolioId { get; set; }
    }
}
