using BO.Models.FinancialAssistant.DAL;
using Common.Models.Exceptions;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DTO;
using FinancialAssistantApp.Models.Services.Interfaces;
using TaskManagementApp.Models.DAL.Repositories.Interfaces;

namespace FinancialAssistantApp.Models.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly IStockRepository _stockRepository;

        public PortfolioService(IPortfolioRepository portfolioRepository)
        {
            _portfolioRepository = portfolioRepository;
        }




        public async Task<Portfolio> CreateAsync(PortfolioCreate obj, long userId)
        {
            if (obj.CurrencyId != null)
            {
                var currency = await _stockRepository.GetNoTrackAsync(obj.CurrencyId.Value) ?? throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
                if (!currency.IsGlobal)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);

                }
            }

            var rec = new Portfolio
            {
                Name = obj.Name,
                UserId = userId,
                CurrencyId = obj.CurrencyId
            };
            return await _portfolioRepository.AddAsync(rec);
        }

        public async Task<Portfolio> UpdateAsync(PortfolioCreate obj, long userId)
        {
            var rec = await _portfolioRepository.GetAsync(obj.Id, userId);
            if (rec?.UserId != userId)
            {
                throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundPortfolio);
            }

            rec.Name = obj.Name;
            rec.CurrencyId = obj.CurrencyId;
            return await _portfolioRepository.UpdateAsync(rec);

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
