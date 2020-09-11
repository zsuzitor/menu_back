using Menu.Models.Error;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Menu.Models.Returns
{
    public class OneErrorReturn
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }
        [JsonPropertyName("errors")]
        public List<string> Errors { get; set; }

        public OneErrorReturn(OneError obj)
        {
            Key = obj.Key;
            Errors = obj.Errors;
        }
    }
}
