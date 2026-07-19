using BO.Models.DAL;
using System;
using System.Collections.Generic;

namespace BO.Models.FinancialAssistant.DAL
{
    public class Stock : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public DateTime? ActualizationTime { get; set; }
        public decimal LastPrice { get; set; }

        public bool IsCurrency { get; set; }
        public long? CurrencyId { get; set; }
        public Stock Currency { get; set; }

        /// <summary>
        /// создана во всем приложении и апрувнута админом, записи можно создать и только для себя например что бы как позицию показать квартиру
        /// </summary>
        public bool IsGlobal { get; set; }
        /// <summary>
        /// для неглобальных
        /// </summary>
        public long? PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }

        public List<StockHistory> StockHistory { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
