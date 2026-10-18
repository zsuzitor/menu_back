using AutoFixture;
using FinancialAssistantApp.Models.Handlers;
using FluentAssertions;
using static FinancialAssistantApp.Models.Handlers.CurrencyConvertHandler;

namespace FinancialAssistantApp.Tests.Handlers
{
    public class CurrencyConvertHandlerTests
    {

        private readonly IFixture _fixture;

        public CurrencyConvertHandlerTests()
        {
            _fixture = new Fixture();
        }



        [Fact]
        public async Task CurrencyConvert_3currencyPath_Success()
        {
            var handler = new CurrencyConvertHandler();
            var rubId = 1;
            var uanId = 2;
            var dollarId = 3;

            var cur = new List<ConvertElement>()
            {
                new ConvertElement(){IdFrom=rubId,IdTo=uanId,Price=0.1m },
                new ConvertElement(){IdFrom=uanId,IdTo=dollarId,Price=0.1m },
            };


            var result = handler.ToCurrency(cur, rubId, 100, dollarId);
            result.Value.Should().Be(1);
        }
    }
}
