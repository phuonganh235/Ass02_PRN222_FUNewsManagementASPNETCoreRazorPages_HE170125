using System;
namespace DataAccess.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string NewsArticleID { get; set; } = default!;
        public short AccountID { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }

        public NewsArticle? NewsArticle { get; set; }
        public SystemAccount? Account { get; set; }
    }
}