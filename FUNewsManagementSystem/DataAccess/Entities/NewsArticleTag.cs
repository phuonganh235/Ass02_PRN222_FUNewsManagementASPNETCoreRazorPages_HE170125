namespace DataAccess.Entities
{
    public class NewsArticleTag
    {
        public int NewsArticleID { get; set; }
        public NewsArticle? NewsArticle { get; set; }

        public int TagID { get; set; }
        public Tag? Tag { get; set; }
    }
}
