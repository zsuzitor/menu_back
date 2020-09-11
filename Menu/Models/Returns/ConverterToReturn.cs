using System;
using System.Collections.Generic;
using System.Linq;

namespace Menu.Models.Returns
{
    public class ConverterToReturn//<T, V> where V : IConv<T, V>, new()
    {
        //public  object GetObjectReturn0(T obj) 
        //{
        //    if (obj is T objTyped)
        //    {
        //        return new V().Convert(objTyped);
        //    }

        //    if (obj is IEnumerable<T> objTypedList)
        //    {
        //        return objTypedList?.Select(x => new V().Convert(x)).ToList();
        //    }

        //    return obj;
        //}

        //public object GetObjectReturn1<T, V>(object obj) where V : IConv<T, V>, new()
        //{
        //    if (obj is T objTyped)
        //    {
        //        return new V().Convert(objTyped);
        //    }

        //    if (obj is IEnumerable<T> objTypedList)
        //    {
        //        return objTypedList?.Select(x => new V().Convert(x)).ToList();
        //    }

        //    return obj;
        //}
    }
}
