using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.ViewModels
{
    public class NewsArticleViewModel
    {
        [Required(ErrorMessage = "News ID is required")]
        [StringLength(20, ErrorMessage = "News ID cannot exceed 20 characters")]
        public string NewsArticleId { get; set; } = string.Empty;

        // Alias property for compatibility
        public string NewsArticleID
        {
            get => NewsArticleId;
            set => NewsArticleId = value;
        }

        [Required(ErrorMessage = "News title is required")]
        [StringLength(400, ErrorMessage = "Title cannot exceed 400 characters")]
        public string NewsTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Headline is required")]
        [StringLength(150, ErrorMessage = "Headline cannot exceed 150 characters")]
        public string Headline { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        [StringLength(4000, ErrorMessage = "Content cannot exceed 4000 characters")]
        public string NewsContent { get; set; } = string.Empty;

        [StringLength(400, ErrorMessage = "News source cannot exceed 400 characters")]
        public string? NewsSource { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public short CategoryId { get; set; }

        public string? CategoryName { get; set; }

        [Required]
        public bool NewsStatus { get; set; } = true;

        public DateTime CreatedDate { get; set; }

        public short CreatedById { get; set; }

        public string? CreatedByName { get; set; }

        public short? UpdatedById { get; set; }

        public string? UpdatedByName { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public List<int> SelectedTagIds { get; set; } = new List<int>();

        public List<TagViewModel> Tags { get; set; } = new List<TagViewModel>();

        public List<CommentViewModel> Comments { get; set; } = new List<CommentViewModel>();

        public string StatusText => NewsStatus ? "Active" : "Inactive";
    }
}