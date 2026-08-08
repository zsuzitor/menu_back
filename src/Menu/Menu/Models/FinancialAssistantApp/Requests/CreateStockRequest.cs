using BO.Models.FinancialAssistant.Enums;

namespace Menu.Host.Models.FinancialAssistantApp.Requests
{
    public class CreateStockRequest
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }


        public StockTypeEnum Type { get; set; }

        /// <summary>
        /// создана во всем приложении и апрувнута админом, записи можно создать и только для себя например что бы как позицию показать квартиру
        /// </summary>
        public bool IsGlobal { get; set; }
        /// <summary>
        /// для неглобальных
        /// </summary>
        //public long? PortfolioId { get; set; }
    }
}
