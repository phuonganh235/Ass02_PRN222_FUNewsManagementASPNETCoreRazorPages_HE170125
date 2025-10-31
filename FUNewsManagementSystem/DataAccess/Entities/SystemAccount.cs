namespace DataAccess.Entities
{
    public class SystemAccount
    {
        public int AccountID { get; set; }
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        // 3 = Admin, 2 = Staff, 1 = Lecturer
        public int AccountRole { get; set; }
        public string? AccountPassword { get; set; }

        // Navigation
        public ICollection<NewsArticle>? CreatedArticles { get; set; }
    }
}
