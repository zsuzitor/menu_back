

namespace TIntegration.Models.DTO
{
    public class PriceResponseDto
    {
        public string Code { get; set; }
        //public StockTypeEnum Type { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Price { get; set; }
    }
}
