using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("SystemAccount")]
    public class SystemAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short AccountId { get; set; }

        [Required]
        [StringLength(70)]
        public string AccountName { get; set; } = string.Empty;

        [Required]
        [StringLength(70)]
        [EmailAddress]
        public string AccountEmail { get; set; } = string.Empty;

        [Required]
        public int AccountRole { get; set; } // 0=Admin, 1=Staff, 2=Lecturer

        [Required]
        [StringLength(70)]
        public string AccountPassword { get; set; } = string.Empty;

        // Navigation property
        public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
        public virtual ICollection<NewsArticle> UpdatedNewsArticles { get; set; } = new List<NewsArticle>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}