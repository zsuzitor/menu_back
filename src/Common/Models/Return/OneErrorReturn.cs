using System.Collections.Generic;
using System.Text.Json.Serialization;
using Common.Models.Error;

namespace Common.Models.Return
{
    public sealed class OneErrorReturn
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; }

        public OneErrorReturn(OneError obj)
        {
            Key = obj?.Key;
            Errors = obj?.Errors;
        }
    }
}
