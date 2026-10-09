using BL.Models.Services.Interfaces;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using Common.Models.Exceptions;
using DAL.Models.DAL;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using FinancialAssistantApp.Models.Handlers.CreateEventHandlers;
using FinancialAssistantApp.Models.Services.Interfaces;
using Menu.Models.Services.Interfaces;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.Services
{
    public class StockEventService : IStockEventService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IDateTimeProvider _datetimProvider;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IStockElementRepository _stockElementRepository;
        private readonly IStockEventRepository _stockEventRepository;
        private readonly CreateEventFactory _createEventFactory;
        private readonly IDBHelper _dbHelper;
        private readonly MenuDbContext _db;
        private readonly IUserService _userService;

        public StockEventService(IStockRepository stockRepository, IDateTimeProvider datetimProvider, IPortfolioRepository portfolioRepository, IStockElementRepository stockElementRepository, IStockEventRepository stockEventRepository, CreateEventFactory createEventFactory, IDBHelper dbHelper, MenuDbContext db, IUserService userService)
        {
            _stockRepository = stockRepository;
            _datetimProvider = datetimProvider;
            _portfolioRepository = portfolioRepository;
            _stockElementRepository = stockElementRepository;
            _stockEventRepository = stockEventRepository;
            _createEventFactory = createEventFactory;
            _dbHelper = dbHelper;
            _db = db;
            _userService = userService;
        }


        public async Task<StockEvent> CreateEventAsync(StockEventCreate obj, long userId)
        {
            //todo транзакция

            if (!Enum.IsDefined(typeof(StockEventEnum), obj.Type))
            {
                throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
            }
            var eventHandler = _createEventFactory.Get(obj.Type, userId);
            return await eventHandler.CreateEvent(obj);

        }

        public async Task<StockEvent> DeleteEventAsync(long id, bool force, long userId)
        {
            //todo другая ошибка
            var ev = await _stockEventRepository.GetAsync(id) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);
            if(!(await _portfolioRepository.ExistAsync(ev.PortfolioId, userId)))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }

            if (!force)
            {
                var mainEvent = await _stockEventRepository.GetLastActualEvent(ev.MainElementId);
                if (mainEvent!=null && mainEvent.Id != id)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.OutdateStockEvent);
                }

                if (ev.SubElementId != null)
                {
                    var subEvent = await _stockEventRepository.GetLastActualEvent(ev.SubElementId.Value);
                    if (subEvent != null && subEvent.Id != id)
                    {
                        throw new SomeCustomNotFoundException(Consts.ErrorConsts.OutdateStockEvent);
                    }
                }
            }

            return await _stockEventRepository.DeleteAsync(ev);

        }

        public async Task<(List<StockEvent>, long)> GetForPortfolioAsync(GetPortfolioEvents req)
        {
            if (!await _portfolioRepository.ExistAsync(req.PortfolioId, req.UserId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }

            var elems =  await _stockEventRepository.GetForPortfolioAsync(req.PortfolioId, req.PageSize, req.Page, req.Type);
            var count = await _stockEventRepository.GetCountForPortfolioAsync(req.PortfolioId, req.Type);
            return (elems, count);
        }

        public async Task<(List<StockEvent>, long)> GetForStockAsync(long portfolioId, long stockId, long userId, int pageSize, int pageNum)
        {
            if (!await _portfolioRepository.ExistAsync(portfolioId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }

            var events =  await _stockEventRepository.GetForStockAsync(portfolioId, stockId,pageSize,pageNum);
            var count =  await _stockEventRepository.GetForStockCountAsync(portfolioId, stockId);
            return (events, count);

        }

        public async Task PortfolioRecalculate(long portfolioId, long userId)
        {
            var portfolio = await _portfolioRepository.GetNoTrackAsync(portfolioId) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            if (portfolio.UserId != userId)
            {
                var admin = await _userService.IsAdminAsync(userId);
                if (!admin)
                {
                      throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
                }
            }
            

            var dictElement = new Dictionary<long, decimal>();

            await _dbHelper.ActionInTransaction(_db, async () =>
            {
                var events = await _stockEventRepository.GetForPortfolioAsync(portfolioId);

                foreach (var elem in events)
                {
                    CreateEventBase.RecalculateEvent(elem, dictElement);

                }

                var elements = await _stockElementRepository.GetForPortfolio(portfolioId);
                foreach (var item in elements)
                {
                    if (dictElement.TryGetValue(item.Id, out var newElemCount))
                    {
                        item.Count = newElemCount;
                    }
                }

                await _stockElementRepository.UpdateAsync(elements);
                await _stockEventRepository.UpdateAsync(events);
            });
        }
    }
}
