using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;
using TIntegration.Models.DTO;
using TIntegration.Models.Services.Interfaces;

namespace TIntegration.Models.Services
{
    public class PriceService : IPriceService
    {
        private readonly InvestApiClient _investApiClient;
        private readonly IConfiguration _configuration;
        public PriceService(InvestApiClient investApiClient, IConfiguration configuration)
        {
            _investApiClient = investApiClient;
            _configuration = configuration;
        }

        public async Task<List<HistoryResponseElementDto>> GetHistory(HistoryRequestDto ticker)
        {
            var result = new List<HistoryResponseElementDto>();
            var lst = new List<MapTElement>();
            _configuration.GetSection("FinancialAssistantApp:TBankMapping").Bind(lst);
            var obgConfig = lst.FirstOrDefault(x => x.AppTicker == ticker.Code);
            if (obgConfig == null)
            {
                return result;
            }
            var candleRequest = new GetCandlesRequest()
            {
                CandleSourceType = GetCandlesRequest.Types.CandleSource.Exchange,
                InstrumentId = obgConfig.TBankFigi,
                Interval = CandleInterval.Day,
                From = ticker.Start.ToUniversalTime().ToTimestamp(),
                To = ticker.End.ToUniversalTime().ToTimestamp(),
            };
            var candles = await _investApiClient.MarketData.GetCandlesAsync(candleRequest);
            return candles.Candles.Select(c => new HistoryResponseElementDto()
            {
                Date = c.Time.ToDateTime(),
                Price = TInvestConverter.ToDecimal(c.High.Units, c.High.Nano),
                CurrencyCode = obgConfig.TBankCurrency,
            }).ToList();

        }

        public async Task<PriceResponseDto> GetPrice(PriceRequestDto ticker)
        {
            return (await GetPrice(new List<PriceRequestDto>() { ticker })).FirstOrDefault();
        }

        public async Task<List<PriceResponseDto>> GetPrice(List<PriceRequestDto> ticker)
        {
            //var grouped = ticker.GroupBy(x => x.Type);
            //foreach(var g in grouped)
            //{

            //}

            //var figi = new Dictionary

            var lst = new List<MapTElement>();
            _configuration.GetSection("FinancialAssistantApp:TBankMapping").Bind(lst);
            var mappedCollection = lst.Where(x => ticker.FirstOrDefault(y => y.Code == x.AppTicker) != null);// join?
            if (mappedCollection.Count() == 0)
            {
                return Enumerable.Empty<PriceResponseDto>().ToList();
            }

            var dictionary = mappedCollection.ToDictionary(x=>x.TBankFigi);


            var priceRequest = new GetLastPricesRequest();
            priceRequest.InstrumentId.AddRange(mappedCollection.Select(x=>x.TBankFigi));
            priceRequest.LastPriceType = LastPriceType.LastPriceExchange;
            priceRequest.InstrumentStatus = InstrumentStatus.Unspecified;
            var prices = await _investApiClient.MarketData.GetLastPricesAsync(priceRequest);
            var res = new List<PriceResponseDto>();
            foreach (var pr in prices.LastPrices)
            {
                var d = dictionary[pr.Figi];
                var code = d.AppTicker;
                res.Add(new PriceResponseDto()
                {
                    Code = code,
                    CurrencyCode = d.TBankCurrency,
                    Price = pr.Price,
                });
            }

            return res;
            //var p = (decimal)prices.LastPrices[0].Price;
        }



        public class TInvestConverter
        {
            private const decimal NanoFactor = 1_000_000_000m;

            /// <summary>
            /// Конвертирует units и nano из API в обычное число decimal
            /// </summary>
            public static decimal ToDecimal(long units, int nano)
            {
                // Делим nano на 1 000 000 000 и прибавляем к целой части
                return units + (nano / NanoFactor);
            }

            /// <summary>
            /// Конвертирует decimal число в формат API (units, nano)
            /// </summary>
            public static (long units, int nano) ToQuotation(decimal value)
            {
                // Получаем целую часть (с отсечением дробной)
                long units = (long)Math.Truncate(value);

                // Получаем дробную часть и умножаем на 10^9
                int nano = (int)((value - units) * NanoFactor);

                return (units, nano);
            }
        }


    }
}
