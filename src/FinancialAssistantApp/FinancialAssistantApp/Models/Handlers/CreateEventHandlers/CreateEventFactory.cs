using BL.Models.Services.Interfaces;
using BO.Models.FinancialAssistant.Enums;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.Handlers.CreateEventHandlers
{
    public class CreateEventFactory
    {
        private protected IStockRepository _stockRepository;
        private protected IDateTimeProvider _datetimProvider;
        private protected IPortfolioRepository _portfolioRepository;
        private protected IStockElementRepository _stockElementRepository;
        private protected IStockEventRepository _stockEventRepository;


        public CreateEventFactory(IStockRepository stockRepository, IDateTimeProvider datetimProvider
            , IPortfolioRepository portfolioRepository, IStockElementRepository stockElementRepository, IStockEventRepository stockEventRepository)
        {
            _stockRepository = stockRepository;
            _datetimProvider = datetimProvider;
            _portfolioRepository = portfolioRepository;
            _stockElementRepository = stockElementRepository;
            _stockEventRepository = stockEventRepository;
        }

        public CreateEventBase Get(StockEventEnum type, long userId)
        {
            switch (type)
            {
                case StockEventEnum.CashReplenishment:
                    return new CreateEventCashReplenishment(userId, _stockRepository, _datetimProvider, _portfolioRepository, _stockElementRepository, _stockEventRepository);
                    break;
                case StockEventEnum.Buy:
                    return new CreateEventBuy(userId, _stockRepository, _datetimProvider, _portfolioRepository, _stockElementRepository, _stockEventRepository);
                    break;
                case StockEventEnum.Sell:
                    return new CreateEventSell(userId, _stockRepository, _datetimProvider, _portfolioRepository, _stockElementRepository, _stockEventRepository);
                    break;
                case StockEventEnum.Dividends:
                    return new CreateEventDividends(userId, _stockRepository, _datetimProvider, _portfolioRepository, _stockElementRepository, _stockEventRepository);
                    break;
                case StockEventEnum.WithdrawalCash:
                    return new CreateEventWithdrawalCash(userId, _stockRepository, _datetimProvider, _portfolioRepository, _stockElementRepository, _stockEventRepository);
                    break;
                case StockEventEnum.CountChange:
                    return new CreateEventCountChange(userId, _stockRepository, _datetimProvider, _portfolioRepository, _stockElementRepository, _stockEventRepository);
                    break;
                default:
                    throw new Exception("Not Found StockEventEnum");
                    break;
            }
        }
    }
}
