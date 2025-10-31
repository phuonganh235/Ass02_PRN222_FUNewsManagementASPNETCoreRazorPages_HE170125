namespace BusinessLogic.ViewModels
{
    public class NewsArticleVM
    {
        public int NewsArticleID { get; set; }
        public string? NewsTitle { get; set; }
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }

        public int CreatedByID { get; set; }
        public string? CreatedByName { get; set; }

        public int NewsStatus { get; set; }

        // Tag selection
        public List<int> SelectedTagIDs { get; set; } = new();
        public string SelectedTagNamesDisplay { get; set; } = "";
    }
}
