using BL.Models.Services.Interfaces;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using Common.Models.Exceptions;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using FinancialAssistantApp.Models.Handlers;
using FinancialAssistantApp.Models.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Org.BouncyCastle.Ocsp;
using System.Collections;
using System.Xml.Linq;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
using Tinkoff.InvestApi.V1;
using static FinancialAssistantApp.Models.Handlers.CurrencyConvertHandler;

namespace FinancialAssistantApp.Models.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IStockElementRepository _stockElementRepository;
        private readonly IStockEventRepository _stockEventRepository;
        protected readonly IDateTimeProvider _dateTimeProvider;

        public PortfolioService(IPortfolioRepository portfolioRepository, IStockRepository stockRepository, IStockElementRepository stockElementRepository, IStockEventRepository stockEventRepository, IDateTimeProvider dateTimeProvider)
        {
            _portfolioRepository = portfolioRepository;
            _stockRepository = stockRepository;
            _stockElementRepository = stockElementRepository;
            _stockEventRepository = stockEventRepository;
            _dateTimeProvider = dateTimeProvider;
        }


        public async Task<Portfolio> CreateAsync(PortfolioCreate obj, long userId)
        {
            if (obj.CurrencyId != null)
            {
                var currency = await _stockRepository.GetNoTrackAsync(obj.CurrencyId.Value) ?? throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
                if (!currency.IsGlobal)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);

                }
            }

            var rec = new Portfolio
            {
                Name = obj.Name,
                UserId = userId,
                CurrencyId = obj.CurrencyId
            };
            return await _portfolioRepository.AddAsync(rec);
        }

        public async Task<Portfolio> UpdateAsync(PortfolioCreate obj, long userId)
        {
            var rec = await _portfolioRepository.GetAsync(obj.Id, userId);
            if (rec?.UserId != userId)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }

            rec.Name = obj.Name;
            rec.CurrencyId = obj.CurrencyId;
            return await _portfolioRepository.UpdateAsync(rec);

        }

        public async Task<Portfolio> DeleteAsync(long id, long userId)
        {
            var rec = await _portfolioRepository.GetAsync(id, userId);
            if (rec?.UserId != userId)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }

            return await _portfolioRepository.DeleteAsync(rec);

        }

        public async Task<List<Portfolio>> GetAllAsync(long userId)
        {
            return await _portfolioRepository.GetAllAsync(userId);
        }

        public async Task<Portfolio> GetAsync(long id, long userId)
        {
            return await _portfolioRepository.GetWithCurrencyNoTrackAsync(id, userId) ?? throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
        }

        public async Task<PortfolioStatistic> GetStatisticAsync(PortfolioStatisticRequestDto req, long userId)
        {
            var result = new PortfolioStatistic();
            var portfolios = await _portfolioRepository.GetAllAsync(req.PortfolioId, userId);
            if (portfolios.Count != req.PortfolioId.Count)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }


            var converter = new CurrencyConvertHandler();
            //_stockRepository;
            var elements = await _stockElementRepository.GetWithStockNoTrack(req.PortfolioId);
            var elementIds = elements.Select(x => x.Id).ToList();
            var elementById = elements.ToDictionary(x => x.Id);

            var stocks = await _stockRepository.GetForUserWithHistoryAsync(userId);
            var currency = stocks.Where(x => x.Type == StockTypeEnum.Currency).ToList();


            if (!currency.Any(x => x.Id == req.CurrencyId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundCurrency);

            }

            //var stockIds = elements.Select(x => x.StockId).Distinct().ToList();
            var events = await _stockEventRepository.GetEvents(elementIds, req.Start, req.End);


            //вся история цен для пары за все время
            Dictionary<(long curId1, long curId2), List<ConvertElement>> pairHistory = converter.GetPairHistory(currency);
            //на данный момент, по сути просто оптимизация




            // только пополнения
            var replenishments = events
                .Where(x => x.Type == StockEventEnum.CashReplenishment)
                .ToList();

            (result.CashReplenishmentSum, result.ReplenishmentsByCurrency) = GetMoneySumFromEvents(replenishments, req.CurrencyId, elementById, pairHistory);

            //выводы
            var withdrawal = events
                .Where(x => x.Type == StockEventEnum.WithdrawalCash)
                .ToList();
            (result.WithdrawalCashSum, result.WithdrawalCashByCurrency) = GetMoneySumFromEvents(withdrawal, req.CurrencyId, elementById, pairHistory);


            //дивиденды
            var dividends = events
                .Where(x => x.Type == StockEventEnum.Dividends)
                .ToList();
            (result.DividendsCashSum, result.DividendsCashByCurrency) = GetMoneySumFromEvents(dividends, req.CurrencyId, elementById, pairHistory, false);

            var actualPairPrice = converter.GetPairHistoryActualPrice(currency);
            result.SumNow = CalculateElementsSum(req.CurrencyId, elements, actualPairPrice);



            var eventsByElementId = new Dictionary<long, List<StockEvent>>();
            var eventsByMainElement = events.GroupBy(x => x.MainElementId);
            var eventsBySubElement = events.Where(x => x.SubElementId != null).GroupBy(x => x.SubElementId);

            //если за выбранный период не было ивентов на какую то акцию то идем ивент ДО периода тк там есть сума на начало
            var allPortfolioActualEventsOnPerionStart = await _stockEventRepository.GetLastActualEvents(req.PortfolioId, req.Start);

            foreach (var e in eventsByMainElement)
            {
                eventsByElementId.Add(e.Key, e.ToList());
            }

            foreach (var e in eventsBySubElement)
            {
                if (eventsByElementId.ContainsKey(e.Key.Value))
                {
                    eventsByElementId[e.Key.Value].AddRange(e.ToList());
                }
                else
                {
                    eventsByElementId.Add(e.Key.Value, e.ToList());
                }
            }

            foreach (var elem in elements)
            {
                if (!eventsByElementId.ContainsKey(elem.Id))
                {
                    var ev = allPortfolioActualEventsOnPerionStart.First(x => x.MainElementId == elem.Id || x.SubElementId == elem.Id);
                    eventsByElementId.Add(elem.Id, new List<StockEvent>() { ev });

                }

            }


            foreach (var e in eventsByElementId)
            {
                var elemEvents = e.Value;
                var element = elementById[e.Key];


                //var orderedElemEvents = elemEvents.OrderBy(x => x.Date);
                //var firstEvent = orderedElemEvents.First();
                //var lastEvent = orderedElemEvents.Last();
                //if (elemEvents.Count == 0)
                //{
                //    //todo запрос  цикле
                //    _stockEventRepository.GetLastActualEvent(element.Id);
                //}

                var orderedElemEvents = elemEvents.OrderBy(x => x.EventDateTime);
                var firstEvent = orderedElemEvents.First();
                if (firstEvent.EventDateTime >= req.Start)
                {
                    //если дата в диапазоне то значение уже изменено этим ивентом, а нам нужно предыдущее
                    //asd
                    //todo
                }

                var elementStock = stocks.FirstOrDefault(x => x.Id == element.StockId);

                var priceStart = converter.GetElementPriceOnEvent(elementStock, element, firstEvent,
                    req.CurrencyId, pairHistory);
                result.SumOnStartPeriod += priceStart;


                var lastEvent = orderedElemEvents.Last();
                var priceEnd = converter.GetElementPriceOnEvent(elementStock, element, lastEvent,
                    req.CurrencyId, pairHistory);

                result.SumOnEndPeriod += priceEnd;

            }


            return result;

        }



        private decimal CalculateElementsSum(
            long destinationCurrencyId, List<StockElement> elements, Dictionary<(long curId1, long curId2), List<ConvertElement>> currencyPairHistory)
        {
            var result = 0m;
            var converter = new CurrencyConvertHandler();
            //тут могут быть валюты которые не надо конвертить
            //валюты которые надо конвертить 
            //НЕ валюты у которых есть цена в валюте
            foreach (var element in elements)
            {

                var price = converter.GetElementPriceOnDate(element.Stock,_dateTimeProvider.CurrentDateTime(), element.Count, element.Stock.LastPrice, element.Stock.CurrencyId.Value, destinationCurrencyId, currencyPairHistory);
                result += price;
            }

            return result;
        }

        /// <summary>
        /// посчитать сумму CountChange по ивентам и перевести все в валюту
        /// </summary>
        /// <param name="events">события по котором надо посчитать сумму изменений</param>
        /// <param name="destinationCurrencyId">в какой валюте результат</param>
        /// <param name="elementById">словарь элементов по которым именты</param>
        /// <param name="pairHistory">история пар</param>
        /// <param name="moneyFromMainElement">считать по зависимым или главным элементам в ивенте</param>
        /// <returns></returns>
        /// <exception cref="SomeCustomException"></exception>
        private (decimal, Dictionary<long, decimal>) GetMoneySumFromEvents(
            List<StockEvent> events,
            long destinationCurrencyId,
            Dictionary<long, StockElement> elementById,
            Dictionary<(long curId1, long curId2), List<ConvertElement>> pairHistory,
            bool moneyFromMainElement = true)
        {
            var converter = new CurrencyConvertHandler();
            decimal totalSum = 0;
            Dictionary<long, decimal> totalSumByCurrency = new Dictionary<long, decimal>();
            foreach (var ev in events)
            {

                var element = elementById[moneyFromMainElement ? ev.MainElementId : ev.SubElementId.Value];
                if (!totalSumByCurrency.ContainsKey(element.StockId))
                    totalSumByCurrency.Add(element.StockId, 0);
                var moneyFromEventChange = moneyFromMainElement ? ev.MainCountChange : ev.SubCountChange.Value;
                totalSumByCurrency[element.StockId] += moneyFromEventChange;

                totalSum += converter.GetCurrencyPriceOnDate(element.StockId, ev.EventDateTime, moneyFromEventChange, destinationCurrencyId, pairHistory);
            }
            return (totalSum, totalSumByCurrency);
        }


        //public decimal GetElementPriceOnDate(Stock stock, long count,
        //    long destinationCurrencyId, List<CurrencyConvertHandler.ConvertElement> currencyToConvertPrice)
        //{
        //    var converter = new CurrencyConvertHandler();
        //    if (stock.Type == StockTypeEnum.Currency)
        //    {
        //        return converter.ToCurrency(currencyToConvertPrice, stock.Id,
        //              count,
        //               destinationCurrencyId) ?? throw new SomeCustomException($"Не смогли сконвертировать валюту из {stock.Id} в {destinationCurrencyId}");
        //    }
        //    else
        //    {
        //        return count * converter.ToCurrency(currencyToConvertPrice, stock.CurrencyId.Value,
        //              element.Stock.LastPrice,
        //               destinationCurrencyId) ?? throw new SomeCustomException($"Не смогли сконвертировать валюту из {stock.Id} в {destinationCurrencyId}");

        //    }
        //}



    }
}
