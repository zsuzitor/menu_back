using System.Text.Json.Serialization;
using Common.Models.Entity;

namespace Common.Models.Return
{

    public class BoolResultNewReturn
    {
        [JsonPropertyName("Result")]
        public bool Result { get; set; }

        public BoolResultNewReturn(BoolResult obj)
        {
            Result = obj.Result;
        }

        public BoolResultNewReturn(bool obj)
        {
            Result = obj;
        }
    }
}
