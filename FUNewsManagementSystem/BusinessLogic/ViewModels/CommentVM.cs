namespace BusinessLogic.ViewModels
{
    public class CommentVM
    {
        public int CommentID { get; set; }
        public int NewsArticleID { get; set; }
        public string? CommentBy { get; set; }
        public string? CommentText { get; set; }
        public DateTime CommentDate { get; set; }
    }
}
