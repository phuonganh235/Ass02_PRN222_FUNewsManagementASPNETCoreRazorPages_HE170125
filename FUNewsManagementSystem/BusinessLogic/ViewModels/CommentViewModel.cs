using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.ViewModels
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }

        [Required]
        public string NewsArticleId { get; set; } = string.Empty;

        public short AccountId { get; set; }

        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Comment content is required")]
        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters")]
        public string Content { get; set; } = string.Empty;

        // Alias properties for compatibility
        public string CommentText
        {
            get => Content;
            set => Content = value;
        }

        public DateTime CreatedAt { get; set; }

        // Alias property for compatibility
        public DateTime CommentDate
        {
            get => CreatedAt;
            set => CreatedAt = value;
        }

        public string TimeAgo
        {
            get
            {
                var timeSpan = DateTime.Now - CreatedAt;
                if (timeSpan.TotalMinutes < 1) return "Just now";
                if (timeSpan.TotalMinutes < 60) return $"{(int)timeSpan.TotalMinutes} minutes ago";
                if (timeSpan.TotalHours < 24) return $"{(int)timeSpan.TotalHours} hours ago";
                if (timeSpan.TotalDays < 30) return $"{(int)timeSpan.TotalDays} days ago";
                return CreatedAt.ToString("dd/MM/yyyy HH:mm");
            }
        }
    }
}