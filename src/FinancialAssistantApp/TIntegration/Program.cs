using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

Console.WriteLine("Hello, World!");


//!!!!!!
//https://developer.tbank.ru/invest/intro/developer/error-codes/errors/ коды ошибок


var options = new JsonSerializerOptions
{
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    WriteIndented = true // для красивого форматирования (опционально)
};



var services = new ServiceCollection();
var configuration = new ConfigurationBuilder()
    //.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettingsSecret.json", optional: false, reloadOnChange: true)
    //.AddEnvironmentVariables()
    .Build();


services.AddSingleton<IConfiguration>(configuration);
var token = configuration.GetValue<string>("AuthToken");

services.AddInvestApiClient((_, settings) => settings.AccessToken = token);
//services.AddInvestApiClient((_, settings) => context.Configuration.Bind(settings));
var provider = services.BuildServiceProvider();


//var figt = GetFigi(configuration, "KGSRUB_TOM");




var client = provider.GetRequiredService<InvestApiClient>();

//var client = InvestApiClientFactory.Create("");
var cur = await client.Instruments.CurrenciesAsync();
var etf = await client.Instruments.EtfsAsync();
var shares = await client.Instruments.SharesAsync();
//var shares = await client.Instruments.BondsAsync();
//var shares = await client.Instruments.DfasAsync();
//var shares = await client.Instruments.();

var c = cur.Instruments.Select(x=> new { TBankTicker=x.Ticker, x.Name, TBankFigi=x.Figi, TBankCurrency="rub", AppTicker=x.Ticker })

    //, x.Lot, x.Uid
    ;
var strJ = JsonSerializer.Serialize(c, options);

var etfShort = etf.Instruments.Select(x => new { TBankTicker = x.Ticker, x.Name, TBankFigi = x.Figi, TBankCurrency = "rub", AppTicker = x.Ticker });
var strEtf = JsonSerializer.Serialize(etfShort, options);

var etfshares = shares.Instruments.Select(x => new { TBankTicker = x.Ticker, x.Name, TBankFigi = x.Figi, TBankCurrency = "rub", AppTicker = x.Ticker });
var strShare = JsonSerializer.Serialize(etfshares, options);


//var p2 = p * c.Lot;


var candleRequest = new GetCandlesRequest() {CandleSourceType= GetCandlesRequest.Types.CandleSource.Exchange,InstrumentId= "BBG0013HGFT4",
    Interval= CandleInterval.Day,From=DateTime.UtcNow.AddYears(-5).ToTimestamp(), To = DateTime.UtcNow.ToTimestamp()
};
var candles = await client.MarketData.GetCandlesAsync(candleRequest);
var strC = JsonSerializer.Serialize(candles, options);


var f = 10;
var g = 10;


 void GetPrice() {

    var priceRequest = new GetLastPricesRequest();
    priceRequest.InstrumentId.AddRange(cur.Instruments.Select(x => x.Figi));
    priceRequest.LastPriceType = LastPriceType.LastPriceExchange;
    priceRequest.InstrumentStatus = InstrumentStatus.Unspecified;
    var prices =  client.MarketData.GetLastPricesAsync(priceRequest).GetAwaiter().GetResult();
    var p = (decimal)prices.LastPrices[0].Price;
}


