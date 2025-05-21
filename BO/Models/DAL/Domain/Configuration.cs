using System;
using System.Collections.Generic;
using System.Text;

namespace BO.Models.DAL.Domain
{
    public sealed class Configuration : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
        public string Type { get; set; }
        public string Lang { get; set; }
    }
}
