using BO.Models.FinancialAssistant.DAL;
using Common.Models.Exceptions;
using FinancialAssistantApp.Models.Services.Interfaces;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;

        public PortfolioService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }




        public async Task<Portfolio> CreateAsync(Portfolio obj, long userId)
        {
            var rec = new Portfolio();
            rec.Name = obj.Name;
            rec.UserId = userId;
            return await _portfolioRepository.AddAsync(rec);
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
    }
}
