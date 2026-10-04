
namespace TIntegration.Models.DTO
{
    public class MapTElement
    {
        /// <summary>
        /// тикер в моем приложении
        /// </summary>
        public string AppTicker { get; set; }
        public string TBankFigi { get; set; }
        /// <summary>
        /// валюта в которой вернется цена, тикер из моего приложения
        /// </summary>
        public string TBankCurrency { get; set; }
    }
}
