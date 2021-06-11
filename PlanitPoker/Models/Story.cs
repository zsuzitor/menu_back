using System;
using System.Collections.Generic;
using System.Text;

namespace PlanitPoker.Models
{
    public class Story
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Vote { get; set; }
        public DateTime Date { get; set; }
        public bool Completed { get; set; }

        public Story()
        {

        }


        public Story Clone()
        {
            var res = (Story)this.MemberwiseClone();
            return res;
        }
    }
}
