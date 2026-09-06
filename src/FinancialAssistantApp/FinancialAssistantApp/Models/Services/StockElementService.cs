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

            var elements = await _stockElementRepository.GetWithStockNoTrack(portfolioId);
            var allCurr = await _stockRepository.GetCurrencyAsync(userId);
            //загрузить stock для элемента - загружены
            //загрузить валюту для stock
            //загрузить валюту для портфеля
            //ищем 
            //если валюта сток не
            if (!portfolio.CurrencyId.HasValue)
            {
                var elementsResult = elements.Select(x => Mapper.Mapper.Map(x)).ToList();
                foreach (var elementResult in elementsResult) {
                    var c = allCurr.FirstOrDefault(x => x.Id == elementResult.CurrencyId);
                    elementResult.CurrencyName = c?.Name;
                }
                return elementsResult;
            }

            var result = new List<StockElementInPortfolio>();//elements.Select(x => Mapper.Mapper.Map(x)).ToList();

                var portfolioCurrency = allCurr.FirstOrDefault(x=>x.Id==portfolio.CurrencyId);
                var price = new List<(long curFrom, decimal? onePrice)>();
                foreach (var el in elements)
                {
                    if(el.Stock.CurrencyId == null)
                    {
                        continue;
                    }

                    var elPrice = price.FirstOrDefault(x => x.curFrom == el.Stock.CurrencyId);
                    if (elPrice == default)
                    {
                        var onePrice = new CurrencyConvertHandler().ToCurrency(allCurr, el.Stock.CurrencyId.Value,1,  portfolio.CurrencyId.Value);
                        elPrice = (el.Stock.CurrencyId.Value, onePrice);
                        price.Add(elPrice);
                    }

                    var oneRes = Mapper.Mapper.Map(el);
                    if (elPrice.onePrice != null)
                    {
                        oneRes.Price = elPrice.onePrice.Value * el.Stock.LastPrice;
                        oneRes.CurrencyId = portfolio.CurrencyId;
                        oneRes.CurrencyName = portfolioCurrency.Name;

                    }
                    result.Add(oneRes);
                }

            



            return result;
        }



    }
}
