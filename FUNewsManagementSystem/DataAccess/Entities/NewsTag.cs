namespace DataAccess.Entities
{
    public class NewsTag
    {
        public int NewsId { get; set; }
        public NewsArticle? NewsArticle { get; set; }

        public int TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
