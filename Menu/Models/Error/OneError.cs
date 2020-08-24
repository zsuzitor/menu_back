

namespace Menu.Models.Error
{
    public class OneError
    {
        public string Key { get; set; }
        public string Body { get; set; }

        public OneError(string key,string body)
        {
            Key = key;
            Body = body;
        }
    }
}
