
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Menu.Models.Healpers.Interfaces
{
    public interface IApiHealper
    {
        Task WriteResponse<T>(HttpResponse response, T data);
    }
}
