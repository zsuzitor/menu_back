using BO.Models.FinancialAssistant.DAL;
using FinancialAssistantApp.Models.DTO;

namespace FinancialAssistantApp.Models.Mapper
{
    public static class Mapper
    {
        public static StockElementInPortfolio Map(StockElement element)
        {
            return new StockElementInPortfolio()
            {
                Id = element.Id,
                PortfolioId = element.PortfolioId,
                Price = element.Stock.LastPrice,
                StockId = element.StockId,
                StockName = element.Stock.Name,
                Count = element.Count,
                CurrencyId = element.Stock.CurrencyId,
                CurrencyName = element.Stock.Currency?.Name,
                Sum = element.Stock.LastPrice * element.Count,
            };
        }
    }
}
