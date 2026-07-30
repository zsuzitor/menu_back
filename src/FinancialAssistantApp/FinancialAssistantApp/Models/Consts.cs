

namespace FinancialAssistantApp.Models
{

    public static class Consts
    {
        public const string ProjectPrefix = "financial_assistant_";
        public static class ErrorConsts
        {

            public const string NotFoundPortfolio = $"{ProjectPrefix}not_found_portfolio";
            public const string NotFoundCurrency = $"{ProjectPrefix}not_found_currency";
            public const string NotFoundStock = $"{ProjectPrefix}not_found_stock";
            public const string NotValideStockEvent = $"{ProjectPrefix}not_valide_stock_event";
        }
    }
}
