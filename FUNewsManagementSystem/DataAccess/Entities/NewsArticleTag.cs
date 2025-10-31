namespace DataAccess.Entities
{
    public class NewsArticleTag
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public int TagID { get; set; }

        // Navigation properties
        public NewsArticle? NewsArticle { get; set; }
        public Tag? Tag { get; set; }
    }
}
