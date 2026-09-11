using BL.Models.Services.Interfaces;
using BO.Models.DAL.Domain;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using Common.Models.Exceptions;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using FinancialAssistantApp.Models.Services.Interfaces;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using Tinkoff.InvestApi.V1;

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

            if (obj.Count <= 0 && obj.Type != StockEventEnum.CountChange)
            {
                //для CountChange свои правила тк это костыль по сути
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

            if (obj.Type == StockEventEnum.CashReplenishment || obj.Type == StockEventEnum.WithdrawalCash)
            {
                if (stock.Type != StockTypeEnum.Currency)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotValideStockEvent);

                }
                obj.CurrencyId = null;
            }

            if (obj.Type == StockEventEnum.CountChange)
            {
                obj.CurrencyId = null;
                obj.CurrencyActions = false;
            }

            if (obj.Type == StockEventEnum.Dividends)
            {
                obj.CurrencyActions = true;
                obj.Count = 1;
            }


            if (((obj.Price != null) && (obj.CurrencyId == null))
                || ((obj.Price == null) && (obj.CurrencyId != null)))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotValideStockEvent);
            }



            Stock currency = await GetCurrencyWithValidate(obj.CurrencyId, userId);


            var element = await _stockElementRepository.Get(obj.PortfolioId, obj.StockId);
            if (obj.Type != StockEventEnum.Dividends && obj.Type != StockEventEnum.CashReplenishment && obj.Type != StockEventEnum.WithdrawalCash)
            {
                // если ивенты чисто денежные то стока не будет, менять нечего, работаем с currency

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


            }


            if (currency != null && obj.CurrencyActions)
            {
                //списываем деньги
                var currencyElement = await _stockElementRepository.Get(obj.PortfolioId, obj.CurrencyId.Value);
                if (currencyElement == null)
                {
                    var elem = new StockElement()
                    {
                        StockId = currency.Id,
                        Count = obj.Type == StockEventEnum.Sell || obj.Type == StockEventEnum.Dividends ? obj.Price.Value * obj.Count : obj.Price.Value * -1 * obj.Count,
                        PortfolioId = obj.PortfolioId,
                    };
                    currencyElement = await _stockElementRepository.AddAsync(elem);
                }
                else
                {
                    currencyElement.Count += obj.Type == StockEventEnum.Sell || obj.Type == StockEventEnum.Dividends ? obj.Price.Value * obj.Count : obj.Price.Value * -1 * obj.Count;
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

            var result =  await _stockEventRepository.AddAsync(newObj);

            result.Currency = currency;
            result.StockElement = element;
            result.StockElement.Stock = stock;
            return result;

        }

        public async Task<List<StockEvent>> GetForPortfolioAsync(long portfolioId, long userId)
        {
            if (!await _portfolioRepository.ExistAsync(portfolioId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }

            return await _stockEventRepository.GetForPortfolioAsync(portfolioId);
        }

        private async Task<Stock> GetCurrencyWithValidate(long? currencyId, long userId)
        {
            //todo вынести куда то в 1 место
            Stock currency = null;
            if (currencyId != null)
            {

                currency = await _stockRepository.GetNoTrackAsync(currencyId.Value) ?? throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
                if (!currency.IsGlobal && currency.UserId != userId)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);

                }
                if ((currency.Type != StockTypeEnum.Currency))
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundCurrency);
            }

            return currency;
        }

    }
}
