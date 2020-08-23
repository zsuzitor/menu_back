


namespace Menu.Models.DAL.Domain
{
    public class Article
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string MainImagePath { get; set; }


        public bool Followed { get; set; }


        public int UserId { get; set; }
        public User User { get; set; }

    }
}
