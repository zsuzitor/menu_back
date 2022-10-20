using System.Text.Json.Serialization;
using Common.Models.Poco;

namespace Common.Models.Return
{
    public sealed class StringResultFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            //if (obj == null)
            //{
            //    return null;
            //}

            //if (obj is BoolResult objTyped)
            //{
            //    return new StringResultReturn(objTyped);
            //}


            return obj;
        }
    }


    public class StringResultReturn
    {
        [JsonPropertyName("result")]
        public string Result { get; set; }

        //public StringResultReturn(BoolResult obj)
        //{
        //    Result = obj.Result;
        //}
        public StringResultReturn(string str)
        {
            Result = str;
        }
    }
}
