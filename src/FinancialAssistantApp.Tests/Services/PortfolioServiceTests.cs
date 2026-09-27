using AutoFixture;
using BL.Models.Services.Interfaces;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using BO.Models.TaskManagementApp.DAL.Domain;
using FinancialAssistantApp.Models;
using FinancialAssistantApp.Models.DAL.Repositories;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using FinancialAssistantApp.Models.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using Xunit;
namespace FinancialAssistantApp.Tests.Services
{
    public class PortfolioServiceTests
    {

        private readonly IFixture _fixture;

        public PortfolioServiceTests(IFixture fixture)
        {
            _fixture = fixture;
        }



        [Fact]
        public async Task GetStatusesAsync_OneStatus_Success()
        {
            var services = DefaultInit();

            var portfolioRepo = AddMock<IPortfolioRepository>(services);
            var stockRepository = AddMock<IStockRepository>(services);
            var stockElementRepository = AddMock<IStockElementRepository>(services);
            var stockEventRepository = AddMock<IStockEventRepository>(services);
            var datetimeProvider = AddMock<IDateTimeProvider>(services);
            var datetimeNow = _fixture.Build<DateTime>().Create();
            var userId = _fixture.Build<long>().Create();
            var portfolio1 = _fixture.Build<Portfolio>().With(x => x.UserId, userId).Create();
            var portfolio2 = _fixture.Build<Portfolio>().With(x => x.UserId, userId).Create();
            var portfolio3 = _fixture.Build<Portfolio>().With(x => x.UserId, userId).Create();
            var statisticReq = new PortfolioStatisticRequestDto() {
                CurrencyId = 1,
                End = datetimeNow.AddYears(-1),
                Start = datetimeNow.AddYears(-2),
                PortfolioId = new List<long>() { portfolio1.Id, portfolio2.Id }
            };

            datetimeProvider.Setup(x => x.CurrentDateTime())
                .Returns(datetimeNow);

            portfolioRepo.Setup(x => x.GetAllAsync(statisticReq.PortfolioId, userId))
                .Returns(Task.FromResult(new List<Portfolio>() { portfolio1, portfolio2, portfolio3 }));

            long currencyStrongId = 2;

            var stockCurrencyCheap = _fixture.Build<Stock>().With(x => x.UserId, userId)
                .With(x=>x.ActualizationTime,datetimeNow)
                .With(x=>x.Type,BO.Models.FinancialAssistant.Enums.StockTypeEnum.Currency)
                .With(x => x.ActualizationTime, datetimeNow)
                .With(x => x.LastPrice, 0.01m)
                .With(x => x.CurrencyId, currencyStrongId)
                .With(x => x.Id, 1)
                .With(x=>x.StockHistory,new List<StockHistory>()
                {
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, currencyStrongId)
                        .With(x => x.Date, statisticReq.Start.AddDays(-9)).With(x => x.Price, 0.02m).With(x => x.StockId, 1).Create(),//price 50
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, currencyStrongId)
                        .With(x => x.Date, statisticReq.Start.AddDays(3)).With(x => x.Price, 0.0125m).With(x => x.StockId, 1).Create(),//price 80
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, currencyStrongId)
                        .With(x => x.Date, statisticReq.Start.AddDays(30)).With(x => x.Price, 0.01m).With(x => x.StockId, 1).Create(),//price 100
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, currencyStrongId)
                        .With(x => x.Date, statisticReq.Start.AddDays(50)).With(x => x.Price, 0.008m).With(x => x.StockId, 1).Create(),//125
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, currencyStrongId)
                        .With(x => x.Date, statisticReq.End.AddDays(9)).With(x => x.Price, 0.02m).With(x => x.StockId, 1).Create(),//price 50
                }
                )
                .Create();


            var elementCurrencyCheap = _fixture.Build<StockElement>().With(x => x.StockId, stockCurrencyCheap.Id)
                .With(x => x.Stock, stockCurrencyCheap)
                .With(x => x.Count, 100000)
                .With(x => x.PortfolioId, portfolio1.Id)
                .Create();

            var elementCurrencyCheapPortfolio2 = _fixture.Build<StockElement>().With(x => x.StockId, stockCurrencyCheap.Id)
                .With(x => x.Stock, stockCurrencyCheap)
                .With(x => x.Count, 10000)
                .With(x => x.PortfolioId, portfolio2.Id)
                .Create();

            var stockCurrencyStrong = _fixture.Build<Stock>().With(x => x.UserId, userId)
                .With(x => x.Type, BO.Models.FinancialAssistant.Enums.StockTypeEnum.Currency)
                .With(x => x.ActualizationTime, datetimeNow)
                .With(x => x.LastPrice, 100m)
                .With(x => x.CurrencyId, stockCurrencyCheap.Id)
                .With(x => x.Id, currencyStrongId)
                .With(x => x.StockHistory, new List<StockHistory>()
                {
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(-9)).With(x => x.Price, 50).With(x => x.StockId, currencyStrongId).Create(),//price 50
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(3)).With(x => x.Price, 8).With(x => x.StockId, currencyStrongId).Create(),//price 80
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(30)).With(x => x.Price, 100).With(x => x.StockId, currencyStrongId).Create(),//price 100
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(50)).With(x => x.Price, 125).With(x => x.StockId, currencyStrongId).Create(),//125
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.End.AddDays(9)).With(x => x.Price, 50).With(x => x.StockId, currencyStrongId).Create(),//price 50
                }
                )
                .Create();

            var elementCurrencyStrong = _fixture.Build<StockElement>().With(x => x.StockId, stockCurrencyStrong.Id)
                .With(x => x.Stock, stockCurrencyStrong)
                .With(x => x.Count, 666)
                .With(x => x.PortfolioId, portfolio2.Id)
                .Create();


            var stockInvestStockId = _fixture.Build<long>().Create();
            var stockInvestStock = _fixture.Build<Stock>().With(x => x.UserId, userId)
                .With(x => x.Id, stockInvestStockId)
                .With(x => x.Type, BO.Models.FinancialAssistant.Enums.StockTypeEnum.InvestmentStock)
                .With(x => x.ActualizationTime, datetimeNow)
                .With(x => x.LastPrice, 5000m)
                .With(x => x.CurrencyId, stockCurrencyCheap.Id)
                .With(x => x.StockHistory, new List<StockHistory>()
                {
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(-9)).With(x => x.Price, 2000).With(x => x.StockId, stockInvestStockId).Create(),
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(3)).With(x => x.Price, 4500).With(x => x.StockId, stockInvestStockId).Create(),
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(30)).With(x => x.Price, 5000).With(x => x.StockId, stockInvestStockId).Create(),
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(50)).With(x => x.Price, 5500).With(x => x.StockId, stockInvestStockId).Create(),
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.End.AddDays(9)).With(x => x.Price, 6000).With(x => x.StockId, stockInvestStockId).Create(),
                }
                )
                //.With(x => x.Id, 3)
                .Create();

            var stockInvestStockPortfolioNotSelectId = _fixture.Build<long>().Create();
            var stockInvestStockPortfolioNotSelect = _fixture.Build<Stock>().With(x => x.UserId, userId)
                .With(x => x.Id, stockInvestStockPortfolioNotSelectId)
                .With(x => x.Type, BO.Models.FinancialAssistant.Enums.StockTypeEnum.InvestmentStock)
                .With(x => x.ActualizationTime, datetimeNow)
                .With(x => x.LastPrice, 50000m)
                .With(x => x.CurrencyId, stockCurrencyCheap.Id)
                .With(x => x.StockHistory, new List<StockHistory>()
                {
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(-9)).With(x => x.Price, 20000).With(x => x.StockId, stockInvestStockPortfolioNotSelectId).Create(),
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(3)).With(x => x.Price, 45000).With(x => x.StockId, stockInvestStockPortfolioNotSelectId).Create(),
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(30)).With(x => x.Price, 50000).With(x => x.StockId, stockInvestStockPortfolioNotSelectId).Create(),
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(50)).With(x => x.Price, 55000).With(x => x.StockId, stockInvestStockPortfolioNotSelectId).Create(),
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.End.AddDays(9)).With(x => x.Price, 60000).With(x => x.StockId, stockInvestStockPortfolioNotSelectId).Create(),
                }
                )
                //.With(x => x.Id, 3)
                .Create();  

            var elementInvestStock = _fixture.Build<StockElement>().With(x => x.StockId, stockInvestStock.Id)
                .With(x => x.Stock, stockInvestStock)
                .With(x => x.Count, 20)
                .With(x => x.PortfolioId, portfolio1.Id)
                .Create();

            stockElementRepository.Setup(x => x.GetWithStockNoTrack(statisticReq.PortfolioId))
                .Returns(Task.FromResult(new List<StockElement>() { elementCurrencyStrong, elementCurrencyStrong, elementInvestStock, elementCurrencyCheapPortfolio2 }));

            stockRepository.Setup(x => x.GetForUserWithHistoryAsync(userId))
                .Returns(Task.FromResult(new List<Stock>() { stockCurrencyCheap, stockCurrencyStrong, stockInvestStock, stockInvestStockPortfolioNotSelect }));


            int eventDay = -10;
            var cashReplenishment5 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio1.Id)
                .With(x => x.Type, StockEventEnum.CashReplenishment)
                .With(x => x.MainCountChange, 10000)
                .With(x => x.MainCountNow, 10000)
                .With(x => x.MainElementId, elementCurrencyCheap.Id)
                .With(x => x.SubCountChange, (decimal?)null)
                .With(x => x.SubCountNow, (decimal?)null)
                .With(x => x.SubCountOldValue, (decimal?)null)
                .Create();

            var buy4 = _fixture.Build<StockEvent>()
                 .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                 .With(x => x.PortfolioId, portfolio1.Id)
                 .With(x => x.Type, StockEventEnum.Buy)
                 .With(x => x.MainCountChange, 5)
                 .With(x => x.MainCountNow, 5)
                 .With(x => x.MainElementId, elementInvestStock.Id)
                 .With(x => x.SubCountChange, 10000)
                 .With(x => x.SubCountNow, 0)
                 .With(x => x.SubCountOldValue, 10000)
                 .With(x => x.SubElementId, elementCurrencyCheap.Id)
                 .Create();

             eventDay = 1;
            var cashReplenishment1 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio1.Id)
                .With(x => x.Type, StockEventEnum.CashReplenishment)
                .With(x => x.MainCountChange, 100000)
                .With(x => x.MainCountNow, 100000)
                .With(x => x.MainElementId, elementCurrencyCheap.Id)
                .With(x => x.SubCountChange, (decimal?)null)
                .With(x => x.SubCountNow, (decimal?)null)
                .With(x => x.SubCountOldValue, (decimal?)null)
                .Create();

            var cashReplenishment2 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio1.Id)
                .With(x => x.Type, StockEventEnum.CashReplenishment)
                .With(x => x.MainCountChange, 100000)
                .With(x => x.MainCountNow, 200000)
                .With(x => x.MainElementId, elementCurrencyCheap.Id)
                .With(x => x.SubCountChange, (decimal?)null)
                .With(x => x.SubCountNow, (decimal?)null)
                .With(x => x.SubCountOldValue, (decimal?)null)
                .Create();


            var cashReplenishment3 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio2.Id)
                .With(x => x.Type, StockEventEnum.CashReplenishment)
                .With(x => x.MainCountChange, 100000)
                .With(x => x.MainCountNow, 100000)
                .With(x => x.MainElementId, elementCurrencyCheap.Id)
                .With(x => x.SubCountChange, (decimal?)null)
                .With(x => x.SubCountNow, (decimal?)null)
                .With(x => x.SubCountOldValue, (decimal?)null)
                .Create();

            var buy1 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio1.Id)
                .With(x => x.Type, StockEventEnum.Buy)
                .With(x => x.MainCountChange, 5)
                .With(x => x.MainCountNow, 10)
                .With(x => x.MainElementId, elementInvestStock.Id)
                .With(x => x.SubCountChange, 22500)
                .With(x => x.SubCountNow, 177500)
                .With(x => x.SubCountOldValue, 200000)
                .With(x => x.SubElementId, elementCurrencyCheap.Id)
                .Create();

            eventDay = 70;
            var buy2 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio1.Id)
                .With(x => x.Type, StockEventEnum.Buy)
                .With(x => x.MainCountChange, 5)
                .With(x => x.MainCountNow, 15)
                .With(x => x.MainElementId, elementInvestStock.Id)
                .With(x => x.SubCountChange, 25000)
                .With(x => x.SubCountNow, 152500)
                .With(x => x.SubCountOldValue, 177500)
                .With(x => x.SubElementId, elementCurrencyCheap.Id)
                .Create();

            var dividends1 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio1.Id)
                .With(x => x.Type, StockEventEnum.Dividends)
                .With(x => x.MainCountChange, 0)
                .With(x => x.MainCountNow, 15)
                .With(x => x.MainElementId, elementInvestStock.Id)
                .With(x => x.SubCountChange, 10000)
                .With(x => x.SubCountNow, 162500)
                .With(x => x.SubCountOldValue, 152500)
                .With(x => x.SubElementId, elementCurrencyCheap.Id)
                .Create();


            eventDay = 100;
            var cashReplenishment4 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio1.Id)
                .With(x => x.Type, StockEventEnum.CashReplenishment)
                .With(x => x.MainCountChange, 100)
                .With(x => x.MainCountNow, 100)
                .With(x => x.MainElementId, elementCurrencyStrong.Id)
                .With(x => x.SubCountChange, (decimal?)null)
                .With(x => x.SubCountNow, (decimal?)null)
                .With(x => x.SubCountOldValue, (decimal?)null)
                .Create();

            var buy3 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio1.Id)
                .With(x => x.Type, StockEventEnum.Buy)
                .With(x => x.MainCountChange, 5)
                .With(x => x.MainCountNow, 20)
                .With(x => x.MainElementId, elementInvestStock.Id)
                .With(x => x.SubCountChange, 50)
                .With(x => x.SubCountNow, 50)
                .With(x => x.SubCountOldValue, 0)
                .With(x => x.SubElementId, elementCurrencyStrong.Id)
                .Create();

            //дивы доллорами?

            stockEventRepository.Setup(x => x.GetEvents(It.IsAny<List<long>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(new List<StockEvent>() { cashReplenishment1, cashReplenishment2, cashReplenishment3,
                    buy1, buy2, dividends1, cashReplenishment4, buy3, cashReplenishment5, buy4 }));



            stockEventRepository.Setup(x => x.GetLastActualEventsForMainElement(It.IsAny<List<long>>(),statisticReq.Start))
    .Returns(Task.FromResult(new List<StockEvent>() { cashReplenishment5, buy4}));

            stockEventRepository.Setup(x => x.GetLastActualEventsForSubElement(It.IsAny<List<long>>(), statisticReq.Start))
    .Returns(Task.FromResult(new List<StockEvent>() { buy4 }));

            stockEventRepository.Setup(x => x.GetLastActualEventsForMainElement(It.IsAny<List<long>>(), statisticReq.End))
    .Returns(Task.FromResult(new List<StockEvent>() { stockCurrencyCheap, stockCurrencyStrong, stockInvestStock, stockInvestStockPortfolioNotSelect }));
            stockEventRepository.Setup(x => x.GetLastActualEventsForSubElement(It.IsAny<List<long>>(), statisticReq.End))
    .Returns(Task.FromResult(new List<StockEvent>() { stockCurrencyCheap, stockCurrencyStrong, stockInvestStock, stockInvestStockPortfolioNotSelect }));





            var container = services.BuildServiceProvider();
            var portfolioService = container.GetRequiredService<PortfolioService>();
            var statistic = await portfolioService.GetStatisticAsync(statisticReq, userId);



            var projectId = 10;
            var status = _fixture.Build<WorkTaskStatus>()
                .With(x => x.Id, projectId)
                .With(x => x.Project, () => null)
                .With(x => x.Tasks, () => null)
                .Create();
            var statusRepo = AddMock<ITaskStatusRepository>(services);
            statusRepo
            .Setup(x => x.GetForProjectAsync(It.IsAny<long>()))
            .ReturnsAsync(new List<WorkTaskStatus> { status });
        }


        private ServiceCollection DefaultInit()
        {
            var services = new ServiceCollection();
            var appInitializer = new FinancialAssistantAppInitializer();
            appInitializer
                .RepositoriesInitialize(services, null)
                .ServicesInitialize(services, null)
                ;
            return services;
        }


        private Mock<T> AddMock<T>(IServiceCollection services) where T : class
        {
            var m = new Mock<T>();
            services.RemoveAll<T>();
            services.AddScoped<T>(_ => m.Object);
            return m;
        }
    }


}
