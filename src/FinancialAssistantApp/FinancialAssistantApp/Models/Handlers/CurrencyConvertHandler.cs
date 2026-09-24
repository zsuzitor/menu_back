using BO.Models.FinancialAssistant.DAL;
using Common.Models.Exceptions;
using Org.BouncyCastle.Ocsp;
using System.Xml.Linq;
using static Google.Api.ResourceDescriptor.Types;

namespace FinancialAssistantApp.Models.Handlers
{
    public class CurrencyConvertHandler
    {
        //private decimal ToCurrency(List<Stock> currencys, long fromCurr, decimal fromSum, long toCurr)
        //{
        //    // Если исходная и целевая валюта совпадают - возвращаем сумму
        //    if (fromCurr == toCurr)
        //        return fromSum;

        //    // Прямая конвертация
        //    var direct = currencys.FirstOrDefault(x => x.Id == fromCurr && x.CurrencyId == toCurr);
        //    if (direct != null)
        //    {
        //        return direct.LastPrice * fromSum;
        //    }

        //    // Обратная конвертация (если есть пара toCurr -> fromCurr)
        //    var reverse = currencys.FirstOrDefault(x => x.Id == toCurr && x.CurrencyId == fromCurr);
        //    if (reverse != null)
        //    {
        //        return fromSum / reverse.LastPrice;
        //    }

        //    // Поиск цепочки конвертации через промежуточные валюты (BFS)
        //    var chain = FindConversionChain(currencys, fromCurr, toCurr);
        //    if (chain != null && chain.Count > 0)
        //    {
        //        decimal result = fromSum;
        //        long currentCurrency = fromCurr;

        //        foreach (var step in chain)
        //        {
        //            // step - это Stock, где Id = текущая валюта, CurrencyId = следующая валюта
        //            var rate = currencys.First(x => x.Id == currentCurrency && x.CurrencyId == step.CurrencyId);
        //            result *= rate.LastPrice;
        //            currentCurrency = step.CurrencyId;
        //        }

        //        return result;
        //    }

        //    // Если ничего не найдено - выбрасываем исключение или возвращаем 0
        //    throw new Exception($"Не удалось найти курс конвертации между валютами {fromCurr} и {toCurr}");
        //}

        ///// <summary>
        ///// Поиск цепочки конвертации с помощью BFS (поиск в ширину)
        ///// </summary>
        //private List<Stock> FindConversionChain(List<Stock> currencys, long fromCurr, long toCurr)
        //{
        //    // Строим граф: валюта -> список доступных курсов из этой валюты
        //    var graph = currencys
        //        .GroupBy(x => x.Id)
        //        .ToDictionary(g => g.Key, g => g.ToList());

        //    // BFS
        //    var queue = new Queue<long>();
        //    var visited = new HashSet<long> { fromCurr };
        //    var parent = new Dictionary<long, Stock>(); // для восстановления пути

        //    queue.Enqueue(fromCurr);

        //    while (queue.Count > 0)
        //    {
        //        var current = queue.Dequeue();

        //        if (!graph.ContainsKey(current))
        //            continue;

        //        foreach (var edge in graph[current])
        //        {
        //            var next = edge.CurrencyId;

        //            if (visited.Contains(next))
        //                continue;

        //            visited.Add(next);
        //            parent[next] = edge;

        //            if (next == toCurr)
        //            {
        //                // Восстанавливаем цепочку
        //                return ReconstructChain(parent, fromCurr, toCurr);
        //            }

        //            queue.Enqueue(next);
        //        }
        //    }

        //    return null; // цепочка не найдена
        //}

        ///// <summary>
        ///// Восстановление цепочки конвертации из parent словаря
        ///// </summary>
        //private List<Stock> ReconstructChain(Dictionary<long, Stock> parent, long fromCurr, long toCurr)
        //{
        //    var chain = new List<Stock>();
        //    var current = toCurr;

        //    while (current != fromCurr)
        //    {
        //        var step = parent[current];
        //        chain.Insert(0, step);
        //        current = step.Id;
        //    }

        //    return chain;
        //}


        ////V2
        ///



        //private decimal ToCurrency(List<Stock> currencys, long fromCurr, decimal fromSum, long toCurr)
        //{
        //    //цель 100 долларов перевести в рубли
        //    var curr = currencys.FirstOrDefault(x => x.Id == fromCurr && x.CurrencyId == toCurr);
        //    if (curr != null)
        //    {
        //        return curr.LastPrice * fromSum;
        //    }
        //}


        public class ConvertElement
        {
            //тоесть курс рубль(id) доллар(CurrencyId) 0.01 (LastPrice)

            /// <summary>
            /// id элемента из которого конвертим
            /// </summary>
            public long IdFrom { get; set; }
            /// <summary>
            /// id элемента в который конвертим
            /// </summary>
            public long? IdTo { get; set; }
            /// <summary>
            /// цена конверта
            /// </summary>
            public decimal Price { get; set; }
            public DateTime DateOfPrice { get; set; }

        }


        public ConvertElement FindClosestTimePoint1(List<ConvertElement> timepoints, DateTime targetDate)
        {
            if (timepoints == null || timepoints.Count == 0)
                return null; // или throw new ArgumentException(...)

            return timepoints
                .OrderBy(tp => Math.Abs((tp.DateOfPrice - targetDate).Ticks))
                .First();
        }

        public StockHistory FindClosestTimePoint1(List<StockHistory> timepoints, DateTime targetDate)
        {
            if (timepoints == null || timepoints.Count == 0)
                return null; // или throw new ArgumentException(...)

            return timepoints
                .OrderBy(tp => Math.Abs((tp.Date - targetDate).Ticks))
                .First();
        }

        public StockHistory FindClosestTimePoint(List<StockHistory> timepoints, DateTime targetDate)
        {
            if (timepoints == null || timepoints.Count == 0)
                return null;

            StockHistory closest = timepoints[0];
            long minDiff = Math.Abs((closest.Date - targetDate).Ticks);

            for (int i = 1; i < timepoints.Count; i++)
            {
                long diff = Math.Abs((timepoints[i].Date - targetDate).Ticks);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    closest = timepoints[i];
                }
            }

            return closest;
        }

        public ConvertElement FindClosestTimePoint(List<ConvertElement> timepoints, DateTime targetDate)
        {
            if (timepoints == null || timepoints.Count == 0)
                return null;

            if (timepoints.Count == 1)
            {
                return timepoints.First();
            }

            ConvertElement closest = timepoints[0];
            long minDiff = Math.Abs((closest.DateOfPrice - targetDate).Ticks);

            for (int i = 1; i < timepoints.Count; i++)
            {
                long diff = Math.Abs((timepoints[i].DateOfPrice - targetDate).Ticks);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    closest = timepoints[i];
                }
            }

            return closest;
        }



        //лукойл - лукойл - покупка --доллар -- рубль - вроде ок
        //доллар - долар - дивиденды - доллар -- рубль   == Sub
        //доллар - долар - пополнение - доллар -- рубль  == Main
        public decimal GetElementPriceOnEvent(
            Stock stock,
            StockElement element,
            StockEvent ev,
            //long eventCurrencyId,
            long destinationCurrencyId,
            Dictionary<(long curId1, long curId2), List<ConvertElement>> pairHistory)
        {

            var elementCount = 0m;
            //var oneElementPrice = 0m;
            if (ev.MainElementId == element.Id)
            {
                //ивент для главного элемента
                elementCount = ev.MainCountNow;//количество элемента которое будем считать
                //oneElementPrice = ev.SubCountChange ?? 0;
            }
            else
            {
                //ивент для зависимого элемента
                //тоесть мы нашли ивент покупки или продажи, а элемент для которого мы нашли его это валюта
                elementCount = ev.SubCountNow.Value;//количество элемента которое будем считать
                //в зависимом элементе может быть только валюта, ее цена не нужна тк найдем через конвертацию

            }

            if (stock.Type == BO.Models.FinancialAssistant.Enums.StockTypeEnum.Currency)
            {
                return GetCurrencyPriceOnDate(stock.Id, ev.EventDateTime, elementCount, destinationCurrencyId, pairHistory);
            }


            //todo тут можно оптимизировать если покупка или продажа например то цену можно и даже лучше брать из ивента
           var history = FindClosestTimePoint(stock.StockHistory, ev.EventDateTime);
            
            return  GetElementPriceOnDate(stock, ev.EventDateTime, elementCount,
                history.Price, history.CurrencyId.Value, destinationCurrencyId, pairHistory);

        }

        /// <summary>
        /// тоесть акция {stock} {elementCount} штук  на дату {priceDate} стоила за 1 акцию {oneElementPrice} в валюте {oneElementPriceCurrencyId}
        /// </summary>
        /// <param name="elementStock"></param>
        /// <param name="priceDate"></param>
        /// <param name="elementCount"></param>
        /// <param name="oneElementPrice"></param>
        /// <param name="currencyId"></param>
        /// <param name="currencyPairHistory"></param>
        /// <exception cref="SomeCustomException"></exception>
        public decimal GetElementPriceOnDate(
            Stock stock,
            DateTime priceDate,
            decimal elementCount,
            decimal oneElementPrice,
            long oneElementPriceCurrencyId,
            long destinationCurrencyId,
            Dictionary<(long curId1, long curId2), List<ConvertElement>> currencyPairHistory)
        {


            if (stock.Type == BO.Models.FinancialAssistant.Enums.StockTypeEnum.Currency)
            {
                return GetCurrencyPriceOnDate(stock.Id, priceDate, elementCount, destinationCurrencyId, currencyPairHistory);
            }

            var actualPrice = GetOnDateFromPairHistory(priceDate, oneElementPriceCurrencyId, destinationCurrencyId, currencyPairHistory);
            return elementCount * ToCurrency(actualPrice, oneElementPriceCurrencyId,
                  oneElementPrice,
                   destinationCurrencyId) ?? throw new SomeCustomException($"Не смогли сконвертировать валюту из {oneElementPriceCurrencyId} в {destinationCurrencyId}");
        }

        public decimal GetCurrencyPriceOnDate(
           long currencyId,
           DateTime priceDate,
           decimal count,
           long destinationCurrencyId,
           Dictionary<(long curId1, long curId2), List<ConvertElement>> currencyPairHistory)
        {
            //что бы определить стоимость валюты на дату, просто ищем по истории pairHistory
            if (currencyId == destinationCurrencyId)
            {
                //валюта сама к себе в любую дату 1 к 1
                return count;
            }

            var actualPrice = GetOnDateFromPairHistory(priceDate, currencyId, destinationCurrencyId, currencyPairHistory);

            return ToCurrency(actualPrice, currencyId,
                  count,
                   destinationCurrencyId) ?? throw new SomeCustomException($"Не смогли сконвертировать валюту из {currencyId} в {destinationCurrencyId}");
        }



        /// <summary>
        /// список элементов, по 1 записи на каждую пару с наиболее актуальным(по дате) значением
        /// если найдена прямая конвертация то вернет 1 элемент цены, если нет то список актуальных цен на каждую пару
        /// </summary>
        /// <param name="date"></param>
        /// <param name="curId1"></param>
        /// <param name="curId2"></param>
        /// <param name="currencyPairHistory">список записей историй для каждой возможной пары</param>
        /// <returns></returns>
        public List<ConvertElement> GetOnDateFromPairHistory(
            DateTime date, long curId1, long curId2,
            Dictionary<(long curId1, long curId2), List<ConvertElement>> currencyPairHistory)
        {

            //валюта сама к себе в любую дату 1 к 1
            if (curId1 == curId2)
            {
                return new List<ConvertElement>()
                {
                    new ConvertElement()
                    {
                        DateOfPrice = date,
                        IdFrom = curId1,
                        IdTo = curId2,
                        Price = 1,
                    }
                };
            }

            //список элементов, по 1 записи на каждую пару с наиболее актуальным(по дате) значением
            currencyPairHistory.TryGetValue((curId1, curId2), out var simplePair1);
            currencyPairHistory.TryGetValue((curId2, curId1), out var simplePair2);
            if (simplePair1 != null)
            {
                //найдена пара
                var nearesHistory = FindClosestTimePoint(simplePair1, date);
                return new List<ConvertElement>() { nearesHistory };
            }

            if (simplePair2 != null)
            {
                //найдена обратная пара
                var nearesHistory = FindClosestTimePoint(simplePair2, date);
                return new List<ConvertElement>() { nearesHistory };
            }

            //если мы не нашли "прямую пару" то для всех пар ищем сумму на дату для того что бы рассчитать курс через другие валюты
            List<ConvertElement> forCurrencyDatePrice = new List<ConvertElement>();
            foreach (var ph in currencyPairHistory)
            {
                //для каждой пары ищем наиболее актуальную цену
                var nearesHistory = FindClosestTimePoint(ph.Value, date);
                forCurrencyDatePrice.Add(nearesHistory);
            }

            //тут можно отсечь валюты которые напрямю не нужны, но тогда уйдет "продвинутый посчет цены" в ToCurrency когда через связку нескольких валют считается
            return forCurrencyDatePrice;
        }


        public Dictionary<(long curId1, long curId2), List<ConvertElement>> GetPairHistoryActualPrice(List<Stock> currency)
        {
            Dictionary<(long curId1, long curId2), List<ConvertElement>> pairHistory = new Dictionary<(long curId1, long curId2), List<ConvertElement>>();
            foreach (var cur in currency)
            {
                var h = new ConvertElement()
                { IdFrom = cur.Id, IdTo = cur.CurrencyId.Value, DateOfPrice = cur.ActualizationTime, Price = cur.LastPrice };
                AddPair(pairHistory, h);
            }

            return pairHistory;
        }

        /// <summary>
        /// из 2х записей доллар-рубль и рубль-доллар сделает в элемент в словаре в котором будет история из обеих записей
        /// </summary>
        /// <param name="currency"></param>
        /// <returns></returns>
        public Dictionary<(long curId1, long curId2), List<ConvertElement>> GetPairHistory(List<Stock> currency)
        {
            //для каждой валюты надо найти цену наиболее подходящую к дате
            //у меня есть валюта - доллар, у него есть изменение цены в рубле, юане
            //мне надо взять всю историю цены
            //мне надо разбить на коллекции пар валют, потом среди каждой найти наиболее актуальное и оставить только его
            //d - массив в котором ключ - пара валюта\валюта а значение их общая история
            Dictionary<(long curId1, long curId2), List<ConvertElement>> pairHistory = new Dictionary<(long curId1, long curId2), List<ConvertElement>>();
            foreach (var cur in currency)
            {
                // StockHistory - может быть в разных валютах, надо как  то раскидывать по валютам
                //делаем обратную конвертацию что бы если были история и в паре рубль\доллар и в паре доллар-рубль учитывать их как общую пару
                foreach (var history in cur.StockHistory)
                {
                    var h = new ConvertElement()
                    { IdFrom = history.StockId, IdTo = history.CurrencyId.Value, DateOfPrice = history.Date, Price = history.Price };
                    AddPair(pairHistory,h);
                }
            }

            return pairHistory;
        }


        private void AddPair(Dictionary<(long curId1, long curId2), List<ConvertElement>> pairHistory, ConvertElement history)
        {

            if (pairHistory.ContainsKey((history.IdFrom, history.IdTo.Value)))
            {
                pairHistory[(history.IdFrom, history.IdTo.Value)].Add(new ConvertElement()
                { IdFrom = history.IdFrom, IdTo = history.IdTo.Value, DateOfPrice = history.DateOfPrice, Price = history.Price });
            }
            else if (pairHistory.ContainsKey((history.IdTo.Value, history.IdFrom)))
            {
                pairHistory[(history.IdTo.Value, history.IdFrom)].Add(new ConvertElement()
                { IdFrom = history.IdTo.Value, IdTo = history.IdFrom, DateOfPrice = history.DateOfPrice, Price = 1m / history.Price });
            }
            else
            {
                pairHistory.Add((history.IdFrom, history.IdTo.Value), new List<ConvertElement>() {new ConvertElement()
                                { IdFrom = history.IdFrom, IdTo = history.IdTo.Value, DateOfPrice = history.DateOfPrice, Price=history.Price } });
            }
        }


        public decimal? ToCurrency(List<ConvertElement> currencys, long fromCurr, decimal fromSum, long toCurr)
        {
            // Если исходная и целевая валюта совпадают - возвращаем сумму
            if (fromCurr == toCurr)
                return fromSum;

            // Прямая конвертация
            var direct = currencys.FirstOrDefault(x => x.IdFrom == fromCurr && x.IdTo == toCurr);
            if (direct != null)
            {
                return direct.Price * fromSum;
            }

            // Обратная конвертация (если есть пара toCurr -> fromCurr)
            var reverse = currencys.FirstOrDefault(x => x.IdFrom == toCurr && x.IdTo == fromCurr);
            if (reverse != null)
            {
                return fromSum / reverse.Price;
            }

            // Если целевая валюта - "главная" (базовая), ищем курс fromCurr -> главная
            if (IsBaseCurrency(currencys, toCurr))
            {
                var toBase = currencys.FirstOrDefault(x => x.IdFrom == fromCurr && x.IdTo == null);
                if (toBase != null)
                {
                    return toBase.Price * fromSum;
                }
            }

            // Если исходная валюта - "главная" (базовая), ищем курс главная -> toCurr
            if (IsBaseCurrency(currencys, fromCurr))
            {
                var fromBase = currencys.FirstOrDefault(x => x.IdFrom == toCurr && x.IdTo == null);
                if (fromBase != null)
                {
                    return fromSum / fromBase.Price;
                }
            }

            // Поиск цепочки конвертации через промежуточные валюты (BFS)
            var chain = FindConversionChain(currencys, fromCurr, toCurr);
            if (chain != null && chain.Count > 0)
            {
                decimal result = fromSum;
                long currentCurrency = fromCurr;

                foreach (var step in chain)
                {
                    // step - это Stock, где Id = текущая валюта, CurrencyId = следующая валюта (или null для главной)
                    var rate = currencys.First(x => x.IdFrom == currentCurrency &&
                                                    (x.IdTo == step.IdTo ||
                                                     (step.IdTo == null && x.IdTo == null)));
                    result *= rate.Price;
                    currentCurrency = step.IdTo ?? 0; // Если null - считаем это базовой валютой
                }

                return result;
            }

            // Если ничего не найдено - выбрасываем исключение или возвращаем 0
            //throw new Exception($"Не удалось найти курс конвертации между валютами {fromCurr} и {toCurr}");
            return null;
        }

        /// <summary>
        /// Проверяет, является ли валюта "главной" (базовой)
        /// </summary>
        private bool IsBaseCurrency(List<ConvertElement> currencys, long currencyId)
        {
            // Если есть запись с таким Id и CurrencyId == null, значит это базовая валюта
            return currencys.Any(x => x.IdFrom == currencyId && x.IdTo == null);
        }

        /// <summary>
        /// Поиск цепочки конвертации с помощью BFS (поиск в ширину)
        /// </summary>
        private List<ConvertElement> FindConversionChain(List<ConvertElement> currencys, long fromCurr, long toCurr)
        {
            // Строим граф: валюта -> список доступных курсов из этой валюты
            var graph = currencys
                .Where(x => x.IdTo != null || IsBaseCurrency(currencys, x.IdFrom))
                .GroupBy(x => x.IdFrom)
                .ToDictionary(g => g.Key, g => g.ToList());

            // BFS
            var queue = new Queue<long>();
            var visited = new HashSet<long> { fromCurr };
            var parent = new Dictionary<long, ConvertElement>(); // для восстановления пути

            queue.Enqueue(fromCurr);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (!graph.ContainsKey(current))
                    continue;

                foreach (var edge in graph[current])
                {
                    // Определяем следующую валюту
                    long next;
                    if (edge.IdTo == null)
                    {
                        // Если CurrencyId == null, значит это базовая валюта
                        // Но мы не знаем ее Id, поэтому пропускаем (будет обработано отдельно)
                        continue;
                    }
                    else
                    {
                        next = edge.IdTo.Value;
                    }

                    if (visited.Contains(next))
                        continue;

                    visited.Add(next);
                    parent[next] = edge;

                    if (next == toCurr)
                    {
                        // Восстанавливаем цепочку
                        return ReconstructChain(parent, fromCurr, toCurr);
                    }

                    queue.Enqueue(next);
                }
            }

            // Если не нашли через обычные связи, пробуем найти через базовую валюту
            return FindChainThroughBaseCurrency(currencys, fromCurr, toCurr);
        }

        /// <summary>
        /// Поиск цепочки через базовую валюту (если она есть)
        /// </summary>
        private List<ConvertElement> FindChainThroughBaseCurrency(List<ConvertElement> currencys, long fromCurr, long toCurr)
        {
            var baseCurrency = currencys.FirstOrDefault(x => x.IdTo == null);
            if (baseCurrency == null)
                return null;

            var chain = new List<ConvertElement>();
            long baseId = baseCurrency.IdFrom;

            // Проверяем путь fromCurr -> базовая валюта
            var toBase = currencys.FirstOrDefault(x => x.IdFrom == fromCurr && x.IdTo == null);
            if (toBase != null)
            {
                chain.Add(toBase);

                // Проверяем путь базовая валюта -> toCurr (обратный курс)
                var fromBase = currencys.FirstOrDefault(x => x.IdFrom == toCurr && x.IdTo == null);
                if (fromBase != null)
                {
                    chain.Add(fromBase);
                    return chain;
                }
            }

            return null;
        }

        /// <summary>
        /// Восстановление цепочки конвертации из parent словаря
        /// </summary>
        private List<ConvertElement> ReconstructChain(Dictionary<long, ConvertElement> parent, long fromCurr, long toCurr)
        {
            var chain = new List<ConvertElement>();
            var current = toCurr;

            while (current != fromCurr)
            {
                var step = parent[current];
                chain.Insert(0, step);
                current = step.IdFrom;
            }

            return chain;
        }


    }
}
