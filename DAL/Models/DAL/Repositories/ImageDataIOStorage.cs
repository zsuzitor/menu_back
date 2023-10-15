using BO.Models.Configs;
using DAL.Models.DAL.Repositories.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories
{
    public class ImageDataIOStorage : IImageDataStorage
    {

        private readonly IFileService _fileService;
        //private readonly IWebHostEnvironment _webHostEnvironment;//не хочу тут на это ссылаться, поэтому просто static строкой. _webHostEnvironment.WebRootPath
        private static string WebRootPath;

        public ImageDataIOStorage(IFileService fileService, ImageConfig settings)
        {
            _fileService = fileService;
            WebRootPath = settings.PhysicalPath;
        }

        public static void Init(string webRootPath)
        {
            WebRootPath = webRootPath;
        }

        public async Task<string> CreateAsync(Stream readStream, string fileName)
        {
            if (readStream == null)
            {
                return null;
            }


            string filePath = _fileService.PathCombine(WebRootPath, "images", fileName);
            //using (MemoryStream memStream = new MemoryStream((int)image.Length))//todo ing??
            //{
            //    image.OpenReadStream();
            //}
            //    using (var fileStream = new FileStream(filePath, FileMode.Create))//TODO будет ли тут исключение если файл существует?
            //{
            //    await image.CopyToAsync(fileStream);
            //    image.len
            //}

            await _fileService.Create(readStream, filePath);
            return filePath;
        }

        public async Task<string> CreateUploadAsync(Stream readStream, string fileName)
        {
            if (readStream == null)
            {
                return null;
            }

            string resPath = _fileService.PathCombine("images", "uploads", fileName);
            string filePath = _fileService.PathCombine(WebRootPath, resPath);
            await _fileService.Create(readStream, filePath);
            return "\\" + resPath;
        }

        public async Task<bool> DeleteAsync(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return true;
            }

            if (path.StartsWith("\\"))
            {
                path = path[1..];
            }

            return await _fileService.DeletePhysicalFile(_fileService.PathCombine(WebRootPath, path));
        }

        public Task Init()
        {
            //nothing todo
            return Task.CompletedTask;
        }
    }
}
