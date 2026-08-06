using Google.Protobuf;
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
            var mappedCollection = lst.Where(x => ticker.FirstOrDefault(y => y.Code == x.AppTicker) != null);
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

    }
}
