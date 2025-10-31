namespace DataAccess.Entities
{
    public class NewsArticle
    {
        public int NewsId { get; set; }
        public string NewsTitle { get; set; } = string.Empty;
        public string NewsContent { get; set; } = string.Empty;
        public string NewsSource { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string NewsStatus { get; set; } = "Draft"; // or "Published"

        // Khóa ngoại
        public short CreatedById { get; set; }
        public SystemAccount? CreatedBy { get; set; }

        // Liên kết Category
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Liên kết Tag (nhiều-nhiều)
        public ICollection<NewsTag>? NewsTags { get; set; }

        // Liên kết Comment
        public ICollection<Comment>? Comments { get; set; }
    }
}
