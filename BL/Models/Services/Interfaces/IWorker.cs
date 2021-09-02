
namespace BL.Models.Services.Interfaces
{
    public interface IWorker
    {
        void Recurring(string url, string cron);
    }
}
