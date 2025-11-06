namespace BusinessLogic.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalArticles { get; set; }
        public int ActiveArticles { get; set; }
        public int InactiveArticles { get; set; }
        public int TotalAccounts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalTags { get; set; }
        public int TotalComments { get; set; }

        public Dictionary<string, int> ArticlesByCategory { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> ArticlesByAuthor { get; set; } = new Dictionary<string, int>();
        public List<NewsArticleViewModel> RecentArticles { get; set; } = new List<NewsArticleViewModel>();
    }
}