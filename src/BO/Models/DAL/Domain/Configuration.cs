
using System;

namespace BO.Models.DAL.Domain
{
    public sealed class Configuration : IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public DateTime Start { get; set; }
        public DateTime? End { get; set; }
        public string Value { get; set; }
        public string Group { get; set; }
        public string Type { get; set; }
        public string Lang { get; set; }
        public bool Public { get; set; }
    }
}
