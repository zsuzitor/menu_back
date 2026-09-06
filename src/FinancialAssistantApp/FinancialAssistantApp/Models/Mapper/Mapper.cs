using BO.Models.FinancialAssistant.DAL;
using FinancialAssistantApp.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinancialAssistantApp.Models.Mapper
{
    public static class Mapper
    {
        public static StockElementInPortfolio Map(StockElement element)
        {
            return new StockElementInPortfolio()
            {
                Count = element.Count,
                CurrencyId = element.Stock.CurrencyId,
                Id = element.Id,
                PortfolioId = element.PortfolioId,
                Price = element.Stock.LastPrice,
                StockId = element.StockId,
                StockName = element.Stock.Name,
                Sum = element.Stock.LastPrice * element.Count,
            };
        }
    }
}
