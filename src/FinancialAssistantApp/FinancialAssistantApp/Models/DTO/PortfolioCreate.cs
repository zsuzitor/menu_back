
namespace FinancialAssistantApp.Models.DTO
{
    public class PortfolioCreate
    {
        public long Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// основная валюта портфеля, для отображения текущих цен
        /// </summary>
        public long? CurrencyId { get; set; }

    }
}
