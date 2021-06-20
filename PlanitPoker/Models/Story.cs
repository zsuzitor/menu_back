using BO.Models.PlaningPoker.DAL;
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


        public PlaningStoryDal ToDbObject(long roomId)
        {
            var forDb = new PlaningStoryDal();
            forDb.Completed = Completed;
            forDb.Date = Date;
            forDb.Description= Description;
            forDb.Name= Name;
            forDb.RoomId= roomId;
            forDb.Vote= Vote;
            return forDb;
        }

        public void FromDbObject(PlaningStoryDal obj)
        {
            Completed = obj.Completed;
            Date = obj.Date;
            Description = obj.Description;
            Id = obj.Id;
            Name = obj.Name;
            Vote = obj.Vote;

        }
    }
}
