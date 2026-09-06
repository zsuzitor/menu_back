using BO.Models.FinancialAssistant.DAL;
using FinancialAssistantApp.Models.DTO;
using Menu.Host.Models.FinancialAssistantApp.Requests;
using Menu.Host.Models.FinancialAssistantApp.Returns;

namespace Menu.Host.Models.FinancialAssistantApp
{
    public static class Mapper
    {
        public static PortfolioReturn Map(this Portfolio p)
        {
            return new PortfolioReturn()
            {
                CurrencyId = p.CurrencyId,
                Id = p.Id,
                Name = p.Name,
                UserId = p.UserId,
                CurrencyName = p.Currency?.Name,
            };
        }


        public static PortfolioCreate Map(this CreatePortfolioRequest p)
        {
            return new PortfolioCreate()
            {
                CurrencyId = p.CurrencyId,
                Id = p.Id,
                Name = p.Name,
            };
        }


        public static StockElementReturn Map(this StockElementInPortfolio p)
        {
            return new StockElementReturn()
            {
                Count = p.Count,
                Id = p.Id,
                PortfolioId = p.PortfolioId,
                StockId = p.StockId,
                StockName = p.StockName,
                Price = p.Price,
                Sum = p.Count * p.Price,
                CurrencyId = p.CurrencyId,
                CurrencyName = p.CurrencyName,
            };
        }

        public static StockEventCreate Map(this StockEventCreateRequest p)
        {
            return new StockEventCreate()
            {
                Count = p.Count,
                PortfolioId = p.PortfolioId,
                StockId = p.StockId,
                CurrencyId = p.CurrencyId,
                Date = p.Date,
                Price = p.Price,
                Type = p.Type,
            };
        }

        public static StockEventReturn Map(this StockEvent p)
        {
            return new StockEventReturn()
            {
                Count = p.Count,
                Id = p.Id,
                PortfolioId = p.PortfolioId,
                Type = p.Type,
                Price = p.Price,
                Date = p.Date,
                CurrencyId = p.CurrencyId,
                CurrencyName = p.Currency?.Name,
                StockElementId = p.StockElementId,
                StockName = p.StockElement?.Stock?.Name,
            };
        }

        public static CreateStock Map(this CreateStockRequest p)
        {
            return new CreateStock()
            {
                Id = p.Id,
                //PortfolioId = p.PortfolioId,
                Type = p.Type,
                Code = p.Code,
                IsGlobal = p.IsGlobal,
                Name = p.Name,
            };
        }

        public static StockReturn Map(this Stock p)
        {
            return new StockReturn()
            {
                Id = p.Id,
                //PortfolioId = p.PortfolioId,
                Type = p.Type,
                Code = p.Code,
                IsGlobal = p.IsGlobal,
                Name = p.Name,
                ActualizationTime = p.ActualizationTime,
                CurrencyId = p.CurrencyId,
                LastPrice = p.LastPrice,
            };
        }

        public static StockHistoryReturn Map(this StockHistory p)
        {
            return new StockHistoryReturn()
            {
                Id = p.Id,
                //PortfolioId = p.PortfolioId,
                CurrencyId = p.CurrencyId,
                Date = p.Date,
                Price = p.Price,
                StockId = p.StockId,
                CurrencyName = p.Currency?.Name,

            };
        }


        public static StockHistory Map(this CreateStockHistoryRequest p)
        {
            return new StockHistory()
            {
                Id = p.Id,
                //PortfolioId = p.PortfolioId,
                CurrencyId = p.CurrencyId,
                Date = p.Date,
                Price = p.Price,
                StockId = p.StockId,

            };
        }
        



    }
}
