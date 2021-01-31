using BO.Models.DAL.Domain;
using DAL.Models.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{
    public class ImageRepository: IImageRepository
    {
        private readonly MenuDbContext _db;

        public async Task<CustomImage> Add(CustomImage newImage)
        {
            _db.Images.Add(newImage);
            await _db.SaveChangesAsync();
            return newImage;
        }

        public async Task<List<CustomImage>> Add(List<CustomImage> newImages)
        {
            _db.Images.AddRange(newImages);
            await _db.SaveChangesAsync();
            return newImages;
        }

        public async Task<List<CustomImage>> Delete(List<CustomImage> images)
        {
            _db.Images.AttachRange(images);

            _db.RemoveRange(images);
            await _db.SaveChangesAsync();
            return images;
        }

        public async Task<CustomImage> Get(long id)
        {
            return await _db.Images.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CustomImage>> Get(List<long> ids)
        {
            return await _db.Images.Where(x => ids.Contains(x.Id)).ToListAsync(); 
        }

        public async Task<List<long>> GetImagesIdsByArticleId(long articleId)
        {
           return await _db.Images.Where(x => x.ArticleId == articleId).Select(x => x.Id).ToListAsync();
        }
    }
}
