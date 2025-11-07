using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.ViewModels
{
    public class CategoryViewModel
    {
        public short CategoryId { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
        public string CategoryName { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
        public string? CategoryDesciption { get; set; }

        public short? ParentCategoryId { get; set; }

        public string? ParentCategoryName { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public int SortOrder { get; set; } = 0;

        public string StatusText => IsActive ? "Active" : "Inactive";
    }
}