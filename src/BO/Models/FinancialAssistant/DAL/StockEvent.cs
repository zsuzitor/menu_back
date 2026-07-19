using BO.Models.DAL;
using BO.Models.FinancialAssistant.Enums;
using System;

namespace BO.Models.FinancialAssistant.DAL
{
    public class StockEvent : IDomainRecord<long>
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Count { get; set; }
        public StockEventEnum Type { get; set; }

        public long StockElementId { get; set; }
        public StockElement StockElement { get; set; }

        public long? CurrencyId { get; set; }
        public Stock Currency { get; set; }

        /// <summary>
        /// по идеи не нужно тк можно выйти через StockElement, но запросить историю портфеля по логике частый запрос так что так лучше
        /// </summary>
        public long PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
