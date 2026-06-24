using BO.Models.DAL.Domain;

namespace Menu.Host.Models.Returns.Types
{
    public class ConfigurationReturn
    {

        public string Key { get; set; }
        public string Value { get; set; }

        public ConfigurationReturn(Configuration config)
        {
            Key = config?.Key;
            Value = config?.Value;
        }
    }
}
