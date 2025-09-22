
using Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlanitPoker.Models.Entity
{
    public sealed class Room
    {
        public StoredRoom StoredRoom { get; set; }
        //private object LockObject { get; set; }
        //private ReaderWriterLock RWL { get; set; }
        private readonly SemaphoreSlim SemaphoreSlimLock;

        public Room(StoredRoom rm)
        {
            //LockObject = new object();
            //RWL = new ReaderWriterLock();
            StoredRoom = rm;
            SemaphoreSlimLock = new SemaphoreSlim(1);
        }

        public async Task<(T res, bool sc)> GetConcurentValue<T>(MultiThreadHelper multiThreadHelper, Func<Room, T> get)
        {
            //return multiThreadHelper.GetValue(this, get, this.RWL);
            //lock (LockObject)
            //{
            //    return (get(this),true);
            //}
            //var s = new SemaphoreSlim(1);

            return await multiThreadHelper.GetValue(this, get, this.SemaphoreSlimLock);
            //SemaphoreSlimLock.Wait();
            //var res = get(this);
            //SemaphoreSlimLock.Release();
            //return (res,true);


        }

        public async Task<T> GetConcurentValueAsync<T>(MultiThreadHelper multiThreadHelper, Func<Room, Task<T>> get)
        {
            return await multiThreadHelper.GetValueAsync(this, get, this.SemaphoreSlimLock);
        }

        public async Task<bool> SetConcurentValue(MultiThreadHelper multiThreadHelper, Action<Room> set)
        {
            // return multiThreadHelper.SetValue(this, rm =>
            //{
            //    set(this);
            //}, this.RWL);

            //lock (LockObject)
            //{
            //    set(this);
            //    return true;
            //}

            return await multiThreadHelper.SetValue(this, rm =>
                {
                    set(this);
                }, this.SemaphoreSlimLock);
            //SemaphoreSlimLock.Wait();
            //set(this);
            //SemaphoreSlimLock.Release();
            //return true;
        }

        public async Task<bool> SetConcurentValueAsync(MultiThreadHelper multiThreadHelper, Func<Room, Task> set)
        {
            //return await multiThreadHelper.SetValue(this,async rm =>
            //{
            //    await set(this);
            //}, this.RWL);

            //lock (LockObject)
            //{
            //    await set(this);
            //    return true;
            //}

            return await multiThreadHelper.SetValue(this, async rm =>
                {
                    await set(this);
                }, this.SemaphoreSlimLock);
            //SemaphoreSlimLock.Wait();
            //await set(this);
            //SemaphoreSlimLock.Release();
            //return true;
        }
    }
}
