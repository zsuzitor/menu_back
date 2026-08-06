using Google.Protobuf;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

Console.WriteLine("Hello, World!");

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
//No service for type 'Microsoft.Extensions.Configuration.IConfiguration' has been registered."

//var client = InvestApiClientFactory.Create("");
var cur = await client.Instruments.CurrenciesAsync();
var etf = await client.Instruments.EtfsAsync();
var shares = await client.Instruments.SharesAsync();
//GetAssetBy 

var c = cur.Instruments.Select(x=> new { TBankTicker=x.Ticker, x.Name, TBankFigi=x.Figi, TBankCurrency="rub", AppTicker=x.Ticker })
    //, x.Lot, x.Uid
    ;
var strJ = JsonSerializer.Serialize(c, options);


var priceRequest = new GetLastPricesRequest();
priceRequest.InstrumentId.AddRange(cur.Instruments.Select(x=>x.Figi));
priceRequest.LastPriceType = LastPriceType.LastPriceExchange;
priceRequest.InstrumentStatus = InstrumentStatus.Unspecified;
var prices = await client.MarketData.GetLastPricesAsync(priceRequest);
var  p = (decimal)prices.LastPrices[0].Price;
//var p2 = p * c.Lot;


var f = 10;
var g = 10;





