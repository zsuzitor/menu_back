using BL.Models.Services.Interfaces;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using Common.Models.Exceptions;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using FinancialAssistantApp.Models.Services.Interfaces;
using Menu.Models.Services.Interfaces;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IStockHistoryRepository _stockHistoryRepository;
        private readonly IDateTimeProvider _datetimeProvider;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IUserService _userService;

        public StockService(IStockRepository stockRepository, IDateTimeProvider datetimeProvider, IPortfolioRepository portfolioRepository, IStockHistoryRepository stockHistoryRepository, IUserService userService)
        {
            _stockRepository = stockRepository;
            _datetimeProvider = datetimeProvider;
            _portfolioRepository = portfolioRepository;
            _stockHistoryRepository = stockHistoryRepository;
            _userService = userService;
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
                rec.PortfolioId = obj.PortfolioId;
                if (rec.PortfolioId == null)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
                }

                if (!await _portfolioRepository.ExistAsync(rec.PortfolioId.Value, userId))
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
                }
            }

            //if (obj.Type != StockTypeEnum.Currency)
            //{
            //    if (obj.CurrencyId == null)
            //    {
            //        throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundCurrency);
            //    }

            //    var cur = await _stockRepository.GetNoTrackAsync(obj.CurrencyId.Value);
            //    if (cur == null || (cur.Type != StockTypeEnum.Currency))
            //        throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundCurrency);
            //    if (!cur.IsGlobal && rec.IsGlobal)
            //    {
            //        //валюта не глобальная а сток глобальный - ошибка
            //        throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundCurrency);
            //    }

            //    if (!cur.IsGlobal && !rec.IsGlobal && cur.PortfolioId != rec.PortfolioId)
            //    {
            //        //ссылаемся на валюту из чужого портфеля
            //        throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundCurrency);

            //    }


            //}

            var result = await _stockRepository.AddAsync(rec);
            var history = GetHistory(result);
            await _stockHistoryRepository.AddAsync(history);
            return result;

        }

        public async Task<Stock> DeleteAsync(long id, long userId)
        {
            var stock = await _stockRepository.GetAsync(id) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);
            if (stock.IsGlobal)
            {
                //todo проверить права
            }
            else
            {
                if (!await _portfolioRepository.ExistAsync(stock.PortfolioId.Value, userId))
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }

            return await _stockRepository.DeleteAsync(stock);

        }

        public async Task<List<Stock>> FindAsync(long? portfolioId, string text, long userId)
        {
            if (portfolioId != null && !await _portfolioRepository.ExistAsync(portfolioId.Value, userId))
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            return await _stockRepository.FindAsync(portfolioId, text);
        }

        public async Task GlobalActualizeAsync(long userId)
        {
            //todo проверить права
            var now = _datetimeProvider.CurrentDateTime();

            var records = await _stockRepository.GetGlobalForActualiztionAsync(now.AddHours(-8));
            //todo тут ходить куда то и узнать цены
            var history = new List<StockHistory>();
            foreach (var rec in records)
            {
                rec.ActualizationTime = now;
                history.Add(GetHistory(rec));
            }

            await _stockHistoryRepository.AddAsync(history);

            await _stockRepository.UpdateAsync(records);

        }

        public async Task<Stock> UpdateAsync(CreateStock obj, long userId)
        {
            var stock = await _stockRepository.GetAsync(obj.Id) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);

            if (stock.IsGlobal)
            {
                //todo проверить права
            }
            else
            {
                if (!await _portfolioRepository.ExistAsync(stock.PortfolioId.Value, userId))
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }

            stock.Name = obj.Name;
            stock.Code = obj.Code;
            //stock.LastPrice = obj.LastPrice;
            var result = await _stockRepository.UpdateAsync(stock);
            var history = GetHistory(result);
            await _stockHistoryRepository.AddAsync(history);
            return result;
        }


        private StockHistory GetHistory(Stock stock)
        {
            return new StockHistory()
            {
                Date = stock.ActualizationTime,
                Price = stock.LastPrice,
                StockId = stock.Id,
                CurrencyId = stock.CurrencyId,
            };
        }
    }
}
