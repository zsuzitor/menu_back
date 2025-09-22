
using System.IO;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface IImageDataStorage
    {
        Task Init();
        Task<bool> DeleteAsync(string path);
        Task<string> CreateAsync(Stream readStream, string fileName);
        Task<string> CreateUploadAsync(Stream readStream, string fileName);
    }
}
