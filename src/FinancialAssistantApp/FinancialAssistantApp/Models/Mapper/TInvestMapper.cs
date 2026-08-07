using BO.Models.FinancialAssistant.DAL;
using TEnum = TIntegration.Models.Enums;
using  TIntegration.Models.DTO;
using AppEnum = BO.Models.FinancialAssistant.Enums;

namespace FinancialAssistantApp.Models.Mapper
{
    public static class TInvestMapper
    {
        public static PriceRequestDto ToTInvestRequest(this Stock stock)
        {
            return new PriceRequestDto()
            {
                Code = stock.Code,
                Type = ToStockTypeEnum(stock.Type)
            };
        }


        public static TEnum.StockTypeEnum ToStockTypeEnum(AppEnum.StockTypeEnum val)
        {
            switch (val)
            {
                case AppEnum.StockTypeEnum.InvestmentFund:
                    return TEnum.StockTypeEnum.InvestmentFund;
                case AppEnum.StockTypeEnum.InvestmentStock:
                    return TEnum.StockTypeEnum.InvestmentStock;
                case AppEnum.StockTypeEnum.InvestmentBond:
                    return TEnum.StockTypeEnum.InvestmentBond;
                case AppEnum.StockTypeEnum.Currency:
                    return TEnum.StockTypeEnum.Currency;
            }

            return TEnum.StockTypeEnum.Currency;
        }

    }
}
