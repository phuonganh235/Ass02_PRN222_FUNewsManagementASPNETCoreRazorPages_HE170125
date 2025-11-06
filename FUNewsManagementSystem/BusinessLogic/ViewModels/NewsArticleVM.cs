using System;

namespace BusinessLogic.ViewModels
{
    public class NewsArticleVM
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public string NewsTitle { get; set; } = string.Empty;
        public string? Headline { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CategoryName { get; set; }
        public bool NewsStatus { get; set; }
        public List<int> SelectedTagIds { get; set; } = new();
        public string SelectedTagNamesDisplay { get; set; } = string.Empty;
    }
}
