namespace DataAccess.Entities
{
    public class NewsTag
    {
        public string NewsArticleID { get; set; } = "";
        public int TagID { get; set; }

        public NewsArticle? NewsArticle { get; set; }
        public Tag? Tag { get; set; }
    }
}
