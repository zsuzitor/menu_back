using BO.Models.DAL;
using System;

namespace BO.Models.FinancialAssistant.DAL
{
    public class StockHistory : IDomainRecord<long>
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }


        public decimal Price { get; set; }

        public long StockId { get; set; }
        public Stock Stock { get; set; }

        public long? CurrencyId { get; set; }
        public Stock Currency { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
