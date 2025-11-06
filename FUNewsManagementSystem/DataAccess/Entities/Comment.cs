using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }

        [Required]
        [StringLength(20)]
        public string NewsArticleId { get; set; } = string.Empty;

        [Required]
        public short AccountId { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("NewsArticleId")]
        public virtual NewsArticle NewsArticle { get; set; } = null!;

        [ForeignKey("AccountId")]
        public virtual SystemAccount Account { get; set; } = null!;
    }
}