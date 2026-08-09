using BO.Models.FinancialAssistant.DAL;
using System;

namespace Menu.Host.Models.FinancialAssistantApp.Returns
{
    public class StockHistoryReturn
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }


        public decimal Price { get; set; }

        public long StockId { get; set; }

        public long? CurrencyId { get; set; }
    }
}
