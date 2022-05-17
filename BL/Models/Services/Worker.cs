using Hangfire;
using System;
using Common.Models;

namespace BL.Models.Services
{
    //Hangfire.AspNetCore
    public class Worker : IWorker
    {
        public void Recurring(string recurringJobId, string cron, Action invoke)
        {
            RecurringJob.AddOrUpdate(recurringJobId, () => invoke(), () => cron);
        }

        public void Recurring(string recurringJobId, string cron, string url)
        {
            throw;
        }

        public void RemoveIfExists(string recurringJobId)
        {
            RecurringJob.RemoveIfExists(recurringJobId);
        }

        //public virtual string GetRecurringJobId()
        //{
        //    return "main_worker_id";
        //}


        //public void Invoke()
        //{
        //    //todo нужно нормально оформить и отправлять тут запрос
        //    var g = 10;
        //    //throw new System.Exception("123");
        //}

    }
}
