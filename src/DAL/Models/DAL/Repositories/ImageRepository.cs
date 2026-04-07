using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{
    public class ImageRepository: GeneralRepository<CustomImage, long>, IImageRepository
    {

        public ImageRepository(MenuDbContext db, IGeneralRepositoryStrategy repo) :base(db, repo)
        {
        }


        public override async Task<IEnumerable<CustomImage>> DeleteAsync(IEnumerable<CustomImage> records)
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

        public override async Task<CustomImage> DeleteAsync(CustomImage record)
        {
            if (record != null)
            {
                record.IsDeleted = true;
                record.PhysFileShouldBeDeleted = true;
                return await base.UpdateAsync(record);
            }

            return record;
        }

        public override async Task<CustomImage> DeleteAsync(long recordId)
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
           return await _db.Images.Where(x => x.ArticleId == articleId).Select(x => x.Id).ToListAsync();
        }

        public async Task<List<CustomImage>> GetNotActualFiles()
        {
            return await _db.Images.Where(x => x.IsDeleted == true && !x.PhysFileDeleted && x.PhysFileShouldBeDeleted).ToListAsync();
        }
    }
}
