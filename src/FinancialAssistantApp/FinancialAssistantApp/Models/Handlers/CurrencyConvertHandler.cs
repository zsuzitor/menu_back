using BO.Models.FinancialAssistant.DAL;

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


        public ConvertElement FindClosestTimePoint(List<ConvertElement> timepoints, DateTime targetDate)
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
                    if (pairHistory.ContainsKey((history.StockId, history.CurrencyId.Value)))
                    {
                        pairHistory[(history.StockId, history.CurrencyId.Value)].Add(new ConvertElement()
                        { IdFrom = history.StockId, IdTo = history.CurrencyId.Value, DateOfPrice = history.Date, Price = history.Price });
                    }
                    else if (pairHistory.ContainsKey((history.CurrencyId.Value, history.StockId)))
                    {
                        pairHistory[(history.CurrencyId.Value, history.StockId)].Add(new ConvertElement()
                        { IdFrom = history.CurrencyId.Value, IdTo = history.StockId, DateOfPrice = history.Date, Price = 1m / history.Price });
                    }
                    else
                    {
                        pairHistory.Add((history.StockId, history.CurrencyId.Value), new List<ConvertElement>() {new ConvertElement()
                                { IdFrom = history.StockId, IdTo = history.CurrencyId.Value, DateOfPrice = history.Date, Price=history.Price } });
                    }
                    //if (forCurrencyDatePrice.Any(x=>x.IdFrom == cur.Id && x.IdTo == history.StockId))
                    //{

                    //}

                }
            }

            return pairHistory;
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
