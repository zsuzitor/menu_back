using Common.Models.Exceptions;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using FinancialAssistantApp.Models.Handlers;
using FinancialAssistantApp.Models.Services.Interfaces;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.Services
{
    public class StockElementService : IStockElementService
    {
        private readonly IStockElementRepository _stockElementRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public StockElementService(IStockElementRepository stockElementRepository, IPortfolioRepository portfolioRepository, IStockRepository stockRepository)
        {
            _stockElementRepository = stockElementRepository;
            _portfolioRepository = portfolioRepository;
            _stockRepository = stockRepository;
        }

        public async Task<List<StockElementInPortfolio>> Get(long portfolioId, long userId)
        {
            var portfolio = await _portfolioRepository.GetAsync(portfolioId, userId) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);

            var elements = await _stockElementRepository.GetWithStockNotEmptyNoTrack(portfolioId);
            var allCurr = await _stockRepository.GetCurrencyAsync(userId);
            //загрузить stock для элемента - загружены
            //загрузить валюту для stock
            //загрузить валюту для портфеля
            //ищем 
            //если валюта сток не
            if (!portfolio.CurrencyId.HasValue)
            {
                var elementsResult = elements.Select(x => Mapper.Mapper.Map(x)).ToList();
                foreach (var elementResult in elementsResult)
                {
                    var c = allCurr.FirstOrDefault(x => x.Id == elementResult.CurrencyId);
                    elementResult.CurrencyName = c?.Name;
                }
                return elementsResult;
            }

            var result = new List<StockElementInPortfolio>();//elements.Select(x => Mapper.Mapper.Map(x)).ToList();

            var portfolioCurrency = allCurr.FirstOrDefault(x => x.Id == portfolio.CurrencyId);
            var countedPrices = new List<(long currencyFrom, decimal? onePrice)>();
            foreach (var oneElement in elements)
            {
                if (oneElement.Stock.CurrencyId == null)
                {
                    if (oneElement.Stock.Type == BO.Models.FinancialAssistant.Enums.StockTypeEnum.Currency)
                    {
                        //для вылюты записываем цену 1 к 1 в себе же, что бы попытаться рассчитать через обратные курсы
                        oneElement.Stock.CurrencyId = oneElement.Stock.Id;
                        oneElement.Stock.LastPrice = 1;
                    }
                    else
                    {
                        var oneRess = Mapper.Mapper.Map(oneElement);
                        oneRess.CurrencyName = allCurr.FirstOrDefault(x => x.Id == oneRess.CurrencyId)?.Name;
                        result.Add(oneRess);
                        continue;
                    }
                }

                var elPrice = countedPrices.FirstOrDefault(x => x.currencyFrom == oneElement.Stock.CurrencyId);
                if (elPrice == default)
                {
                    var onePrice = new CurrencyConvertHandler().ToCurrency(allCurr.Select(x => new CurrencyConvertHandler.ConvertElement()
                    {
                        IdFrom = x.Id,
                        IdTo = x.CurrencyId,
                        Price = x.LastPrice
                    }).ToList(), oneElement.Stock.CurrencyId.Value, 1, portfolio.CurrencyId.Value);
                    elPrice = (oneElement.Stock.CurrencyId.Value, onePrice);
                    countedPrices.Add(elPrice);
                }

                var oneRes = Mapper.Mapper.Map(oneElement);
                if (elPrice.onePrice != null)
                {
                    oneRes.Price = elPrice.onePrice.Value * oneElement.Stock.LastPrice;
                    oneRes.CurrencyId = portfolio.CurrencyId;
                    oneRes.CurrencyName = portfolioCurrency.Name;
                    oneRes.Sum = oneRes.Price * oneRes.Count;
                }

                oneRes.CurrencyName = allCurr.FirstOrDefault(x => x.Id == oneRes.CurrencyId)?.Name;

                result.Add(oneRes);
            }





            return result;
        }



    }
}
