using Common.Models.Poco;
using Menu.Models.Returns.Interfaces;
using System.Text.Json.Serialization;

namespace Menu.Models.Returns.Types
{
    public class BoolResultFactory : IReturnObjectFactory
    {
        public object GetObjectReturn(object obj)
        {
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
