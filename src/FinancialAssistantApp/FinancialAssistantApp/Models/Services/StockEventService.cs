using BL.Models.Services.Interfaces;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using Common.Models.Exceptions;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using FinancialAssistantApp.Models.Services.Interfaces;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.Services
{
    public class StockEventService : IStockEventService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IDateTimeProvider _datetimProvider;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IStockElementRepository _stockElementRepository;
        private readonly IStockEventRepository _stockEventRepository;

        public StockEventService(IStockRepository stockRepository, IDateTimeProvider datetimProvider, IPortfolioRepository portfolioRepository, IStockElementRepository stockElementRepository, IStockEventRepository stockEventRepository)
        {
            _stockRepository = stockRepository;
            _datetimProvider = datetimProvider;
            _portfolioRepository = portfolioRepository;
            _stockElementRepository = stockElementRepository;
            _stockEventRepository = stockEventRepository;
        }


        public async Task<StockEvent> CreateEventAsync(StockEventCreate obj, long userId)
        {
            //списать деньги
            //начислить акции
            //создать сток если его нет?
            //todo транзакция

            if (obj.CurrencyId <= 0)
            {
                obj.CurrencyId = null;
            }

            if (obj.Price <= 0)
            {
                obj.Price = null;
            }

            if (!Enum.IsDefined(typeof(StockEventEnum), obj.Type))
            {
                throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
            }

            if (obj.Count <= 0)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotValideStockEvent);
            }



            if (!await _portfolioRepository.ExistAsync(obj.PortfolioId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }

            var stock = await _stockRepository.GetNoTrackAsync(obj.StockId) ?? throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
            if (!stock.IsGlobal && stock.UserId != userId)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);

            }

            if (obj.Type == StockEventEnum.CashReplenishment || obj.Type == StockEventEnum.Dividends || obj.Type == StockEventEnum.WithdrawalCash)
            {
                if (stock.Type != StockTypeEnum.Currency)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotValideStockEvent);

                }
                obj.CurrencyId = null;
            }


            if (((obj.Price != null) && (obj.CurrencyId == null))
                || ((obj.Price == null) && (obj.CurrencyId != null)))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotValideStockEvent);
            }



            Stock currency = null;
            if (obj.CurrencyId != null)
            {

                currency = await _stockRepository.GetNoTrackAsync(obj.CurrencyId.Value) ?? throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
                if (!currency.IsGlobal && currency.UserId != userId)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);

                }
                if ((currency.Type != StockTypeEnum.Currency))
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundCurrency);
            }

            var element = await _stockElementRepository.Get(obj.PortfolioId, obj.StockId);
            if (element == null)
            {
                var elem = new StockElement()
                {
                    StockId = stock.Id,
                    Count = obj.Type == StockEventEnum.Sell || obj.Type == StockEventEnum.WithdrawalCash ? obj.Count * -1 : obj.Count,
                    PortfolioId = obj.PortfolioId,
                };
                element = await _stockElementRepository.AddAsync(elem);
            }
            else
            {
                element.Count += obj.Type == StockEventEnum.Sell || obj.Type == StockEventEnum.WithdrawalCash ? obj.Count * -1 : obj.Count;
                element = await _stockElementRepository.UpdateAsync(element);
            }

            if (currency != null)
            {
                //списываем деньги
                var currencyElement = await _stockElementRepository.Get(obj.PortfolioId, obj.CurrencyId.Value);
                if (currencyElement == null)
                {
                    var elem = new StockElement()
                    {
                        StockId = currency.Id,
                        Count = obj.Type == StockEventEnum.Sell ? obj.Price.Value * -1 : obj.Price.Value,
                        PortfolioId = obj.PortfolioId,
                    };
                    currencyElement = await _stockElementRepository.AddAsync(elem);
                }
                else
                {
                    currencyElement.Count += obj.Type == StockEventEnum.Sell ? obj.Price.Value * -1 : obj.Price.Value;
                    currencyElement = await _stockElementRepository.UpdateAsync(currencyElement);
                }
            }

            var newObj = new StockEvent()
            {
                Date = _datetimProvider.CurrentDateTime(),
                Count = obj.Count,
                Type = obj.Type,
                StockElementId = element.Id,
                CurrencyId = obj.CurrencyId,
                Price = obj.Price,
                PortfolioId = obj.PortfolioId
            };

            return await _stockEventRepository.AddAsync(newObj);
            //todo списать деньги

        }

    }
}
