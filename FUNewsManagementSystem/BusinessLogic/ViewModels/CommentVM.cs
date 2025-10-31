namespace BusinessLogic.ViewModels
{
    public class CommentVM
    {
        public int CommentID { get; set; }
        public string NewsArticleID { get; set; } = string.Empty;
        public string CommentBy { get; set; } = string.Empty;

        public string CommentText { get; set; } = string.Empty;
        public DateTime CommentDate { get; set; }
    }
}
