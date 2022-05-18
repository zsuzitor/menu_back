using Hangfire;
using System;
using Common.Models;
using System.Net.Http;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BL.Models.Services
{
    //Hangfire.AspNetCore
    public class Worker : IWorker
    {
        //static HttpClient client = new HttpClient();
        //private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private static object _locker = new object();
        public Worker()//IHttpClientFactory httpClientFactory)
        {
            //_httpClientFactory = httpClientFactory;
            _httpClient = new HttpClient();
        }

        public void Recurring(string recurringJobId, string cron, Action invoke)
        {
            //RecurringJob.AddOrUpdate(recurringJobId, () => invoke.Invoke(), () => cron);
            //RecurringJob.AddOrUpdate(recurringJobId, () => Invoke(), () => cron);
            throw new NotImplementedException();

        }

        public void Recurring(string recurringJobId, string cron, Expression<Func<Task>> invoke)
        {
            RecurringJob.AddOrUpdate(recurringJobId, invoke, () => cron);

        }


        public void Recurring(string recurringJobId, string cron, string url)
        {
            //Action httpInvoke = () =>
            //{
            //    lock (_locker)
            //    {
            //        //var httpClient = _httpClientFactory.CreateClient();
            //        //todo тут есть перегрузка на cancel token может тупо сразу отменять?
            //        _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            //    }
            //};
            RecurringJob.AddOrUpdate(recurringJobId, () => Send(url), () => cron);

        }

        public void Send(string url)
        {
            lock (_locker)
            {
                //var httpClient = _httpClientFactory.CreateClient();
                //todo тут есть перегрузка на cancel token может тупо сразу отменять?
                try
                {
                    _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
                }
                catch { }
            }
        }

        public void RemoveIfExists(string recurringJobId)
        {
            RecurringJob.RemoveIfExists(recurringJobId);
        }

        //public virtual string GetRecurringJobId()
        //{
        //    return "main_worker_id";
        //}


        public void Invoke()
        {
            //todo нужно нормально оформить и отправлять тут запрос
            var g = 10;
            //throw new System.Exception("123");
        }

    }
}
