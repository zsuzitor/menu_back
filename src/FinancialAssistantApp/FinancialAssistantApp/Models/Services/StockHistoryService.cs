using BL.Models.Services.Interfaces;
using BO.Models.FinancialAssistant.DAL;
using BO.Models.FinancialAssistant.Enums;
using FinancialAssistantApp.Models.DAL.Repositories;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.Mapper;
using FinancialAssistantApp.Models.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using TIntegration.Models.Services.Interfaces;

namespace FinancialAssistantApp.Models.Services
{
    public class StockHistoryService : IStockHistoryService
    {
        private readonly IStockHistoryRepository _stockHistoryRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IPriceService _priceService;

        public StockHistoryService(IStockHistoryRepository stockHistoryRepository, IStockRepository stockRepository, IDateTimeProvider dateTimeProvider, IPriceService priceService)
        {
            _stockHistoryRepository = stockHistoryRepository;
            _stockRepository = stockRepository;
            _dateTimeProvider = dateTimeProvider;
            _priceService = priceService;
        }

        public async Task UpdateGlobal()
        {
            var notActual = await _stockRepository.GetGlobalForActualiztionAsync(_dateTimeProvider.CurrentDateTime().AddHours(6));
            var tRequests = notActual.Where(x => x.Type != StockTypeEnum.Other).Select(x => x.ToTInvestRequest()).ToList();
            var tPrices = await _priceService.GetPrice(tRequests);
            var history = new List<StockHistory>();
            //достаем из бд вторым запросом что бы засунуть это в транзакцию потом, а запрос с получением цен вынести из транзакции
            var forUpdate = await _stockRepository.GetAsync(notActual.Select(x => x.Id).ToList());
            var tCurrency = tPrices.Select(x => x.CurrencyCode).Distinct();
            var appCurrency = await _stockRepository.GetGlobalByCodesNoTrack(tCurrency);
            foreach (var stock in forUpdate)
            {
                var newVal = tPrices.FirstOrDefault(x => x.Code == stock.Code);
                if (newVal == null)
                    continue;

                var curr = appCurrency.FirstOrDefault(x => x.Code == newVal.CurrencyCode);
                stock.LastPrice = newVal.Price;
                stock.CurrencyId = curr.Id;//todo у валюты есть это поле? у всей валюты? есть какая то главная валюта?
                stock.ActualizationTime = _dateTimeProvider.CurrentDateTime();
                //todo CurrencyId

                history.Add(new StockHistory()
                {
                    CurrencyId = curr.Id,
                    Date = _dateTimeProvider.CurrentDateTime(),
                    Price = newVal.Price,
                    StockId = stock.Id,
                });

            }

            await _stockRepository.UpdateAsync(forUpdate);
            await _stockHistoryRepository.AddAsync(history);
        }
    }
}
