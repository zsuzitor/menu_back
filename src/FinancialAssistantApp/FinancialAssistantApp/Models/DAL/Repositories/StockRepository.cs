using BO.Models.FinancialAssistant.DAL;
using Common.Models.Exceptions;
using DAL.Models.DAL;
using DAL.Models.DAL.Repositories;
using DAL.Models.DAL.Repositories.Interfaces;
using FinancialAssistantApp.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinancialAssistantApp.Models.DAL.Repositories
{
    public class StockRepository : GeneralRepository<Stock, long>, IStockRepository
    {
        public StockRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) : base(db, repo)
        {
        }

        public async Task<List<Stock>> FindAsync(string text, long? userId)
        {
            return await _db.Stock
                .Where(x =>
                (x.UserId == userId || x.IsGlobal)
                && (string.IsNullOrWhiteSpace(text) || EF.Functions.Like(x.Code, $"%{text}%")
                    || EF.Functions.Like(x.Name, $"%{text}%"))
                ).ToListAsync();
        }

        public async Task<List<Stock>> GetForUserAsync(long? userId)
        {
            return await _db.Stock
                .Where(x =>
                (x.UserId == userId || x.IsGlobal)).ToListAsync();
        }

        public async Task<List<Stock>> GetGlobalByCodesNoTrack(IEnumerable<string> codes)
        {
            return await _db.Stock.AsNoTracking().Where(x => x.IsGlobal && codes.Contains(x.Code)).ToListAsync();
        }

        public async Task<List<Stock>> GetGlobalAsync()
        {
            return await _db.Stock.AsNoTracking().Where(x => x.IsGlobal).ToListAsync();
        }

        public async Task<List<Stock>> GetGlobalForActualiztionAsync(DateTime date)
        {
            return await _db.Stock.AsNoTracking().Where(x => x.IsGlobal && x.ActualizationTime < date).ToListAsync();
        }

        public async Task<List<Stock>> GetCurrencyAsync(long? userId)
        {
            return await _db.Stock.AsNoTracking().Where(x =>
            (x.IsGlobal || x.UserId == userId) && x.Type == BO.Models.FinancialAssistant.Enums.StockTypeEnum.Currency
            ).ToListAsync();

        }

        public async Task<Stock> GetAsync(long id, long? userId)
        {
            return await _db.Stock.AsNoTracking().Where(x => x.Id == id &&
            (x.IsGlobal || x.UserId == userId)
            ).FirstOrDefaultAsync();

        }


        public async Task<Stock> GetCurrencyWithValidate(long? currencyId, long userId)
        {
            //todo вынести куда то в 1 место
            Stock currency = null;
            if (currencyId != null)
            {

                currency = await GetNoTrackAsync(currencyId.Value) ?? throw new SomeCustomBadRequestException(Consts.ErrorConsts.NotFoundStock);
                if (!currency.IsGlobal && currency.UserId != userId)
                {
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundStock);

                }
                if ((currency.Type != BO.Models.FinancialAssistant.Enums.StockTypeEnum.Currency))
                    throw new SomeCustomNotFoundException(Consts.ErrorConsts.NotFoundCurrency);
            }

            return currency;
        }

    }
}
