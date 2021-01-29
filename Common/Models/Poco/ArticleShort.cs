

using Common.Models.DAL.Domain;

namespace Common.Models.Poco
{
    public class ArticleShort
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string MainImagePath { get; set; }

        public bool Followed { get; set; }

        public ArticleShort()
        {

        }

        public ArticleShort(Article article)
        {
            Id = article.Id;
            Title = article.Title;
            Body = article.Body;
            MainImagePath = article.MainImagePath;
            Followed = article.Followed;
        }
    }
}
