using BO.Models.DAL.Domain;
using System.Threading.Tasks;

namespace BL.Models.Services.Interfaces
{
    public interface IConfigurationService
    {
        Task AddIfNotExistAsync(string key, string value, string group, string type, bool isPublic = false);
        Task<Configuration> GetAsync(string key);
        Task<Configuration> GetPublicAsync(string key);
    }
}
