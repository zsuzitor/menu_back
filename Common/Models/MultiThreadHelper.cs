using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Models
{
    public class MultiThreadHelper
    {
        public (T1 res, bool success) GetValue<T1, T2>
            (T2 obj, Func<T2, T1> get, ReaderWriterLock rwl, int timeOut = 60000)
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
                    //if (rwl?.IsReaderLockHeld ?? false)
                    {
                        rwl?.ReleaseReaderLock();
                    }
                }
            }
            catch
            {
                return (default, false);
            }
        }



        public async Task<bool> SetValue<T1>
            (T1 obj, Func<T1, Task> set, ReaderWriterLock rwl, int timeOut = 60000)
        //where T1 : class
        {
            try
            {
                rwl?.AcquireWriterLock(timeOut);
                try
                {
                    await set(obj);
                    return true;
                }
                finally
                {
                    //if(rwl?.IsWriterLockHeld ?? false)
                    {
                        rwl?.ReleaseWriterLock();
                    }
                }
            }
            catch
            {
                //TODO конечно бы транзакцию хорошо бы
                return false;
            }
        }


        public bool SetValue<T1>
            (T1 obj, Action<T1> set, ReaderWriterLock rwl, int timeOut = 60000)
        //where T1 : class
        {
            try
            {
                rwl?.AcquireWriterLock(timeOut);
                try
                {
                    set(obj);
                    return true;
                }
                finally
                {
                    //if (rwl?.IsWriterLockHeld ?? false)
                    {
                        rwl?.ReleaseWriterLock();
                    }
                }
            }
            catch
            {
                //TODO конечно бы транзакцию хорошо бы
                return false;
            }
        }




        public (T1 res, bool success) GetValue<T1, T2>
            (T2 obj, Func<T2, T1> get, SemaphoreSlim smSl)
        //where T1 : class
        {
            try
            {
                try
                {
                    smSl.Wait();
                    return (get(obj), true);
                }
                finally
                {
                    smSl.Release();
                }
            }
            catch
            {
                return (default, false);
            }
        }


        public async Task<bool> SetValue<T1>
            (T1 obj, Func<T1, Task> set, SemaphoreSlim smSl)
        {
            try
            {
                try
                {
                    smSl.Wait();
                    await set(obj);
                    return true;
                }
                finally
                {
                    smSl.Release();
                }
            }
            catch
            {
                return false;
            }
        }


        public bool SetValue<T1>
            (T1 obj, Action<T1> set, SemaphoreSlim smSl)
        //where T1 : class
        {
            try
            {
                try
                {
                    smSl.Wait();
                    set(obj);
                    return true;
                }
                finally
                {
                    smSl.Release();
                }
            }
            catch
            {
                //TODO конечно бы транзакцию хорошо бы
                return false;
            }
        }
    }
}
