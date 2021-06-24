
using Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlanitPoker.Models
{
    public class Room
    {
        public StoredRoom StoredRoom { get; set; }
        public object LockObject { get; set; }
        public ReaderWriterLock RWL { get; set; }

        public Room(StoredRoom rm)
        {
            LockObject = new object();
            RWL = new ReaderWriterLock();
            StoredRoom = rm;
        }

        public (T res, bool sc) GetConcurentValue<T>(MultiThreadHelper multiThreadHelper, Func<Room, T> get)
        {
            return multiThreadHelper.GetValue(this, get, this.RWL);
            //lock (LockObject)
            //{
            //    return (get(this),true);
            //}
            //var s = new SemaphoreSlim(1);
            
        }

        public bool SetConcurentValue<T>(MultiThreadHelper multiThreadHelper, Action<Room> set)
        {
            return multiThreadHelper.SetValue(this, rm =>
           {
               set(this);
           }, this.RWL);

            //lock (LockObject)
            //{
            //    set(this);
            //    return true;
            //}
        }

        public async Task<bool> SetConcurentValueAsync<T>(MultiThreadHelper multiThreadHelper, Func<Room, Task> set)
        {
            return await multiThreadHelper.SetValue(this,async rm =>
            {
                await set(this);
            }, this.RWL);

            //lock (LockObject)
            //{
            //    await set(this);
            //    return true;
            //}
        }
    }
}
