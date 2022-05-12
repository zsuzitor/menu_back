
using System.Collections.Generic;

namespace BO.Models.Configs
{
    public class MailSendingConfig
    {
        public Dictionary<string, MailSendingInstanceConfig> Values { get; set; }
    }

    public class MailSendingInstanceConfig
    {
        public string NameFrom { get; set; }
        public string EmailFrom { get; set; }
        public string MailingHost { get; set; }
        public int MailingPort { get; set; }
        public string MailingLogin { get; set; }
        public string MailingPassword { get; set; }
    }
}
