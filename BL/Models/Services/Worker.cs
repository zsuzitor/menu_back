using BL.Models.Services.Interfaces;
using Hangfire;

namespace BL.Models.Services
{
    //todo это надо убрать из BL и выпилить ссылку на hangfire
    //Hangfire.AspNetCore
    public class Worker : IWorker
    {
        public void Recurring(string url, string cron)
        {
            RecurringJob.AddOrUpdate(()=> Invoke(), ()=>cron);
        }


        private void Invoke()
        {
            //todo нужно нормально оформить и отправлять тут запрос
        }

    }
}
