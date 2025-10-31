using System;

namespace DataAccess.Entities
{
    public class Comment
    {
        public int CommentID { get; set; }
        public string NewsArticleID { get; set; } = string.Empty;
        public short AccountID { get; set; }

        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public NewsArticle? NewsArticle { get; set; }
        public SystemAccount? Account { get; set; }
    }
}
