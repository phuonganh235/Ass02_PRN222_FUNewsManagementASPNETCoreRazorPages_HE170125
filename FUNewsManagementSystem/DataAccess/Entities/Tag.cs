using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Entities
{
    [Table("Tag")]
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }

        [Required]
        [StringLength(50)]
        public string TagName { get; set; } = string.Empty;

        [StringLength(400)]
        public string? Note { get; set; }

        // Navigation property
        public virtual ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}