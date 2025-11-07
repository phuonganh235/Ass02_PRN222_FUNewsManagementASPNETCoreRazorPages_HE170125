// DataAccess/Entities/Category.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Entities;

[Table("Category")]
public class Category
{
    [Key]
    public short CategoryID { get; set; } 

    [Required]
    [StringLength(100)]
    public string CategoryName { get; set; } = string.Empty;

    [StringLength(250)]
    public string? CategoryDescription { get; set; }

    public short? ParentCategoryID { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    public int SortOrder { get; set; } = 0;

    // Navigation properties...
    public virtual ICollection<Category> ChildCategories { get; set; } = new List<Category>();
    public virtual Category? ParentCategory { get; set; }
    public virtual ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}