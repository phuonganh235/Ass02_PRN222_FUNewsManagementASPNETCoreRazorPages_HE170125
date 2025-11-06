using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("NewsArticle")]
    public class NewsArticle
    {
        [Key]
        [StringLength(20)]
        public string NewsArticleId { get; set; } = string.Empty;

        [Required]
        [StringLength(400)]
        public string NewsTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string Headline { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(4000)]
        public string NewsContent { get; set; } = string.Empty;

        [StringLength(400)]
        public string? NewsSource { get; set; }

        [Required]
        public short CategoryId { get; set; }

        [Required]
        public bool NewsStatus { get; set; } = true; // 1=Active, 0=Inactive

        [Required]
        public short CreatedById { get; set; }

        public short? UpdatedById { get; set; }

        public DateTime? ModifiedDate { get; set; }

        // Navigation properties
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;

        [ForeignKey("CreatedById")]
        public virtual SystemAccount CreatedBy { get; set; } = null!;

        [ForeignKey("UpdatedById")]
        public virtual SystemAccount? UpdatedBy { get; set; }

        public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}