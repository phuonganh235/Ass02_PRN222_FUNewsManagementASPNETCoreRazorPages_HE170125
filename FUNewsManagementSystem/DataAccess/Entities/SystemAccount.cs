namespace DataAccess.Entities
{
    public class SystemAccount
    {
        public short AccountId { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountEmail { get; set; } = string.Empty;
        public string AccountPassword { get; set; } = string.Empty;
        public string AccountRole { get; set; } = "Staff"; // "Admin", "Lecturer", "Staff"
        public bool IsActive { get; set; } = true;

        public ICollection<NewsArticle>? CreatedArticles { get; set; }
    }
}
