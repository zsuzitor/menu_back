using AutoFixture;
using BL.Models.Services.Interfaces;
using BO.Models.DAL.Domain;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using FinancialAssistantApp.Models;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using FinancialAssistantApp.Models.Services.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Text.Json;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
namespace FinancialAssistantApp.Tests.Services
{
    public class PortfolioServiceTests
    {

        private readonly IFixture _fixture;

        public PortfolioServiceTests()
        {
            _fixture = new Fixture();
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
            var datetimeNow = _fixture.Create<DateTime>();
            var userId = _fixture.Create<long>();
            var portfolio1 = _fixture.Build<Portfolio>().With(x => x.UserId, userId)
                .With(x => x.User, (User)null).With(x => x.Currency, (Stock)null).With(x => x.Elements, (List<StockElement>)null).With(x => x.Events, (List<StockEvent>)null)
                .Create();
            var portfolio2 = _fixture.Build<Portfolio>().With(x => x.UserId, userId)
                .With(x => x.User, (User)null).With(x => x.Currency, (Stock)null).With(x => x.Elements, (List<StockElement>)null).With(x => x.Events, (List<StockEvent>)null)
                .Create();
            var portfolio3 = _fixture.Build<Portfolio>().With(x => x.UserId, userId)
                .With(x => x.User, (User)null).With(x => x.Currency, (Stock)null).With(x => x.Elements, (List<StockElement>)null).With(x => x.Events, (List<StockEvent>)null)
                .Create();
            var statisticReq = new PortfolioStatisticRequestDto()
            {
                CurrencyId = 1,
                End = datetimeNow.AddYears(-1),
                Start = datetimeNow.AddYears(-2),
                PortfolioId = new List<long>() { portfolio1.Id, portfolio2.Id }
            };

            datetimeProvider.Setup(x => x.CurrentDateTime())
                .Returns(datetimeNow);

            portfolioRepo.Setup(x => x.GetAllAsync(statisticReq.PortfolioId, userId))
                .ReturnsAsync(new List<Portfolio>() { portfolio1, portfolio2 });

            long currencyStrongId = 2;

            var stockCurrencyCheap = _fixture.Build<Stock>().With(x => x.UserId, userId)
                .With(x => x.User, (User)null).With(x => x.Currency, (Stock)null)
                .With(x => x.ActualizationTime, datetimeNow)
                .With(x => x.Type, BO.Models.FinancialAssistant.Enums.StockTypeEnum.Currency)
                .With(x => x.ActualizationTime, datetimeNow)
                .With(x => x.LastPrice, 0.00667m)
                .With(x => x.CurrencyId, currencyStrongId)
                .With(x => x.Id, 1)
                .With(x => x.StockHistory, new List<StockHistory>()
                {
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, currencyStrongId)
                        .With(x => x.Date, statisticReq.Start.AddDays(-9)).With(x => x.Price, 0.02m).With(x => x.StockId, 1)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),//price 50
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, currencyStrongId)
                        .With(x => x.Date, statisticReq.Start.AddDays(3)).With(x => x.Price, 0.0125m).With(x => x.StockId, 1)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),//price 80
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, currencyStrongId)
                        .With(x => x.Date, statisticReq.Start.AddDays(30)).With(x => x.Price, 0.01m).With(x => x.StockId, 1)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),//price 100
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, currencyStrongId)
                        .With(x => x.Date, statisticReq.Start.AddDays(50)).With(x => x.Price, 0.008m).With(x => x.StockId, 1)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),//125
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, currencyStrongId)
                        .With(x => x.Date, statisticReq.Start.AddDays(90)).With(x => x.Price, 0.008m).With(x => x.StockId, 1)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),//125
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, currencyStrongId)
                        .With(x => x.Date, statisticReq.End.AddDays(9)).With(x => x.Price, 0.00667m).With(x => x.StockId, 1)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),//price 50
                }
                )
                .Create();


            var elementCurrencyCheap = _fixture.Build<StockElement>().With(x => x.StockId, stockCurrencyCheap.Id)
                .With(x => x.Stock, stockCurrencyCheap)
                .With(x => x.Count, 162500)
                .With(x => x.PortfolioId, portfolio1.Id).With(x => x.Portfolio, (Portfolio)null)
                .Create();

            var elementCurrencyCheapPortfolio2 = _fixture.Build<StockElement>().With(x => x.StockId, stockCurrencyCheap.Id)
                .With(x => x.Stock, stockCurrencyCheap)
                .With(x => x.Count, 100000)
                .With(x => x.PortfolioId, portfolio2.Id).With(x => x.Portfolio, (Portfolio)null)
                .Create();

            var stockCurrencyStrong = _fixture.Build<Stock>().With(x => x.UserId, userId)
                .With(x => x.User, (User)null).With(x => x.Currency, (Stock)null)
                .With(x => x.Type, BO.Models.FinancialAssistant.Enums.StockTypeEnum.Currency)
                .With(x => x.ActualizationTime, datetimeNow)
                .With(x => x.LastPrice, 150m)
                .With(x => x.CurrencyId, stockCurrencyCheap.Id)
                .With(x => x.Id, currencyStrongId)
                .With(x => x.StockHistory, new List<StockHistory>()
                {
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(-9)).With(x => x.Price, 50).With(x => x.StockId, currencyStrongId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),//price 50
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(3)).With(x => x.Price, 8).With(x => x.StockId, currencyStrongId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),//price 80
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(30)).With(x => x.Price, 100).With(x => x.StockId, currencyStrongId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),//price 100
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(50)).With(x => x.Price, 125).With(x => x.StockId, currencyStrongId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),//125
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.End.AddDays(9)).With(x => x.Price, 150).With(x => x.StockId, currencyStrongId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),//price 50
                }
                )
                .Create();

            var elementCurrencyStrong = _fixture.Build<StockElement>().With(x => x.StockId, stockCurrencyStrong.Id)
                .With(x => x.Stock, stockCurrencyStrong)
                .With(x => x.Count, 50)
                .With(x => x.PortfolioId, portfolio2.Id).With(x => x.Portfolio, (Portfolio)null)
                .Create();


            var stockInvestStockId = _fixture.Create<long>();
            var stockInvestStock = _fixture.Build<Stock>().With(x => x.UserId, userId)
                .With(x => x.User, (User)null).With(x => x.Currency, (Stock)null)
                .With(x => x.Id, stockInvestStockId)
                .With(x => x.Type, BO.Models.FinancialAssistant.Enums.StockTypeEnum.InvestmentStock)
                .With(x => x.ActualizationTime, datetimeNow)
                .With(x => x.LastPrice, 6000m)
                .With(x => x.CurrencyId, stockCurrencyCheap.Id)
                .With(x => x.StockHistory, new List<StockHistory>()
                {
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(-9)).With(x => x.Price, 2000).With(x => x.StockId, stockInvestStockId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),

                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(-1)).With(x => x.Price, 2500).With(x => x.StockId, stockInvestStockId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),

                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(3)).With(x => x.Price, 4500).With(x => x.StockId, stockInvestStockId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),

                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(30)).With(x => x.Price, 5000).With(x => x.StockId, stockInvestStockId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),

                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(50)).With(x => x.Price, 5500).With(x => x.StockId, stockInvestStockId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),

                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(90)).With(x => x.Price, 5500).With(x => x.StockId, stockInvestStockId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),

                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.End.AddDays(9)).With(x => x.Price, 6000).With(x => x.StockId, stockInvestStockId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),
                }
                )
                //.With(x => x.Id, 3)
                .Create();

            var stockInvestStockPortfolioNotSelectId = _fixture.Create<long>();
            var stockInvestStockPortfolioNotSelect = _fixture.Build<Stock>().With(x => x.UserId, userId)
                .With(x => x.User, (User)null).With(x => x.Currency, (Stock)null)
                .With(x => x.Id, stockInvestStockPortfolioNotSelectId)
                .With(x => x.Type, BO.Models.FinancialAssistant.Enums.StockTypeEnum.InvestmentStock)
                .With(x => x.ActualizationTime, datetimeNow)
                .With(x => x.LastPrice, 50000m)
                .With(x => x.CurrencyId, stockCurrencyCheap.Id)
                .With(x => x.StockHistory, new List<StockHistory>()
                {
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(-9)).With(x => x.Price, 20000).With(x => x.StockId, stockInvestStockPortfolioNotSelectId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(3)).With(x => x.Price, 45000).With(x => x.StockId, stockInvestStockPortfolioNotSelectId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(30)).With(x => x.Price, 50000).With(x => x.StockId, stockInvestStockPortfolioNotSelectId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.Start.AddDays(50)).With(x => x.Price, 55000).With(x => x.StockId, stockInvestStockPortfolioNotSelectId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),
                    _fixture.Build<StockHistory>().With(x => x.CurrencyId, stockCurrencyCheap.Id)
                        .With(x => x.Date, statisticReq.End.AddDays(9)).With(x => x.Price, 60000).With(x => x.StockId, stockInvestStockPortfolioNotSelectId)
                        .With(x => x.Currency, (Stock)null).With(x => x.Stock, (Stock)null).Create(),
                }
                )
                //.With(x => x.Id, 3)
                .Create();

            var elementInvestStock = _fixture.Build<StockElement>().With(x => x.StockId, stockInvestStock.Id)
                .With(x => x.Stock, stockInvestStock)
                .With(x => x.Count, 20)
                .With(x => x.PortfolioId, portfolio1.Id).With(x => x.Portfolio, (Portfolio)null)
                .Create();

            stockElementRepository.Setup(x => x.GetWithStockNoTrack(statisticReq.PortfolioId))
                .Returns(Task.FromResult(new List<StockElement>() { elementCurrencyCheap, elementCurrencyStrong, elementInvestStock, elementCurrencyCheapPortfolio2 }));

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
                        .With(x => x.MainElement, (StockElement)null).With(x => x.SubElement, (StockElement)null).With(x => x.Portfolio, (Portfolio)null)
                .Create();

            var buy4 = _fixture.Build<StockEvent>()
                 .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                 .With(x => x.PortfolioId, portfolio1.Id)
                 .With(x => x.Type, StockEventEnum.Buy)
                 .With(x => x.MainCountChange, 5)
                 .With(x => x.MainCountNow, 5)
                 .With(x => x.MainElementId, elementInvestStock.Id)
                 .With(x => x.SubCountChange, -10000)
                 .With(x => x.SubCountNow, 0)
                 .With(x => x.SubCountOldValue, 10000)
                 .With(x => x.SubElementId, elementCurrencyCheap.Id)
                        .With(x => x.MainElement, (StockElement)null).With(x => x.SubElement, (StockElement)null).With(x => x.Portfolio, (Portfolio)null)
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
                        .With(x => x.MainElement, (StockElement)null).With(x => x.SubElement, (StockElement)null).With(x => x.Portfolio, (Portfolio)null)
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
                        .With(x => x.MainElement, (StockElement)null).With(x => x.SubElement, (StockElement)null).With(x => x.Portfolio, (Portfolio)null)
                .Create();


            var cashReplenishment3 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio2.Id)
                .With(x => x.Type, StockEventEnum.CashReplenishment)
                .With(x => x.MainCountChange, 100000)
                .With(x => x.MainCountNow, 100000)
                .With(x => x.MainElementId, elementCurrencyCheapPortfolio2.Id)
                .With(x => x.SubCountChange, (decimal?)null)
                .With(x => x.SubCountNow, (decimal?)null)
                .With(x => x.SubCountOldValue, (decimal?)null)
                        .With(x => x.MainElement, (StockElement)null).With(x => x.SubElement, (StockElement)null).With(x => x.Portfolio, (Portfolio)null)
                .Create();

            var buy1 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio1.Id)
                .With(x => x.Type, StockEventEnum.Buy)
                .With(x => x.MainCountChange, 5)
                .With(x => x.MainCountNow, 10)
                .With(x => x.MainElementId, elementInvestStock.Id)
                .With(x => x.SubCountChange, -22500)
                .With(x => x.SubCountNow, 177500)
                .With(x => x.SubCountOldValue, 200000)
                .With(x => x.SubElementId, elementCurrencyCheap.Id)
                        .With(x => x.MainElement, (StockElement)null).With(x => x.SubElement, (StockElement)null).With(x => x.Portfolio, (Portfolio)null)
                .Create();

            eventDay = 70;
            var buy2 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio1.Id)
                .With(x => x.Type, StockEventEnum.Buy)
                .With(x => x.MainCountChange, 5)
                .With(x => x.MainCountNow, 15)
                .With(x => x.MainElementId, elementInvestStock.Id)
                .With(x => x.SubCountChange, -25000)
                .With(x => x.SubCountNow, 152500)
                .With(x => x.SubCountOldValue, 177500)
                .With(x => x.SubElementId, elementCurrencyCheap.Id)
                        .With(x => x.MainElement, (StockElement)null).With(x => x.SubElement, (StockElement)null).With(x => x.Portfolio, (Portfolio)null)
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
                        .With(x => x.MainElement, (StockElement)null).With(x => x.SubElement, (StockElement)null).With(x => x.Portfolio, (Portfolio)null)
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
                .With(x => x.SubElementId, (decimal?)null)
                        .With(x => x.MainElement, (StockElement)null).With(x => x.SubElement, (StockElement)null).With(x => x.Portfolio, (Portfolio)null)
                .Create();

            var buy3 = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, statisticReq.Start.AddDays(eventDay++))
                .With(x => x.PortfolioId, portfolio1.Id)
                .With(x => x.Type, StockEventEnum.Buy)
                .With(x => x.MainCountChange, 5)
                .With(x => x.MainCountNow, 20)
                .With(x => x.MainElementId, elementInvestStock.Id)
                .With(x => x.SubCountChange, -50)
                .With(x => x.SubCountNow, 50)
                .With(x => x.SubCountOldValue, 100)
                .With(x => x.SubElementId, elementCurrencyStrong.Id)
                .With(x => x.MainElement, (StockElement)null).With(x => x.SubElement, (StockElement)null).With(x => x.Portfolio, (Portfolio)null)
                .Create();

            //дивы доллорами?

            stockEventRepository.Setup(x => x.GetEvents(It.IsAny<List<long>>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Task.FromResult(new List<StockEvent>() { cashReplenishment1, cashReplenishment2, cashReplenishment3,
                    buy1, buy2, dividends1, cashReplenishment4, buy3 }));



            stockEventRepository.Setup(x => x.GetLastActualEventsForMainElement(It.IsAny<List<long>>(), statisticReq.Start))
                .Returns(Task.FromResult(new List<StockEvent>() { cashReplenishment5, buy4 }));

            stockEventRepository.Setup(x => x.GetLastActualEventsForSubElement(It.IsAny<List<long>>(), statisticReq.Start))
                .Returns(Task.FromResult(new List<StockEvent>() { buy4 }));

            stockEventRepository.Setup(x => x.GetLastActualEventsForMainElement(It.IsAny<List<long>>(), statisticReq.End))
                .Returns(Task.FromResult(new List<StockEvent>() { cashReplenishment3, cashReplenishment4, buy3 }));
            stockEventRepository.Setup(x => x.GetLastActualEventsForSubElement(It.IsAny<List<long>>(), statisticReq.End))
                .Returns(Task.FromResult(new List<StockEvent>() { buy3, dividends1 }));





            var container = services.BuildServiceProvider();
            var portfolioService = container.GetRequiredService<IPortfolioService>();
            var result = await portfolioService.GetStatisticAsync(statisticReq, userId);
            var json = JsonSerializer.Serialize(result);

            var ReplenishmentsByCurrencyGood = new Dictionary<long, PortfolioStatistic.Currency>()
            {
                { stockCurrencyCheap.Id, new PortfolioStatistic.Currency(){CurrencyId=stockCurrencyCheap.Id,CurrencySum=300000M,CurrencyName=stockCurrencyCheap.Name } },
                { stockCurrencyStrong.Id, new PortfolioStatistic.Currency(){CurrencyId=stockCurrencyStrong.Id,CurrencySum=100,CurrencyName=stockCurrencyStrong.Name } }
            };

            result.Should().NotBeNull();
            result.CashReplenishmentSum.Should().Be(312500M);
            result.DividendsCashSum.Should().Be(10000);
            Math.Round(result.SumNow,0).Should().Be(389996M);//result.SumNow.Should().Be(390000);

            Math.Round(result.SumOnStartPeriod, 0).Should().Be(12500);
            Math.Round(result.SumOnEndPeriod, 0).Should().Be(389996M);

            result.WithdrawalCashSum.Should().Be(0);
            result.ReplenishmentsByCurrency.Count.Should().Be(2);
            result.ReplenishmentsByCurrency.Should().BeEquivalentTo(ReplenishmentsByCurrencyGood);
            result.WithdrawalCashByCurrency.Should().BeEquivalentTo(new Dictionary<long, PortfolioStatistic.Currency>());
            result.DividendsCashByCurrency.Should().BeEquivalentTo(new Dictionary<long, PortfolioStatistic.Currency>() { { stockCurrencyCheap.Id, new PortfolioStatistic.Currency() { CurrencyId = stockCurrencyCheap.Id, CurrencySum = 10000, CurrencyName = stockCurrencyCheap.Name } } });
            var monthIteration = 0;
            result.PeriodSums.Should().BeEquivalentTo(new List<PeriodSum>() {
                new PeriodSum() {Date=statisticReq.Start.AddMonths(monthIteration++).Date,Sum= result.SumOnStartPeriod },
                new PeriodSum() {Date=statisticReq.Start.AddMonths(monthIteration++).Date,Sum= 327500 },
                new PeriodSum() {Date=statisticReq.Start.AddMonths(monthIteration++).Date,Sum= 332500M },
                new PeriodSum() {Date=statisticReq.Start.AddMonths(monthIteration++).Date,Sum= 345000 },
                new PeriodSum() {Date=statisticReq.Start.AddMonths(monthIteration++).Date,Sum= 378750M },
                new PeriodSum() {Date=statisticReq.Start.AddMonths(monthIteration++).Date,Sum= 378750M },
                new PeriodSum() {Date=statisticReq.Start.AddMonths(monthIteration++).Date,Sum= 378750M },
                new PeriodSum() {Date=statisticReq.Start.AddMonths(monthIteration++).Date,Sum= 378750M },
                new PeriodSum() {Date=statisticReq.Start.AddMonths(monthIteration++).Date,Sum= result.SumOnEndPeriod },
                new PeriodSum() {Date=statisticReq.Start.AddMonths(monthIteration++).Date,Sum= result.SumOnEndPeriod },
                new PeriodSum() {Date=statisticReq.Start.AddMonths(monthIteration++).Date,Sum= result.SumOnEndPeriod },
                new PeriodSum() {Date=statisticReq.End.Date,Sum= result.SumOnEndPeriod },
            }, options => options
                .Using<decimal>(ctx => ctx.Subject.Should().BeApproximately(ctx.Expectation, 1M))
                .WhenTypeIs<decimal>());


            //result.Should().HaveCount(1);
            //result.First().Id.Should().Be(projectId);



            //{ "CashReplenishmentSum":322500,"ReplenishmentsByCurrency":{ "1":310000,"2":100},"WithdrawalCashSum":0,"WithdrawalCashByCurrency":{ },"DividendsCashSum":10000,"DividendsCashByCurrency":{ "1":10000},
            //"SumNow":276600,"SumOnStartPeriod":22500,"SumOnEndPeriod":285000,"PeriodSums":[{ "Date":"2024-06-09T04:06:11.4288438","Sum":22500},{ "Date":"2024-07-09T04:06:11.4288438","Sum":227500},{ "Date":"2024-08-09T04:06:11.4288438","Sum":232500},{ "Date":"2024-09-09T04:06:11.4288438","Sum":245000},{ "Date":"2024-10-09T04:06:11.4288438","Sum":278750},{ "Date":"2024-11-09T04:06:11.4288438","Sum":278750},{ "Date":"2024-12-09T04:06:11.4288438","Sum":278750},{ "Date":"2025-01-09T04:06:11.4288438","Sum":285000},{ "Date":"2025-02-09T04:06:11.4288438","Sum":285000},{ "Date":"2025-03-09T04:06:11.4288438","Sum":285000},{ "Date":"2025-04-09T04:06:11.4288438","Sum":285000},{ "Date":"2025-06-09T04:06:11.4288438","Sum":285000}]}

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
