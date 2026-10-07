using AutoFixture;
using BL.Models.Services.Interfaces;
using BO.Models.DAL.Domain;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using FinancialAssistantApp.Models;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using FinancialAssistantApp.Models.Handlers.CreateEventHandlers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using System.Text.Json;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Tests.Handlers.CreateEventHandlers
{
    public class CreateEventDividendsTests
    {

        private readonly IFixture _fixture;

        public CreateEventDividendsTests()
        {
            _fixture = new Fixture();
        }



        [Fact]
        public async Task CreateEventDividendsAsync_NonExistsElements_Success()
        {
            var userId = _fixture.Create<long>();
            var services = DefaultInit();

            var stockRepository = AddMock<IStockRepository>(services);
            var dateTimeProvider = AddMock<IDateTimeProvider>(services);
            var portfolioRepository = AddMock<IPortfolioRepository>(services);
            var stockElementRepository = AddMock<IStockElementRepository>(services);
            var stockEventRepository = AddMock<IStockEventRepository>(services);

            var datetimeNow = _fixture.Create<DateTime>();
            dateTimeProvider.Setup(x => x.CurrentDateTime())
                .Returns(datetimeNow);



            var createObj = _fixture.Build<StockEventCreate>()
                .With(x => x.Type, StockEventEnum.Dividends)
                .With(x => x.OutdateForce, true)
                .With(x => x.CurrencyActions, true)
                .Create();



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
                { })
                .Create();

            var stockInvestStockId = _fixture.Create<long>();
            var stockInvestStock = _fixture.Build<Stock>().With(x => x.UserId, userId)
                .With(x => x.User, (User)null).With(x => x.Currency, (Stock)null)
                .With(x => x.Id, stockInvestStockId)
                .With(x => x.Type, BO.Models.FinancialAssistant.Enums.StockTypeEnum.InvestmentStock)
                .With(x => x.ActualizationTime, datetimeNow.AddDays(-1))
                .With(x => x.LastPrice, 6000m)
                .With(x => x.CurrencyId, stockCurrencyCheap.Id)
                .With(x => x.StockHistory, new List<StockHistory>()
                { })
                    .Create();


            stockRepository.Setup(x => x.GetNoTrackAsync(createObj.StockId.Value))
                .ReturnsAsync(stockInvestStock);
            stockRepository.Setup(x => x.GetCurrencyWithValidate(createObj.CurrencyId.Value, userId))
                .ReturnsAsync(stockCurrencyCheap);


            //stockElementRepository.Setup(x => x.AddAsync(createObj.StockId.Value))
            //    .ReturnsAsync(stockInvestStock);

            stockElementRepository.Setup(x => x.Get(It.IsAny<long>(), createObj.StockId.Value))
                .ReturnsAsync((StockElement)null);
            stockElementRepository.Setup(x => x.Get(It.IsAny<long>(), createObj.CurrencyId.Value))
                .ReturnsAsync((StockElement)null);

            portfolioRepository.Setup(x => x.ExistAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(true);

            stockEventRepository.Setup(x => x.AddAsync(It.IsAny<StockEvent>()))
                .ReturnsAsync((StockEvent e) => e);


            var container = services.BuildServiceProvider();
            var factory = container.GetRequiredService<CreateEventFactory>();
            var DividendsHandler = factory.Get(StockEventEnum.Dividends, userId);


            var ev = await DividendsHandler.CreateEvent(createObj);

            ev.EventDateTime.Should().Be(createObj.Date);
            ev.CreationDateTime.Should().Be(datetimeNow);
            ev.MainCountChange.Should().Be(0);
            ev.MainCountNow.Should().Be(0);
            ev.SubCountChange.Should().Be(createObj.Price);
            ev.SubCountNow.Should().Be(createObj.Price);
            ev.SubCountOldValue.Should().Be(0);
            ev.PortfolioId.Should().Be(createObj.PortfolioId);
        }


        [Fact]
        public async Task CreateEventDividendsAsync_ExistsElements_Success()
        {
            var userId = _fixture.Create<long>();
            var services = DefaultInit();

            var stockRepository = AddMock<IStockRepository>(services);
            var dateTimeProvider = AddMock<IDateTimeProvider>(services);
            var portfolioRepository = AddMock<IPortfolioRepository>(services);
            var stockElementRepository = AddMock<IStockElementRepository>(services);
            var stockEventRepository = AddMock<IStockEventRepository>(services);

            var datetimeNow = _fixture.Create<DateTime>();
            dateTimeProvider.Setup(x => x.CurrentDateTime())
                .Returns(datetimeNow);



            var createObj = _fixture.Build<StockEventCreate>()
                .With(x => x.Type, StockEventEnum.Dividends)
                .With(x => x.OutdateForce, true)
                .With(x => x.CurrencyActions, true)
                .Create();



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
                { })
                .Create();

            var stockInvestStockId = _fixture.Create<long>();
            var stockInvestStock = _fixture.Build<Stock>().With(x => x.UserId, userId)
                .With(x => x.User, (User)null).With(x => x.Currency, (Stock)null)
                .With(x => x.Id, stockInvestStockId)
                .With(x => x.Type, BO.Models.FinancialAssistant.Enums.StockTypeEnum.InvestmentStock)
                .With(x => x.ActualizationTime, datetimeNow.AddDays(-1))
                .With(x => x.LastPrice, 6000m)
                .With(x => x.CurrencyId, stockCurrencyCheap.Id)
                .With(x => x.StockHistory, new List<StockHistory>()
                { })
                    .Create();


            stockRepository.Setup(x => x.GetNoTrackAsync(createObj.StockId.Value))
                .ReturnsAsync(stockInvestStock);
            stockRepository.Setup(x => x.GetCurrencyWithValidate(createObj.CurrencyId.Value, userId))
                .ReturnsAsync(stockCurrencyCheap);


            //stockElementRepository.Setup(x => x.AddAsync(createObj.StockId.Value))
            //    .ReturnsAsync(stockInvestStock);


            var elementCurrencyCheap = _fixture.Build<StockElement>().With(x => x.StockId, stockCurrencyCheap.Id)
                .With(x => x.Stock, stockCurrencyCheap)
                .With(x => x.PortfolioId, createObj.PortfolioId).With(x => x.Portfolio, (Portfolio)null)
                .Create();
            var elementCurrencyCheapCopy = JsonSerializer.Deserialize<StockElement>(JsonSerializer.Serialize(elementCurrencyCheap))!;

            var elementStock = _fixture.Build<StockElement>().With(x => x.StockId, stockInvestStock.Id)
                .With(x => x.Stock, stockInvestStock)
                .With(x => x.PortfolioId, createObj.PortfolioId).With(x => x.Portfolio, (Portfolio)null)
                .Create();

            var elementStockCopy = JsonSerializer.Deserialize<StockElement>(JsonSerializer.Serialize(elementStock))!;

            stockElementRepository.Setup(x => x.Get(It.IsAny<long>(), createObj.StockId.Value))
                .ReturnsAsync((StockElement)elementStock);
            stockElementRepository.Setup(x => x.Get(It.IsAny<long>(), createObj.CurrencyId.Value))
                .ReturnsAsync((StockElement)elementCurrencyCheap);

            portfolioRepository.Setup(x => x.ExistAsync(It.IsAny<long>(), It.IsAny<long>()))
                .ReturnsAsync(true);

            stockEventRepository.Setup(x => x.AddAsync(It.IsAny<StockEvent>()))
                .ReturnsAsync((StockEvent e) => e);


            var container = services.BuildServiceProvider();
            var factory = container.GetRequiredService<CreateEventFactory>();
            var DividendsHandler = factory.Get(StockEventEnum.Dividends, userId);


            var ev = await DividendsHandler.CreateEvent(createObj);

            ev.EventDateTime.Should().Be(createObj.Date);
            ev.CreationDateTime.Should().Be(datetimeNow);
            ev.MainCountChange.Should().Be(0);
            ev.MainCountNow.Should().Be(elementStockCopy.Count);
            ev.SubCountChange.Should().Be(createObj.Price);
            ev.SubCountNow.Should().Be(elementCurrencyCheapCopy.Count + createObj.Price);
            ev.SubCountOldValue.Should().Be(elementCurrencyCheapCopy.Count);
            ev.PortfolioId.Should().Be(createObj.PortfolioId);
        }


        [Fact]
        public async Task UndoEventDividendsAsync_Success()
        {

            var userId = _fixture.Create<long>();
            var services = DefaultInit();

            var stockRepository = AddMock<IStockRepository>(services);
            var dateTimeProvider = AddMock<IDateTimeProvider>(services);
            var portfolioRepository = AddMock<IPortfolioRepository>(services);
            var stockElementRepository = AddMock<IStockElementRepository>(services);
            var stockEventRepository = AddMock<IStockEventRepository>(services);

            var datetimeNow = _fixture.Create<DateTime>();
            dateTimeProvider.Setup(x => x.CurrentDateTime())
                .Returns(datetimeNow);



            var ev = _fixture.Build<StockEvent>()
                .With(x => x.EventDateTime, datetimeNow.AddDays(-5))
                .With(x => x.Type, StockEventEnum.Dividends)
                .With(x => x.MainElementId, 1)
                        .With(x => x.MainElement, (StockElement)null).With(x => x.SubElement, (StockElement)null).With(x => x.Portfolio, (Portfolio)null)
                .Create();

            if (ev.MainCountChange < 0)
            {
                ev.MainCountChange *= -1;
            }
            if (ev.SubCountChange > 0)
            {
                ev.SubCountChange *= -1;
            }

            var container = services.BuildServiceProvider();
            var factory = container.GetRequiredService<CreateEventFactory>();
            var DividendsHandler = factory.Get(StockEventEnum.Dividends, userId);


            var rollBack = DividendsHandler.GetRollBackCountChange(ev);

            rollBack.Count.Should().Be(2);
            rollBack[0].Type.Should().Be(StockEventEnum.CountChange);
            rollBack[0].MainCountChange.Should().Be(0);
            rollBack[0].MainCountNow.Should().Be(ev.MainCountNow);
            rollBack[0].MainElementId.Should().Be(ev.MainElementId);
            rollBack[0].PortfolioId.Should().Be(ev.PortfolioId);
            rollBack[0].CreationDateTime.Should().Be(datetimeNow);
            rollBack[0].EventDateTime.Should().Be(datetimeNow);

            rollBack[1].Type.Should().Be(StockEventEnum.CountChange);
            rollBack[1].MainCountChange.Should().Be(ev.SubCountChange * -1);
            rollBack[1].MainCountNow.Should().Be(ev.SubCountOldValue);
            rollBack[1].MainElementId.Should().Be(ev.SubElementId);
            rollBack[1].PortfolioId.Should().Be(ev.PortfolioId);
            rollBack[1].CreationDateTime.Should().Be(datetimeNow);
            rollBack[1].EventDateTime.Should().Be(datetimeNow);

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