using BL.Models.Services.Interfaces;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using Common.Models.Exceptions;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using Microsoft.AspNetCore.SignalR;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.Handlers.CreateEventHandlers
{
    public class CreateEventCashReplenishment : CreateEventBase
    {
        public CreateEventCashReplenishment(long userId, IStockRepository stockRepository, IDateTimeProvider datetimProvider,
            IPortfolioRepository portfolioRepository, IStockElementRepository stockElementRepository, IStockEventRepository stockEventRepository)
            : base(userId, stockRepository, datetimProvider, portfolioRepository, stockElementRepository, stockEventRepository)
        {
        }

        protected override  StockEventEnum Type => StockEventEnum.CashReplenishment;
        private StockElement Main;



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
                    Count = obj.CurrencyActions ?  obj.Count: 0,
                    PortfolioId = obj.PortfolioId,
                };
                currencyElement = elem;
            }
            else
            {
                currencyElement.Count += obj.CurrencyActions ?  obj.Count : 0;
            }

            Main = currencyElement;
            return currencyElement;
        }

        protected override async Task<Stock> GetStock(StockEventCreate obj)
        {
            return null;
        }

        protected override async Task<StockElement> GetStockElement(StockEventCreate obj)
        {
            // если ивенты чисто денежные то стока не будет, менять нечего, работаем с currency
            return null;
        }

        protected override async Task<bool> StockElementCanChange(StockEventCreate obj)
        {
            return false;
        }

        protected override async Task ValidateRequest(StockEventCreate obj)
        {
            obj.StockId = null;
            if (obj.Price == null || obj.CurrencyId == null
                || obj.Price <= 0 || obj.CurrencyId <= 0

    //            ((obj.Price != null) && (obj.CurrencyId == null))
    //|| ((obj.Price == null) && (obj.CurrencyId != null)))
    )
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotValideStockEvent);
            }

            obj.Count = obj.Price.Value;
            obj.Price = 1;
        }

        protected override async Task<StockEvent> GetStockEvent(StockEventCreate obj)
        {
            var newObj = new StockEvent()
            {
                Date = _datetimProvider.CurrentDateTime(),
                MainCountChange = obj.Count,
                MainCountNow = Main.Count,
                Type = obj.Type,
                MainElementId = Main.Id,
                PortfolioId = obj.PortfolioId,
            };

            return newObj;
        }
    }
}
