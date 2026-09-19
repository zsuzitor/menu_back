
namespace FinancialAssistantApp.Models.DTO
{
    public class PortfolioStatisticRequestDto
    {
        public List<long> PortfolioId { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public long CurrencyId { get; set; }
    }
}
