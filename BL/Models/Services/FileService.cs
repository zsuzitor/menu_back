using BL.Models.Services.Interfaces;
using System.IO;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    public class FileService: IFileService
    {
        public async Task<bool> Create(Stream readStream, string filePath)
        {
            if (readStream.CanRead)
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))//TODO будет ли тут исключение если файл существует?
                {
                    await readStream.CopyToAsync(fileStream);
                }
                return true;
            }

            return false;
        }

        public async Task<bool> DeletePhysicalFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return true;
            }

            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);//TODO если файл кто то читает будет ли ошибка?
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public string PathCombine(string path1, string path2)
        {
            return Path.Combine(path1, path2);
        }

        public string PathCombine(params string[] paths)
        {
            return Path.Combine(paths);
        }
    }
}
