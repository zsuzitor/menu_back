using BO.Models.DAL;
using BO.Models.FinancialAssistant.Enums;
using System;

namespace BO.Models.FinancialAssistant.DAL
{
    public class StockEvent : IDomainRecord<long>
    {
        public long Id { get; set; }
        public DateTime EventDateTime { get; set; }
        public DateTime CreationDateTime { get; set; }
        public StockEventEnum Type { get; set; }


        #region main
        //при покупке тут то что мы покупаем, тоесть главная сущность, акция фонд и тд
        public decimal MainCountChange { get; set; }
        public decimal MainCountNow { get; set; }
        public long MainElementId { get; set; }
        public StockElement MainElement { get; set; }
        #endregion

        #region sub
        //при покупке тут то за что мы покупаем, тоесть зависимая сущность, валюта
        public decimal? SubCountChange { get; set; }
        public decimal? SubCountNow { get; set; }
        public decimal? SubCountOldValue { get; set; }
        public long? SubElementId { get; set; }
        public StockElement SubElement { get; set; }
        #endregion

        //public decimal? Price { get; set; }

        /// <summary>
        /// если есть некий обмен что то на что то, если просто докинуть денег на счет то валюты тут нет тк нет обмена
        /// </summary>
        //public long? CurrencyId { get; set; }
        //public Stock Currency { get; set; }

        /// <summary>
        /// по идеи не нужно тк можно выйти через StockElement, но запросить историю портфеля по логике частый запрос так что так лучше
        /// </summary>
        public long PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }

        public byte[] RowVersion { get; set; }
    }
}
