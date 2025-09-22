
using System.IO;
using System.Threading.Tasks;

namespace DAL.Models.DAL.Repositories.Interfaces
{
    public interface IFileService
    {
        Task<bool> DeletePhysicalFile(string path);
        string PathCombine(string path1, string path2);
        string PathCombine(params string[] paths);

        Task<bool> Create(Stream readStream, string filePath);
    }
}
