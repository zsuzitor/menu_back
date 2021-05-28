
using Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlanitPoker.Models
{
    public class Room
    {
        public StoredRoom StoredRoom { get; set; }
        public object Lock { get; set; }
        public ReaderWriterLock RWL = new ReaderWriterLock();

        public (T res, bool sc) GetConcurentValue<T>(MultiThreadHelper multiThreadHelper, Func<Room, T> get)
        {//TODO перетащить в репо
            return multiThreadHelper.GetValue(this, get, this.RWL);
        }

        public bool SetConcurentValue<T>(MultiThreadHelper multiThreadHelper, Action<Room> set)
        {//TODO перетащить в репо
            return multiThreadHelper.SetValue(this, rm =>
           {
               set(this);
           }, this.RWL);
        }
    }
}
