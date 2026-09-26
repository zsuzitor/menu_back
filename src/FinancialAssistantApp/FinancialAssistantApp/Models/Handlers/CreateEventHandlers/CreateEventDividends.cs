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
    public class CreateEventDividends : CreateEventBase
    {
        public CreateEventDividends(long userId, IStockRepository stockRepository, IDateTimeProvider datetimProvider,
            IPortfolioRepository portfolioRepository, IStockElementRepository stockElementRepository, IStockEventRepository stockEventRepository)
            : base(userId, stockRepository, datetimProvider, portfolioRepository, stockElementRepository, stockEventRepository)
        {
        }

        protected override StockEventEnum Type => StockEventEnum.Dividends;
        private StockElement Main;
        private StockElement Sub;
        private decimal OldCurrencyValue = 0;



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
                    Count = obj.CurrencyActions ?  obj.Price.Value:0 ,
                    PortfolioId = obj.PortfolioId,
                };
                currencyElement = elem;
            }
            else
            {
                OldCurrencyValue = currencyElement.Count;
                currencyElement.Count += obj.CurrencyActions ? obj.Price.Value : 0;
            }

            Sub = currencyElement;
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

            var element = await _stockElementRepository.Get(obj.PortfolioId, obj.StockId.Value);

            if (element == null)
            {
                var elem = new StockElement()
                {
                    StockId = obj.StockId.Value,
                    Count = 0,
                    PortfolioId = obj.PortfolioId,
                };
                element = elem;
            }
            Main = element;
            return element;

        }

        protected override async Task ValidateRequest(StockEventCreate obj)
        {

            if (obj.StockId == null || obj.StockId <= 0)
            {
                throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
            }
            if (obj.Price == null || obj.CurrencyId == null
                || obj.Price<=0 || obj.CurrencyId<=0

    //            ((obj.Price != null) && (obj.CurrencyId == null))
    //|| ((obj.Price == null) && (obj.CurrencyId != null)))
    )
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotValideStockEvent);
            }

            //obj.Count = obj.Price.Value;
            //obj.Price = 1;
        }

        protected override async Task<StockEvent> GetStockEvent(StockEventCreate obj)
        {
            var newObj = new StockEvent()
            {
                EventDateTime = obj.Date,
                CreationDateTime = _datetimProvider.CurrentDateTime(),
                MainCountChange = 0,
                MainCountNow = Main.Count,
                Type = obj.Type,
                MainElementId = Main.Id,
                PortfolioId = obj.PortfolioId,
                SubCountChange = obj.Price,
                SubCountNow = Sub.Count,
                SubCountOldValue = OldCurrencyValue,
                SubElementId = Sub.Id,
            };

            return newObj;
        }

        protected override async Task<bool> StockElementCanChange(StockEventCreate obj)
        {
            //так как в StockEvent есть ссылка на главный элемент, может ыть кейс когда у чела в портфеле нет акции а он хочет с нее дивиденды записать,
            //тогда мы не найдем элемент и упадем, поэтому элемент всегда создаем\апдейтим
            return true;
        }

        public override List<StockEvent> GetRollBackCountChange(StockEvent obj)
        {
            var mainObj = new StockEvent()
            {
                Type = StockEventEnum.CountChange,
                CreationDateTime = _datetimProvider.CurrentDateTime(),
                EventDateTime = _datetimProvider.CurrentDateTime(),
                //obj.EventDateTime,
                MainCountChange = 0,
                MainCountNow = obj.MainCountNow,
                MainElementId = obj.MainElementId,
                PortfolioId = obj.PortfolioId,
            };
            var subObj = new StockEvent()
            {
                Type = StockEventEnum.CountChange,
                CreationDateTime = _datetimProvider.CurrentDateTime(),
                EventDateTime = _datetimProvider.CurrentDateTime(),//obj.EventDateTime,   ту же дату нельзя ставить потому что может сломаться статистика и тд, в 1 секунду и пополнение и снятие
                MainCountChange = obj.SubCountChange.Value *-1,
                MainCountNow = obj.SubCountNow.Value - obj.SubCountChange.Value,
                MainElementId = obj.SubElementId.Value,
                PortfolioId = obj.PortfolioId,
            };
            var result = new List<StockEvent>() { mainObj, subObj };
            return result;

        }
    }
}