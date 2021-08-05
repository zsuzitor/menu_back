using System.Text.Json.Serialization;
using Common.Models.Poco;

namespace Common.Models.Return
{
    public class BoolResultFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj is BoolResult objTyped)
            {
                return new BoolResultReturn(objTyped);
            }


            return obj;
        }
    }


    public class BoolResultReturn
    {
        [JsonPropertyName("result")]
        public bool Result { get; set; }

        public BoolResultReturn(BoolResult obj)
        {
            Result = obj.Result;
        }
    }
}
