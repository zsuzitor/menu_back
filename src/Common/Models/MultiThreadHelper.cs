using System;
using System.Threading;
using System.Threading.Tasks;

namespace Common.Models
{
    public sealed class MultiThreadHelper
    {
        [Obsolete("ReaderWriterLock - не работает с потоками")]
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


        [Obsolete("ReaderWriterLock - не работает с потоками")]
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


        [Obsolete("ReaderWriterLock - не работает с потоками")]
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




        public async Task<(T1 res, bool success)> GetValue<T1, T2>
            (T2 obj, Func<T2, T1> get, SemaphoreSlim smSl)
        //where T1 : class
        {
            try
            {
                try
                {
                    await smSl.WaitAsync();
                    return (get(obj), true);
                }
                finally
                {
                    smSl.Release();
                }
            }
            catch (Exception)
            {
                //if (e is IBaseCusomException)
                {
                    throw;
                }

                //return (default, false);
            }
        }

        public async Task<T1> GetValueAsync<T1, T2>
            (T2 obj, Func<T2, Task<T1>> get, SemaphoreSlim smSl)
        //where T1 : class
        {
            try
            {
                try
                {
                    await smSl.WaitAsync();
                    return await get(obj);
                }
                finally
                {
                    smSl.Release();
                }
            }
            catch (Exception)
            {
                //if (e is IBaseCusomException)
                {
                    throw;
                }

                //return (default, false);
            }
        }

        public async Task<bool> SetValue<T1>
            (T1 obj, Func<T1, Task> set, SemaphoreSlim smSl)
        {
            try
            {
                try
                {
                    await smSl.WaitAsync();
                    await set(obj);
                    return true;
                }
                finally
                {
                    smSl.Release();
                }
            }
            catch (Exception)
            {
                //if (e is IBaseCusomException)
                {
                    throw;
                }

                //return false;
            }
        }


        public async Task<bool> SetValue<T1>
            (T1 obj, Action<T1> set, SemaphoreSlim smSl)
        //where T1 : class
        {
            try
            {
                try
                {
                    await smSl.WaitAsync();
                    set(obj);
                    return true;
                }
                finally
                {
                    smSl.Release();
                }
            }
            catch (Exception)
            {
                //if (e is IBaseCusomException)
                {
                    throw;
                }
                //возможно тут всегда надо бросать исключение

                //TODO конечно бы транзакцию хорошо бы
                //return false;
            }
        }
    }
}
