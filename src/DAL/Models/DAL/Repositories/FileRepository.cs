using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{
    public class FileRepository: GeneralRepository<CustomFile, long>, IFileRepository
    {

        public FileRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) :base(db, repo)
        {
        }


        public override async Task<IEnumerable<CustomFile>> DeleteAsync(IEnumerable<CustomFile> records)
        {
            if (records != null)
            {
                foreach (var item in records)
                {
                    item.IsDeleted = true;
                    item.PhysFileShouldBeDeleted = true;
                }
            }

            return await base.UpdateAsync(records);
        }

        public override async Task<CustomFile> DeleteAsync(CustomFile record)
        {
            if (record != null)
            {
                record.IsDeleted = true;
                record.PhysFileShouldBeDeleted = true;
                return await base.UpdateAsync(record);
            }

            return record;
        }

        public override async Task<CustomFile> DeleteAsync(long recordId)
        {
            var record = await base.GetAsync(recordId);
            if (record != null)
            {
                record.IsDeleted = true;
                record.PhysFileShouldBeDeleted = true;
                return await base.UpdateAsync(record);
            }

            return record;
        }



        public async Task<List<long>> GetImagesIdsByArticleId(long articleId)
        {
           return await _db.Files.Where(x => x.ArticleId == articleId).Select(x => x.Id).ToListAsync();
        }

        public async Task<List<CustomFile>> GetNotActualFiles()
        {
            return await _db.Files.Where(x => x.IsDeleted == true && !x.PhysFileDeleted && x.PhysFileShouldBeDeleted).ToListAsync();
        }
    }
}
