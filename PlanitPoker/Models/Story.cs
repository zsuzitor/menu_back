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

        public Story()
        {

        }
    }
}
