namespace DataAccess.Entities
{
    public class SystemAccount
    {
        public short AccountID { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountEmail { get; set; } = string.Empty;
        public int AccountRole { get; set; } // 1=Admin,2=Staff,3=Lecturer (ví dụ)
        public string AccountPassword { get; set; } = string.Empty;

        public ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
