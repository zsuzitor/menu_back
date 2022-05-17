
using System;

namespace Common.Models
{
    public interface IWorker
    {
        void Recurring(string recurringJobId, string cron, Action invoke);
        void Recurring(string recurringJobId, string cron, string url);
        void RemoveIfExists(string recurringJobId);
    }
}
