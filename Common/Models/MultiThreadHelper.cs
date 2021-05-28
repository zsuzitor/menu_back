using System;
using System.Threading;

namespace Common.Models
{
    public class MultiThreadHelper
    {
        public (T1 res, bool success) GetValue<T1, T2>
            (T2 obj, Func<T2, T1> get, ReaderWriterLock rwl, int timeOut = 100)
            //where T1 : class
        {
            try
            {
                rwl?.AcquireReaderLock(timeOut);
                try
                {
                    return (get(obj), true);
                }
                finally
                {
                    rwl?.ReleaseReaderLock();
                }
            }
            catch
            {
                return (default, false);
            }
        }
    }
}
