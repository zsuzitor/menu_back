
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Common.Models
{
    public interface IWorker
    {
        void Recurring(string recurringJobId, string cron, Action invoke);
        void Recurring(string recurringJobId, string cron, Expression<Func<Task>> invoke);
        void Recurring(string recurringJobId, string cron, string url);
        void RemoveIfExists(string recurringJobId);
    }
}
