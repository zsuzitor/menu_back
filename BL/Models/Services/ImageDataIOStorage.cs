using BL.Models.Services.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    public class ImageDataIOStorage : IImageDataStorage
    {

        private readonly IFileService _fileService;
        private readonly dynamic _webHostEnvironment;

        public ImageDataIOStorage(IFileService fileService)
        {

            _fileService = fileService;
        }
        public async Task<string> Create(Stream readStream, string fileName)
        {
            if (readStream == null)
            {
                return null;
            }


            string filePath = _fileService.PathCombine(_webHostEnvironment.WebRootPath, "images", fileName);
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

        public async Task<string> CreateUpload(Stream readStream, string fileName)
        {
            if (readStream == null)
            {
                return null;
            }

            string resPath = _fileService.PathCombine("uploads", fileName);
            string filePath = _fileService.PathCombine(_webHostEnvironment.WebRootPath, "images", resPath);
            await _fileService.Create(readStream, filePath);
            return filePath;
        }

        public async Task<bool> Delete(string path)
        {
            return await _fileService.DeletePhysicalFile(path);
        }

        public Task Init()
        {
            //nothing todo
            return Task.CompletedTask;
        }
    }
}
