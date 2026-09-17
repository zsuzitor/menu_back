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
    public class CreateEventCountChange : CreateEventBase
    {
        public CreateEventCountChange(long userId, IStockRepository stockRepository, IDateTimeProvider datetimProvider,
            IPortfolioRepository portfolioRepository, IStockElementRepository stockElementRepository, IStockEventRepository stockEventRepository)
            : base(userId, stockRepository, datetimProvider, portfolioRepository, stockElementRepository, stockEventRepository)
        {
        }

        protected override StockEventEnum Type => StockEventEnum.CountChange;
        private StockElement Main;



        protected override async Task<Stock> GetCurrency(StockEventCreate obj)
        {
            return null;
        }

        protected override async Task<StockElement> GetCurrencyElement(StockEventCreate obj)
        {
            return null;
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
            var element = await _stockElementRepository.Get(obj.PortfolioId, obj.StockId.Value);

            if (element == null)
            {
                var elem = new StockElement()
                {
                    StockId = obj.StockId.Value,
                    Count =  obj.Count,
                    PortfolioId = obj.PortfolioId,
                };
                element = elem;
            }
            else
            {
                element.Count +=  obj.Count;
            }
            Main = element;
            return element;

        }

        protected override async Task ValidateRequest(StockEventCreate obj)
        {

            obj.CurrencyId = null;
            obj.Price = null;
            if (obj.StockId == null || obj.StockId <= 0)
            {
                throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
            }
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

        protected override async Task<bool> StockElementCanChange(StockEventCreate obj)
        {
            return true;
        }
    }
}