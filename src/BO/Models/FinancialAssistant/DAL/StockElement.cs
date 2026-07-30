

using BO.Models.DAL;
using System.Collections.Generic;

namespace BO.Models.FinancialAssistant.DAL
{
    public class StockElement : IDomainRecord<long>
    {
        public long Id { get; set; }

        public long StockId { get; set; }
        public Stock Stock { get; set; }
        public decimal Count { get; set; }

        public long PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }

        public List<StockEvent> Events { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
