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



        public decimal? ToCurrency(List<Stock> currencys, long fromCurr, decimal fromSum, long toCurr)
        {
            // Если исходная и целевая валюта совпадают - возвращаем сумму
            if (fromCurr == toCurr)
                return fromSum;

            // Прямая конвертация
            var direct = currencys.FirstOrDefault(x => x.Id == fromCurr && x.CurrencyId == toCurr);
            if (direct != null)
            {
                return direct.LastPrice * fromSum;
            }

            // Обратная конвертация (если есть пара toCurr -> fromCurr)
            var reverse = currencys.FirstOrDefault(x => x.Id == toCurr && x.CurrencyId == fromCurr);
            if (reverse != null)
            {
                return fromSum / reverse.LastPrice;
            }

            // Если целевая валюта - "главная" (базовая), ищем курс fromCurr -> главная
            if (IsBaseCurrency(currencys, toCurr))
            {
                var toBase = currencys.FirstOrDefault(x => x.Id == fromCurr && x.CurrencyId == null);
                if (toBase != null)
                {
                    return toBase.LastPrice * fromSum;
                }
            }

            // Если исходная валюта - "главная" (базовая), ищем курс главная -> toCurr
            if (IsBaseCurrency(currencys, fromCurr))
            {
                var fromBase = currencys.FirstOrDefault(x => x.Id == toCurr && x.CurrencyId == null);
                if (fromBase != null)
                {
                    return fromSum / fromBase.LastPrice;
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
                    var rate = currencys.First(x => x.Id == currentCurrency &&
                                                    (x.CurrencyId == step.CurrencyId ||
                                                     (step.CurrencyId == null && x.CurrencyId == null)));
                    result *= rate.LastPrice;
                    currentCurrency = step.CurrencyId ?? 0; // Если null - считаем это базовой валютой
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
        private bool IsBaseCurrency(List<Stock> currencys, long currencyId)
        {
            // Если есть запись с таким Id и CurrencyId == null, значит это базовая валюта
            return currencys.Any(x => x.Id == currencyId && x.CurrencyId == null);
        }

        /// <summary>
        /// Поиск цепочки конвертации с помощью BFS (поиск в ширину)
        /// </summary>
        private List<Stock> FindConversionChain(List<Stock> currencys, long fromCurr, long toCurr)
        {
            // Строим граф: валюта -> список доступных курсов из этой валюты
            var graph = currencys
                .Where(x => x.CurrencyId != null || IsBaseCurrency(currencys, x.Id))
                .GroupBy(x => x.Id)
                .ToDictionary(g => g.Key, g => g.ToList());

            // BFS
            var queue = new Queue<long>();
            var visited = new HashSet<long> { fromCurr };
            var parent = new Dictionary<long, Stock>(); // для восстановления пути

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
                    if (edge.CurrencyId == null)
                    {
                        // Если CurrencyId == null, значит это базовая валюта
                        // Но мы не знаем ее Id, поэтому пропускаем (будет обработано отдельно)
                        continue;
                    }
                    else
                    {
                        next = edge.CurrencyId.Value;
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
        private List<Stock> FindChainThroughBaseCurrency(List<Stock> currencys, long fromCurr, long toCurr)
        {
            var baseCurrency = currencys.FirstOrDefault(x => x.CurrencyId == null);
            if (baseCurrency == null)
                return null;

            var chain = new List<Stock>();
            long baseId = baseCurrency.Id;

            // Проверяем путь fromCurr -> базовая валюта
            var toBase = currencys.FirstOrDefault(x => x.Id == fromCurr && x.CurrencyId == null);
            if (toBase != null)
            {
                chain.Add(toBase);

                // Проверяем путь базовая валюта -> toCurr (обратный курс)
                var fromBase = currencys.FirstOrDefault(x => x.Id == toCurr && x.CurrencyId == null);
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
        private List<Stock> ReconstructChain(Dictionary<long, Stock> parent, long fromCurr, long toCurr)
        {
            var chain = new List<Stock>();
            var current = toCurr;

            while (current != fromCurr)
            {
                var step = parent[current];
                chain.Insert(0, step);
                current = step.Id;
            }

            return chain;
        }


    }
}
