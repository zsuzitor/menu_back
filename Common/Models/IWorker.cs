
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Models
{
    public interface IWorker
    {
        /// <summary>
        /// T - тип который зарезолвится из DI
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="recurringJobId"></param>
        /// <param name="cron"></param>
        /// <param name="invoke"></param>
        void Recurring<T>(string recurringJobId, string cron, Expression<Action<T>> invoke);
        void Recurring(string recurringJobId, string cron, string url);
        void RemoveIfExists(string recurringJobId);
    }
}
