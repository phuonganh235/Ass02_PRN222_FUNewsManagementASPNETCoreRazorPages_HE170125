namespace DataAccess.Entities
{
    public class NewsArticle
    {
        public int NewsArticleID { get; set; }
        public string? NewsTitle { get; set; }
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }

        public int CategoryID { get; set; }
        public Category? Category { get; set; }

        public int CreatedByID { get; set; }
        public SystemAccount? CreatedBy { get; set; }

        // 0 = Draft, 1 = Published, 2 = Archived (ví dụ)
        public int NewsStatus { get; set; }

        public ICollection<NewsArticleTag>? NewsArticleTags { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
