using BO.Models.MenuApp.DAL.Domain;



namespace BO.Models.DAL.Domain
{
    public sealed class CustomImage: IDomainRecord<long>
    {
        public long Id { get; set; }
        public string Path { get; set; }

        //public long? UserId { get; set; }
        //public User User { get; set; }

        public long? ArticleId { get; set; }//сейчас как каскад, при добавлении новых внешних ключей надо поаккуратнее мб сломается
        public Article Article { get; set; }
        public byte[] RowVersion { get; set; }



    }
}
