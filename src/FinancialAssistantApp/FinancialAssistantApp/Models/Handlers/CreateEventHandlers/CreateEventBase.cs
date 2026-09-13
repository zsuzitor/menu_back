using BL.Models.Services.Interfaces;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using Common.Models.Exceptions;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.Handlers.CreateEventHandlers
{
    public abstract class CreateEventBase
    {
        protected abstract StockEventEnum Type { get; }
        protected long UserId { get; }


        private protected IStockRepository _stockRepository;
        private protected IDateTimeProvider _datetimProvider;
        private protected IPortfolioRepository _portfolioRepository;
        private protected IStockElementRepository _stockElementRepository;
        private protected IStockEventRepository _stockEventRepository;


        protected CreateEventBase(long userId, IStockRepository stockRepository, IDateTimeProvider datetimProvider
            , IPortfolioRepository portfolioRepository, IStockElementRepository stockElementRepository, IStockEventRepository stockEventRepository)
        {
            UserId = userId;
            _stockRepository = stockRepository;
            _datetimProvider = datetimProvider;
            _portfolioRepository = portfolioRepository;
            _stockElementRepository = stockElementRepository;
            _stockEventRepository = stockEventRepository;
        }





        public async Task<StockEvent> CreateEvent(StockEventCreate obj)
        {

            if (!Enum.IsDefined(typeof(StockEventEnum), obj.Type))
            {
                throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
            }

            if (obj.CurrencyId <= 0)
            {
                obj.CurrencyId = null;
            }

            if (obj.StockId <= 0)
            {
                obj.StockId = null;
            }

            if (obj.Price <= 0)
            {
                obj.Price = null;
            }

            await ValidateRequest(obj);
            if (!await _portfolioRepository.ExistAsync(obj.PortfolioId, UserId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }

            var stock = await GetStock(obj);

            var element = await GetStockElement(obj);
            if (element != null)
            {
                if (element.Id > 0)
                {

                    element = await _stockElementRepository.UpdateAsync(element);
                }
                else
                {
                    element = await _stockElementRepository.AddAsync(element);
                }
            }

            var currency = await GetCurrency(obj);
            var elementCurrency = await GetCurrencyElement(obj);
            if (elementCurrency != null)
            {
                if (elementCurrency.Id > 0)
                {

                    elementCurrency = await _stockElementRepository.UpdateAsync(elementCurrency);
                }
                else
                {
                    elementCurrency = await _stockElementRepository.AddAsync(elementCurrency);
                }
            }

            var newObj = new StockEvent()
            {
                Date = _datetimProvider.CurrentDateTime(),
                Count = obj.Count,
                Type = obj.Type,
                StockElementId = element?.Id ?? elementCurrency.Id,
                CurrencyId = obj.CurrencyId,
                Price = obj.Price,
                PortfolioId = obj.PortfolioId
            };

            var result = await _stockEventRepository.AddAsync(newObj);


            result.Currency = currency;
            result.StockElement = element ?? elementCurrency;
            result.StockElement.Stock = stock ?? currency;
            return result;

        }


        protected abstract Task ValidateRequest(StockEventCreate obj);
        protected abstract Task<StockElement> GetStockElement(StockEventCreate obj);
        protected abstract Task<StockElement> GetCurrencyElement(StockEventCreate obj);
        protected abstract Task<Stock> GetCurrency(StockEventCreate obj);
        protected abstract Task<Stock> GetStock(StockEventCreate obj);


    }
}
