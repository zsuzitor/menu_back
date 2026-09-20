using BO.Models.FinancialAssistant.DAL;
using FinancialAssistantApp.Models.DTO;
using Menu.Host.Models.FinancialAssistantApp.Requests;
using Menu.Host.Models.FinancialAssistantApp.Returns;
using System;

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


        public static PortfolioStatisticReturn Map(this PortfolioStatistic p)
        {
            return new PortfolioStatisticReturn()
            {
                CashReplenishmentSum = p.CashReplenishmentSum,
                ReplenishmentsByCurrency = p.ReplenishmentsByCurrency,
                WithdrawalCashByCurrency = p.WithdrawalCashByCurrency,
                WithdrawalCashSum = p.WithdrawalCashSum,
                DividendsCashByCurrency = p.DividendsCashByCurrency,
                DividendsCashSum = p.DividendsCashSum,
                SumNow = p.SumNow,
                SumOnEndPeriod = p.SumOnEndPeriod,
                SumOnStartPeriod = p.SumOnStartPeriod,
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

        public static PortfolioStatisticRequestDto Map(this Requests.PortfolioStatisticRequest p)
        {
            return new PortfolioStatisticRequestDto()
            {
                CurrencyId = p.CurrencyId,
                End = p.End,
                PortfolioId = p.PortfolioId,
                Start = p.Start
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
                Price = Math.Round(p.Price, 3),//.Normalize()// тут из за конвертации валют страшное число получается
                Sum = Math.Round(p.Count * p.Price, 3),
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
                CurrencyActions = p.CurrencyActions,
            };
        }

        public static StockEventReturn Map(this StockEvent p)
        {
            return new StockEventReturn()
            {
                Count = p.MainCountChange,
                Id = p.Id,
                PortfolioId = p.PortfolioId,
                Type = p.Type,
                Price = p.SubCountChange,
                Date = p.Date,
                CurrencyId = p.SubElement?.StockId,
                CurrencyName = p.SubElement?.Stock?.Name,
                StockElementId = p.MainElementId,
                StockName = p.MainElement?.Stock?.Name,
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
