using BO.Models.FinancialAssistant.DAL;
using Common.Models.Exceptions;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.Services.Interfaces;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.Services
{
    public class StockElementService : IStockElementService
    {
        private readonly IStockElementRepository _stockElementRepository;
        private readonly IPortfolioRepository _portfolioRepository;

        public StockElementService(IStockElementRepository stockElementRepository, IPortfolioRepository portfolioRepository)
        {
            _stockElementRepository = stockElementRepository;
            _portfolioRepository = portfolioRepository;
        }

        public async Task<List<StockElement>> Get(long portfolioId, long userId)
        {
            if(!await _portfolioRepository.ExistAsync(portfolioId, userId))
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }

            return await _stockElementRepository.Get(portfolioId);
        }
    }
}
