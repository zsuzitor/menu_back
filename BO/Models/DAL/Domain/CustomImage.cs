using BO.Models.MenuApp.DAL.Domain;



namespace BO.Models.DAL.Domain
{
    public class CustomImage: IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Path { get; set; }

        //public long? UserId { get; set; }
        //public User User { get; set; }

        public long? ArticleId { get; set; }
        public Article Article { get; set; }


    }
}
