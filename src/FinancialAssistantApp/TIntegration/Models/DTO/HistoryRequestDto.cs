using TIntegration.Models.Enums;

namespace TIntegration.Models.DTO
{
    public class HistoryRequestDto
    {
        public string Code { get; set; }
        //public StockTypeEnum Type { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

    }
}
