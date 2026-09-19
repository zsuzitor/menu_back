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

        public PortfolioService(IPortfolioRepository portfolioRepository, IStockRepository stockRepository, IStockElementRepository stockElementRepository, IStockEventRepository stockEventRepository)
        {
            _portfolioRepository = portfolioRepository;
            _stockRepository = stockRepository;
            _stockElementRepository = stockElementRepository;
            _stockEventRepository = stockEventRepository;
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
            //var currencyToConvert = currency.Select(x => new CurrencyConvertHandler.ConvertElement()
            //{
            //    IdFrom = x.Id,
            //    IdTo = x.CurrencyId,
            //    Price = x.LastPrice
            //}).ToList();

            if (!currency.Any(x => x.Id == req.CurrencyId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundCurrency);

            }

            //var stockIds = elements.Select(x => x.StockId).Distinct().ToList();
            var events = await _stockEventRepository.GetEvents(elementIds, req.Start, req.End);


            Dictionary<(long curId1, long curId2), List<ConvertElement>> pairHistory = converter.GetPairHistory(currency);




            // только пополнения
            var replenishments = events
                .Where(x => x.Type == StockEventEnum.CashReplenishment)
                .ToList();

            (result.CashReplenishmentSum, result.ReplenishmentsByCurrency) = GetMoneySumFromEvents(replenishments, req, elementById, pairHistory);

            var withdrawal = events
                .Where(x => x.Type == StockEventEnum.WithdrawalCash)
                .ToList();
            (result.WithdrawalCashSum, result.WithdrawalCashByCurrency) = GetMoneySumFromEvents(withdrawal, req, elementById, pairHistory);

            var dividends = events
                .Where(x => x.Type == StockEventEnum.Dividends)
                .ToList();
            (result.DividendsCashSum, result.DividendsCashByCurrency) = GetMoneySumFromEvents(dividends, req, elementById, pairHistory,false);



            //result.ReplenishmentsByCurrency = replenishmentsByCurrency;
            //result.CashReplenishmentSum = cashReplenishmentSum;

            return result;
            //foreach ( var element in elementById)
            //{
            //    var elementEvents = events.Where(x => x.MainElementId == element.Key || x.SubElementId == element.Key).ToList();
            //    var stockElement = stocks.FirstOrDefault(x => x.Id == element.Value.StockId);


            //    converter.FindClosestTimePoint();
            //}

            //var byCurrency = replenishments
            //   .GroupBy(ev =>
            //   {
            //       var el = elementById[ev.MainElementId];
            //       var stock = el.Stock;
            //       // если CurrencyId == null, сам Stock — валюта
            //       return stock.Id;
            //   });


            //events.Where(x => x.Type == StockEventEnum.CashReplenishment)
            //    .GroupBy(x=>x.MainElementId)
            //    .Select(x=>x)
            //    .Sum(x=>x.MainCountChange);//todo тут разные валюты могут быть, надо конвертить? или выводить по валютам? или и то и то


            //new CurrencyConvertHandler().ToCurrency(currency,)

        }


        

        private (decimal, Dictionary<long, decimal>) GetMoneySumFromEvents(List<StockEvent> events,
            PortfolioStatisticRequestDto req,
            Dictionary<long,StockElement> elementById,
             Dictionary<(long curId1, long curId2), List<ConvertElement>> pairHistory,
             bool moneyFromMainElement = true)
        {
            var converter = new CurrencyConvertHandler();
            decimal cashReplenishmentSum = 0;
            Dictionary<long, decimal> replenishmentsByCurrency = new Dictionary<long, decimal>();
            foreach (var rep in events)
            {

                var element = elementById[moneyFromMainElement?rep.MainElementId:rep.SubElementId.Value];
                if (!replenishmentsByCurrency.ContainsKey(element.StockId))
                    replenishmentsByCurrency.Add(element.StockId, 0);
                replenishmentsByCurrency[element.StockId] += moneyFromMainElement?rep.MainCountChange:rep.SubCountChange.Value;

                if (element.StockId == req.CurrencyId)
                {
                    //просто берем сумму так так нас эта валюта и интересует
                    cashReplenishmentSum += moneyFromMainElement ? rep.MainCountChange : rep.SubCountChange.Value;
                }
                else
                {
                    //надо сконвертить сумму на дату
                    //var stock = stocks.FirstOrDefault(x => x.Id == element.StockId);

                    //список элементов, по 1 записи на каждую пару с наиболее актуальным(по дате) значением
                    List<ConvertElement> forCurrencyDatePrice = new List<ConvertElement>();
                    //var simplePair = pairHistory.FirstOrDefault(x => (x.Key.curId1 == element.StockId && x.Key.curId2 == req.CurrencyId)
                    //|| (x.Key.curId1 == req.CurrencyId && x.Key.curId2 == element.StockId));
                    //if(pairHistory.ContainsKey((element.StockId, req.CurrencyId)) || pairHistory.ContainsKey((req.CurrencyId, element.StockId)))
                    //simplePair
                    pairHistory.TryGetValue((element.StockId, req.CurrencyId),out var simplePair1);
                    pairHistory.TryGetValue((req.CurrencyId, element.StockId), out var simplePair2);
                    if (simplePair1 != null)
                    {
                        var nearesHistory = converter.FindClosestTimePoint(simplePair1, rep.Date);
                        forCurrencyDatePrice.Add(nearesHistory);
                    }
                    else if (simplePair2 != null)
                    {
                        var nearesHistory = converter.FindClosestTimePoint(simplePair2, rep.Date);
                        forCurrencyDatePrice.Add(nearesHistory);
                    }
                    else
                    {
                        //если мы не нашли "прямую пару" то для всех пар ищем сумму на дату для того что бы рассчитать курс через другие валюты
                        foreach (var ph in pairHistory)
                        {
                            //для каждой пары ищем наиболее актуальную цену
                            var nearesHistory = converter.FindClosestTimePoint(ph.Value, rep.Date);
                            forCurrencyDatePrice.Add(nearesHistory);

                        }
                    }
                    //тут можно отсечь валюты которые напрямю не нужны, но тогда уйдет "продвинутый посчет цены" в ToCurrency когда через связку нескольких валют считается


                    cashReplenishmentSum += converter.ToCurrency(forCurrencyDatePrice, element.StockId, 
                        moneyFromMainElement ? rep.MainCountChange : rep.SubCountChange.Value, 
                        req.CurrencyId) ?? throw new SomeCustomException($"Не смогли сконвертировать валюту из {element.StockId} в {req.CurrencyId}");
                }
            }
            return (cashReplenishmentSum, replenishmentsByCurrency);
        }


    }
}
