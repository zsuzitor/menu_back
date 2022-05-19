using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{
    public class ImageRepository: GeneralRepository<CustomImage, long>,IImageRepository
    {

        public ImageRepository(MenuDbContext db):base(db)
        {
        }


        public async Task<List<long>> GetImagesIdsByArticleId(long articleId)
        {
           return await _db.Images.Where(x => x.ArticleId == articleId).Select(x => x.Id).ToListAsync();
        }
    }
}
