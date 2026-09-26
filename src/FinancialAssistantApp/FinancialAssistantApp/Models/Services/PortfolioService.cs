using BL.Models.Services.Interfaces;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using Common.Models.Exceptions;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using FinancialAssistantApp.Models.Handlers;
using FinancialAssistantApp.Models.Handlers.CreateEventHandlers;
using FinancialAssistantApp.Models.Services.Interfaces;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;
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
        private readonly CreateEventFactory _createEventFactory;

        public PortfolioService(IPortfolioRepository portfolioRepository, IStockRepository stockRepository, IStockElementRepository stockElementRepository, IStockEventRepository stockEventRepository, IDateTimeProvider dateTimeProvider, CreateEventFactory createEventFactory)
        {
            _portfolioRepository = portfolioRepository;
            _stockRepository = stockRepository;
            _stockElementRepository = stockElementRepository;
            _stockEventRepository = stockEventRepository;
            _dateTimeProvider = dateTimeProvider;
            _createEventFactory = createEventFactory;
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
            if (req.Start <= req.End)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);//todo другая ошибка

            }

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


            //var eventsByElementId = new Dictionary<long, List<StockEvent>>();
            var eventsByMainElement = events.OrderBy(x=>x.EventDateTime).GroupBy(x => x.MainElementId).ToDictionary(x=>x.Key);
            var eventsBySubElement = events.OrderBy(x => x.EventDateTime).Where(x => x.SubElementId != null).GroupBy(x => x.SubElementId).ToDictionary(x => x.Key);

            //если за выбранный период не было ивентов на какую то акцию то идем ивент ДО периода тк там есть сума на начало
            var allPortfolioActualEventsOnPerionStartMain = await _stockEventRepository.GetLastActualEventsForMainElement(req.PortfolioId, req.Start);
            var allPortfolioActualEventsOnPerionStartSub = await _stockEventRepository.GetLastActualEventsForSubElement(req.PortfolioId, req.Start);
            var allPortfolioActualEventsOnPerionEndMain = await _stockEventRepository.GetLastActualEventsForMainElement(req.PortfolioId, req.End);
            var allPortfolioActualEventsOnPerionEndSub = await _stockEventRepository.GetLastActualEventsForSubElement(req.PortfolioId, req.End);



            foreach (var elem in elements)
            {
                var elementStock = stocks.FirstOrDefault(x => x.Id == elem.StockId);
                //var count = GetElemSumOnDate(elem.Id, elementStock, pairHistory,req.Start,req.CurrencyId);
                {
                    var countStart = GetElemCount(
                    allPortfolioActualEventsOnPerionStartMain.FirstOrDefault(x => x.MainElementId == elem.Id),
                    allPortfolioActualEventsOnPerionStartSub.FirstOrDefault(x => x.SubElementId == elem.Id),
                    eventsByMainElement.TryGetValue(elem.Id, out var mainEventList) ? mainEventList.FirstOrDefault() : null,
                    eventsBySubElement.TryGetValue(elem.Id, out var subEventList) ? subEventList.FirstOrDefault() : null
                    );

                    if (countStart != 0)
                    {
                        //ивентов не нашли, элемент создан ивентом ЗА диапазоном
                        result.SumOnStartPeriod += converter.GetElementPriceOnEvent(elementStock, countStart,
                            req.Start,//думаю что правильно передавать дату начала периода а не дату ивента тк нам цена именно на начало периода нужна для статистики priceDate.Value,
                            req.CurrencyId, pairHistory);
                    }
                }

                {
                    var countEnd = GetElemCount(
                        allPortfolioActualEventsOnPerionEndMain.FirstOrDefault(x => x.MainElementId == elem.Id),
                        allPortfolioActualEventsOnPerionEndSub.FirstOrDefault(x => x.SubElementId == elem.Id),
                        eventsByMainElement.TryGetValue(elem.Id, out var mainEventListEnd) ? mainEventListEnd.LastOrDefault() : null,
                        eventsBySubElement.TryGetValue(elem.Id, out var subEventListEnd) ? subEventListEnd.LastOrDefault() : null
                        );

                    if (countEnd != 0)
                    {
                        //ивентов не нашли, элемент создан ивентом ЗА диапазоном
                        result.SumOnEndPeriod += converter.GetElementPriceOnEvent(elementStock, countEnd,
                            req.End,//думаю что правильно передавать дату начала периода а не дату ивента тк нам цена именно на начало периода нужна для статистики priceDate.Value,
                            req.CurrencyId, pairHistory);
                    }
                }
            }


            //тут нужно идти по датам а не по элементам, лучше лишний раз пройти еще раз по коллекции чем потом разгребать
            var perionDate = req.Start;
            result.PeriodSums.Add(new PeriodSum() { Date = req.Start, Sum = result.SumOnStartPeriod });
            while (perionDate < req.End)
            {
                perionDate = perionDate.AddMonths(1);

                if (perionDate.AddMonths(1) >= req.End)
                {
                    result.PeriodSums.Add(new PeriodSum() { Date = req.End, Sum = result.SumOnEndPeriod });
                    break;
                }

                var sum = 0m;
                foreach (var elem in elements)
                {
                    var elementStock = stocks.FirstOrDefault(x => x.Id == elem.StockId);


                    var mainFirstInPeriod = eventsByMainElement.TryGetValue(elem.Id, out var mainEventList) ? mainEventList.Where(x => x.EventDateTime >= perionDate).FirstOrDefault() : null;
                    var mainLastBeforePeriod = eventsByMainElement.TryGetValue(elem.Id, out var mainEventListBefore) ? mainEventListBefore.Where(x => x.EventDateTime < perionDate).LastOrDefault() : null;
                    if (mainLastBeforePeriod == null)
                    {
                        mainLastBeforePeriod = allPortfolioActualEventsOnPerionStartMain.FirstOrDefault(x => x.MainElementId == elem.Id);
                    }

                    var subFirstInPeriod = eventsBySubElement.TryGetValue(elem.Id, out var subEventList) ? subEventList.Where(x => x.EventDateTime >= perionDate).FirstOrDefault() : null;
                    var subLastBeforePeriod = eventsBySubElement.TryGetValue(elem.Id, out var subEventListBefore) ? subEventListBefore.Where(x => x.EventDateTime < perionDate).LastOrDefault() : null;
                    if (subLastBeforePeriod == null)
                    {
                        subLastBeforePeriod = allPortfolioActualEventsOnPerionStartSub.FirstOrDefault(x => x.SubElementId == elem.Id);
                    }

                    var countOnDate = GetElemCount(
                        mainLastBeforePeriod,
                        subLastBeforePeriod,
                        mainFirstInPeriod,
                        subFirstInPeriod
                        );

                    if (countOnDate != 0)
                    {
                        //ивентов не нашли, элемент создан ивентом ЗА диапазоном

                        sum += converter.GetElementPriceOnEvent(elementStock, countOnDate,
                            perionDate,//думаю что правильно передавать дату начала периода а не дату ивента тк нам цена именно на начало периода нужна для статистики priceDate.Value,
                            req.CurrencyId, pairHistory);
                    }
                }

                result.PeriodSums.Add(new PeriodSum() { Date = perionDate, Sum = sum });
            }


            //foreach (var e in eventsByMainElement)
            //{
            //    eventsByElementId.Add(e.Key, e.ToList());
            //}

            //foreach (var e in eventsBySubElement)
            //{
            //    if (eventsByElementId.ContainsKey(e.Key.Value))
            //    {
            //        eventsByElementId[e.Key.Value].AddRange(e.ToList());
            //    }
            //    else
            //    {
            //        eventsByElementId.Add(e.Key.Value, e.ToList());
            //    }
            //}

            //foreach (var elem in elements)
            //{
            //    if (!eventsByElementId.ContainsKey(elem.Id))
            //    {
            //        var ev = allPortfolioActualEventsOnPerionStart.First(x => x.MainElementId == elem.Id || x.SubElementId == elem.Id);
            //        eventsByElementId.Add(elem.Id, new List<StockEvent>() { ev });

            //    }

            //}


            //foreach (var e in eventsByElementId)
            //{
            //    var elemEvents = e.Value;
            //    var element = elementById[e.Key];


            //    //var orderedElemEvents = elemEvents.OrderBy(x => x.Date);
            //    //var firstEvent = orderedElemEvents.First();
            //    //var lastEvent = orderedElemEvents.Last();
            //    //if (elemEvents.Count == 0)
            //    //{
            //    //    //todo запрос  цикле
            //    //    _stockEventRepository.GetLastActualEvent(element.Id);
            //    //}

            //    var orderedElemEvents = elemEvents.OrderBy(x => x.EventDateTime);
            //    var firstEvent = orderedElemEvents.First();
            //    if (firstEvent.EventDateTime >= req.Start)
            //    {
            //        //если дата в диапазоне то значение уже изменено этим ивентом, а нам нужно предыдущее
            //        //asd
            //        //todo
            //    }

            //    var elementStock = stocks.FirstOrDefault(x => x.Id == element.StockId);

            //    var priceStart = converter.GetElementPriceOnEvent(elementStock, element, firstEvent,
            //        req.CurrencyId, pairHistory);
            //    result.SumOnStartPeriod += priceStart;


            //    var lastEvent = orderedElemEvents.Last();
            //    var priceEnd = converter.GetElementPriceOnEvent(elementStock, element, lastEvent,
            //        req.CurrencyId, pairHistory);

            //    result.SumOnEndPeriod += priceEnd;

            //}


            return result;

        }


        /// <summary>
        /// есть некий интвервал ивентов, допустим с января по февраль, на любую точку в этом интервали можно найти количество
        /// </summary>
        /// <param name="lastEventBeforeMain">последний ивент до даты(точки) где элемент основной</param>
        /// <param name="lastEventBeforeSub">последний ивент до даты(точки) где элемент зависимый</param>
        /// <param name="eventAfterMain">первый ивент после точки</param>
        /// <param name="eventAfterSub">первый ивент после точки</param>
        /// <returns></returns>
        private decimal GetElemCount(
            StockEvent lastEventBeforeMain, StockEvent lastEventBeforeSub,
            StockEvent eventAfterMain, StockEvent eventAfterSub
            )
        {
            DateTime? priceDate = null;
            decimal elemCount = 0;
            var converter = new CurrencyConvertHandler();
            if (lastEventBeforeMain != null)
            {
                priceDate = lastEventBeforeMain.EventDateTime;
                elemCount = lastEventBeforeMain.MainCountNow;
            }

            if (lastEventBeforeSub != null && (priceDate == null || priceDate < lastEventBeforeSub.EventDateTime))
            {
                //по sub элементу более актуальная дата(она ближе к интересующей нас дате), надо брать его
                priceDate = lastEventBeforeSub.EventDateTime;
                elemCount = lastEventBeforeSub.SubCountNow.Value;

            }

            //если мы нашли то дальше смотреть смысла нет тк это самый подходящий и точный вариант
            if (priceDate == null)
            {
                //ивента до периода не нашли, будем искать в рамках и пытаться его откатить
                if (eventAfterMain != null)
                {
                    priceDate = eventAfterMain.EventDateTime;
                    var rollback = _createEventFactory.Get(eventAfterMain.Type, 0).GetRollBackCountChange(eventAfterMain);
                    var c = rollback.First(x => x.MainElementId == eventAfterMain.MainElementId).MainCountNow;
                    elemCount = c;//eventAfterMain.MainCountNow ?? 0; это значние после ивента, его надо откатить
                }

                if (eventAfterSub != null && (priceDate == null || eventAfterSub.EventDateTime < priceDate))
                {
                    //ивента до периода не нашли, будем искать в рамках и пытаться его откатить
                    priceDate = eventAfterSub.EventDateTime;
                    var rollback = _createEventFactory.Get(eventAfterSub.Type, 0).GetRollBackCountChange(eventAfterSub);
                    var c = rollback.First(x => x.MainElementId == eventAfterSub.SubElementId).MainCountNow;
                    elemCount = c;//eventAfterSub.SubCountNow ?? 0; это значние после ивента, его надо откатить
                }
            }


            return elemCount;
        }


        //private decimal GetElemSumOnDate(long elementId, Stock elementStock,
        //   Dictionary<(long curId1, long curId2), List<ConvertElement>> pairHistory,
        //   DateTime datetime,//дата на которую надо узнать
        //                     //long eventCurrencyId,
        //   long destinationCurrencyId,
        //   StockEvent lastEventBeforeMain, StockEvent lastEventBeforeSub,
        //   StockEvent eventInMain, StockEvent lastEventBeforeSub,

        //   )
        //{
            

        //    if (priceDate != null)
        //    {
        //        //ивентов не нашли, элемент создан ивентом ЗА диапазоном
        //        return converter.GetElementPriceOnEvent(elementStock, elemCount,
        //            datetime,//думаю что правильно передавать дату начала периода а не дату ивента тк нам цена именно на начало периода нужна для статистики priceDate.Value,
        //            destinationCurrencyId, pairHistory);
        //    }

        //    return 0;
        //}



        /// <summary>
        /// сумма по элементам на данный момент переведенная в валюту
        /// </summary>
        /// <param name="destinationCurrencyId"></param>
        /// <param name="elements"></param>
        /// <param name="currencyPairHistory"></param>
        /// <returns></returns>
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
