namespace DataAccess.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int NewsArticleID { get; set; }
        public int AccountID { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public NewsArticle? NewsArticle { get; set; }
        public SystemAccount? Account { get; set; }
    }
}
