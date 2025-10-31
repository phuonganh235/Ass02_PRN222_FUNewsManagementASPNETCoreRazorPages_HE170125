namespace BusinessLogic.ViewModels
{
    public class NewsArticleVM
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public string NewsTitle { get; set; } = string.Empty;
        public string Headline { get; set; } = string.Empty;
        public string NewsContent { get; set; } = string.Empty;
        public string NewsSource { get; set; } = string.Empty;

        public short CategoryID { get; set; }
        public string? CategoryName { get; set; }

        public short CreatedByID { get; set; }
        public string? CreatedByName { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool NewsStatus { get; set; }

        public List<int> SelectedTagIDs { get; set; } = new();
        public List<string> SelectedTagNames { get; set; } = new();

        public string SelectedTagNamesDisplay =>
            SelectedTagNames != null && SelectedTagNames.Count > 0
                ? string.Join(", ", SelectedTagNames)
                : string.Empty;
    }
}
