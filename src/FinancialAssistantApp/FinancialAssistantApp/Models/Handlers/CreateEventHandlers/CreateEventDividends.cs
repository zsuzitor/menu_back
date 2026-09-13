using BL.Models.Services.Interfaces;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using Common.Models.Exceptions;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.Handlers.CreateEventHandlers
{
    public class CreateEventDividends : CreateEventBase
    {
        public CreateEventDividends(long userId, IStockRepository stockRepository, IDateTimeProvider datetimProvider,
            IPortfolioRepository portfolioRepository, IStockElementRepository stockElementRepository, IStockEventRepository stockEventRepository)
            : base(userId, stockRepository, datetimProvider, portfolioRepository, stockElementRepository, stockEventRepository)
        {
        }

        protected override StockEventEnum Type => StockEventEnum.Dividends;



        protected override async Task<Stock> GetCurrency(StockEventCreate obj)
        {
            return await _stockRepository.GetCurrencyWithValidate(obj.CurrencyId, UserId);

        }

        protected override async Task<StockElement> GetCurrencyElement(StockEventCreate obj)
        {



            //списываем деньги
            var currencyElement = await _stockElementRepository.Get(obj.PortfolioId, obj.CurrencyId.Value);
            if (currencyElement == null)
            {
                var elem = new StockElement()
                {
                    StockId = obj.CurrencyId.Value,
                    Count = obj.CurrencyActions ?  obj.Count:0 ,
                    PortfolioId = obj.PortfolioId,
                };
                currencyElement = elem;
            }
            else
            {
                currencyElement.Count += obj.CurrencyActions ? obj.Count:0;
            }

            return currencyElement;
        }

        protected override async Task<Stock> GetStock(StockEventCreate obj)
        {
            var stock = await _stockRepository.GetNoTrackAsync(obj.StockId.Value) ?? throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
            if (!stock.IsGlobal && stock.UserId != UserId)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);

            }

            return stock;
        }

        protected override async Task<StockElement> GetStockElement(StockEventCreate obj)
        {
            // если ивенты чисто денежные то стока не будет, менять нечего, работаем с currency
            return null;

        }

        protected override async Task ValidateRequest(StockEventCreate obj)
        {

            if (obj.StockId == null || obj.StockId <= 0)
            {
                throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
            }
            if (obj.Price == null || obj.CurrencyId == null

    //            ((obj.Price != null) && (obj.CurrencyId == null))
    //|| ((obj.Price == null) && (obj.CurrencyId != null)))
    )
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotValideStockEvent);
            }

            obj.Count = obj.Price.Value;
            obj.Price = 1;
        }
    }
}