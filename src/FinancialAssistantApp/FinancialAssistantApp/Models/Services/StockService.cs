using BL.Models.Services.Interfaces;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using Common.Models.Exceptions;
using DAL.Models.DAL;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using FinancialAssistantApp.Models.Mapper;
using FinancialAssistantApp.Models.Services.Interfaces;
using Menu.Models.Services.Interfaces;
using System.Reflection.Metadata;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using TIntegration.Models.DTO;
using TIntegration.Models.Services.Interfaces;

namespace FinancialAssistantApp.Models.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockHistoryRepository _stockHistoryRepository;
        private readonly IDateTimeProvider _datetimeProvider;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IUserService _userService;
        private readonly IPriceService _priceService;
        private readonly IDBHelper _dbHelper;
        private readonly MenuDbContext _db;

        public StockService(IStockRepository stockRepository, IDateTimeProvider datetimeProvider, IPortfolioRepository portfolioRepository, IStockHistoryRepository stockHistoryRepository, IUserService userService, IPriceService priceService, MenuDbContext db, IDBHelper dbHelper)
        {
            _stockRepository = stockRepository;
            _datetimeProvider = datetimeProvider;
            _portfolioRepository = portfolioRepository;
            _stockHistoryRepository = stockHistoryRepository;
            _userService = userService;
            _priceService = priceService;
            _db = db;
            _dbHelper = dbHelper;
        }

        public async Task<Stock> CreateAsync(CreateStock obj, long userId)
        {
            if (!Enum.IsDefined(typeof(StockTypeEnum), obj.Type))
            {
                throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
            }

            var rec = new Stock
            {
                Name = obj.Name,
                Code = obj.Code,
                //ActualizationTime = _datetimeProvider.CurrentDateTime(),
                //LastPrice = obj.LastPrice,
                Type = obj.Type,
                IsGlobal = obj.IsGlobal
            };

            if (rec.IsGlobal)
            {
                var admin = await _userService.IsAdminAsync(userId);
                if (!admin)
                {
                    throw new SomeCustomNotAllowedException();
                }
            }
            else
            {
                rec.UserId = userId;
           
            }

           

            var result = await _stockRepository.AddAsync(rec);
            //var history = GetHistory(result);
            //await _stockHistoryRepository.AddAsync(history);
            return result;

        }


        public async Task<StockHistory> DeleteHistoryAsync(long id, long userId)
        {
            StockHistory result = null;
            await _dbHelper.ActionInTransaction(_db, async () =>
            {
                var history = await _stockHistoryRepository.GetAsync(id);
                var stock = await _stockRepository.GetAsync(history.StockId, userId) ?? throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);

                result = await _stockHistoryRepository.DeleteAsync(history);
                var lastHistory = await _stockHistoryRepository.GetLastHistoryAsync(history.StockId);
                //если только что удалили последнюю историю то все должно упасть и откатить транзакцию
                if (lastHistory == null)
                {
                    throw new SomeCustomException("Нельзя удалить последнюю историю");
                }
                stock.LastPrice = lastHistory.Price;
                stock.ActualizationTime = lastHistory.Date;
                stock.CurrencyId = lastHistory.CurrencyId;
                await _stockRepository.UpdateAsync(stock);
               
            });

            return result;
        }

        public async Task<StockHistory> CreateHistoryAsync(StockHistory req, long userId)
        {
            if (req.CurrencyId == null || req.CurrencyId <= 0 || req.Price <= 0)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);
            }

            return await _dbHelper.ActionInTransaction(_db, async () =>
            {
                var stock = await _stockRepository.GetAsync(req.StockId) ?? throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);

                if (stock.IsGlobal)
                {
                    var admin = await _userService.IsAdminAsync(userId);
                    if (!admin)
                    {
                        throw new SomeCustomNotAllowedException();
                    }
                }
                else if (stock.UserId != userId)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);

                }


                var currency = await _stockRepository.GetCurrencyWithValidate(req.CurrencyId, userId);
                var history = new StockHistory()
                {
                    CurrencyId = req.CurrencyId,
                    Date = req.Date,
                    Price = req.Price,
                    StockId = req.StockId,
                };
                var result = await _stockHistoryRepository.AddAsync(history);
                stock.LastPrice = req.Price;
                stock.CurrencyId = currency.Id;
                await _stockRepository.UpdateAsync(stock);

                result.Currency = currency;
                return result;
            });
        }

        public async Task<Stock> DeleteAsync(long id, long userId)
        {
            var stock = await _stockRepository.GetAsync(id) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);
            if (stock.IsGlobal)
            {
                //todo проверить, удалится каскадом?
                if(!await _userService.IsAdminAsync(userId))
                {
                    throw new SomeCustomNotAllowedException();
                }
            }
            else
            {
                //if (!await _portfolioRepository.ExistAsync(stock.PortfolioId.Value, userId))
                //    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
                if(stock.UserId!=userId)
                    throw new SomeCustomNotAllowedException();
            }

            return await _stockRepository.DeleteAsync(stock);

        }

        public async Task FillHistoryAsync(long stockId, long userId)
        {
            if (!await _userService.IsAdminAsync(userId))
            {
                throw new SomeCustomNotAllowedException();
            }

            var stock = await _stockRepository.GetGlobalNoTrackAsync(stockId) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);
            var tRequest = stock.ToTInvestHistoryRequest(_datetimeProvider.CurrentDateTime());

            var tHistory = await _priceService.GetHistory(tRequest);
            if (tHistory == null || tHistory.Count == 0)
            {
                return;
            }


            var appCurrency = await _stockRepository.GetGlobalByCodeNoTrack(tHistory.First().CurrencyCode);
            var historyForAdd = tHistory.Select(x => new StockHistory()
            {
                CurrencyId = appCurrency.Id,
                Date = x.Date,
                Price = x.Price,
                StockId = stock.Id,
            });


            await _dbHelper.ActionInTransaction(_db, async () =>
            {
                //загружаем снова в транзакции
                stock = await _stockRepository.GetGlobalAsync(stockId) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);
                await _stockHistoryRepository.AddAsync(historyForAdd);

                var lastHistory = tHistory.OrderByDescending(x => x.Date).Last();
                stock.ActualizationTime = lastHistory.Date;
                stock.LastPrice = lastHistory.Price;
                stock.CurrencyId = appCurrency.Id;
                await _stockRepository.UpdateAsync(stock);
            });

        }

        public async Task<List<Stock>> FindAsync( string text, long userId)
        {
            //if (portfolioId != null && !await _portfolioRepository.ExistAsync(portfolioId.Value, userId))
            //    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            return await _stockRepository.FindAsync( text, userId);
        }

        public async Task<List<Stock>> GetAsync(long userId)
        {
            return await _stockRepository.GetForUserAsync( userId);

        }

        public async Task<Stock> GetAsync(long id, long userId)
        {
            return await _stockRepository.GetAsync(id, userId) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);
        }

        public async Task<List<Stock>> GetCurrencyAsync( long userId)
        {
            //if (portfolioId != null && !await _portfolioRepository.ExistAsync(portfolioId.Value, userId))
            //    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            return await _stockRepository.GetCurrencyAsync(userId);

        }

        public async Task<(List<StockHistory>, long)> GetHistoryAsync(long id, long userId, int pageSize, int pageNum)
        {
            var stock = await _stockRepository.GetAsync(id, userId) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);

            var history = await _stockHistoryRepository.GetHistoryWithCurrencyAsync(id,pageSize,pageNum);
            var count = await _stockHistoryRepository.GetHistoryCountAsync(id);
            return (history, count);

        }

        public async Task GlobalActualizeAsync(long userId)
        {
            if (!await _userService.IsAdminAsync(userId))
            {
                throw new SomeCustomNotAllowedException();
            }

            var notActual = await _stockRepository.GetGlobalForActualiztionAsync(_datetimeProvider.CurrentDateTime().AddHours(6));
            var tRequests = notActual.Where(x => x.Type != StockTypeEnum.Other).Select(x => x.ToTInvestPriceRequest()).ToList();
            var tPrices = await _priceService.GetPrice(tRequests);
            var history = new List<StockHistory>();

            await _dbHelper.ActionInTransaction(_db, async () =>
            {
                //достаем из бд вторым запросом что бы засунуть это в транзакцию потом, а запрос с получением цен вынести из транзакции
                var forUpdate = await _stockRepository.GetAsync(notActual.Select(x => x.Id).ToList());
                //var tCurrency = tPrices.Select(x => x.CurrencyCode).Distinct();//тут надо брать еще и id а не только CurrencyCode + еще фильтровать по тому валюта или нет
                var appCurrency = await _stockRepository.GetGlobalAsync();
                foreach (var stock in forUpdate)
                {
                    var newVal = tPrices.FirstOrDefault(x => x.Code == stock.Code);
                    if (newVal == null)
                    {
                        //рассчитываем обратный курс для условного рубля
                        var newValByCurrency = tPrices.FirstOrDefault(x => x.CurrencyCode == stock.Code
                            && forUpdate.FirstOrDefault(s => s.Code == x.Code)?.Type == StockTypeEnum.Currency);
                        //надо обязательно проверить что мы нашли валюту именно а не акцию
                        if (newValByCurrency == null)//ничего не нашли
                            continue;
                        newVal = new PriceResponseDto() { CurrencyCode = newValByCurrency.Code, Code = stock.Code, Price = 1 / newValByCurrency.Price };

                    }

                    var curr = appCurrency.FirstOrDefault(x => x.Code == newVal.CurrencyCode);
                    var dateNow = _datetimeProvider.CurrentDateTime();
                    stock.LastPrice = newVal.Price;
                    stock.CurrencyId = curr.Id;//todo у валюты есть это поле? у всей валюты? есть какая то главная валюта?
                    stock.ActualizationTime = dateNow;
                    //todo CurrencyId

                    history.Add(new StockHistory()
                    {
                        CurrencyId = curr.Id,
                        Date = dateNow,
                        Price = newVal.Price,
                        StockId = stock.Id,
                    });

                }

                await _stockRepository.UpdateAsync(forUpdate);
                await _stockHistoryRepository.AddAsync(history);
            });
        }

        public async Task<Stock> UpdateAsync(CreateStock obj, long userId)
        {
            var stock = await _stockRepository.GetAsync(obj.Id) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);

            if (stock.IsGlobal)
            {
                if (!await _userService.IsAdminAsync(userId))
                {
                    throw new SomeCustomNotAllowedException();
                }
            }
            else
            {
                if (stock.UserId != userId)
                    throw new SomeCustomNotAllowedException();
            }

            stock.Name = obj.Name;
            stock.Code = obj.Code;
            //stock.LastPrice = obj.LastPrice;
            var result = await _stockRepository.UpdateAsync(stock);
            //var history = GetHistory(result);
            //await _stockHistoryRepository.AddAsync(history);
            return result;
        }


        //private StockHistory GetHistory(Stock stock)
        //{
        //    return new StockHistory()
        //    {
        //        Date = stock.ActualizationTime,
        //        Price = stock.LastPrice,
        //        StockId = stock.Id,
        //        CurrencyId = stock.CurrencyId,
        //    };
        //}


    }
}
