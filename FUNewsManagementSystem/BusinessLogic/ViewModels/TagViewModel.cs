using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.ViewModels
{
    public class TagViewModel
    {
        public int TagId { get; set; }

        [Required(ErrorMessage = "Tag name is required")]
        [StringLength(50, ErrorMessage = "Tag name cannot exceed 50 characters")]
        public string TagName { get; set; } = string.Empty;

        [StringLength(400, ErrorMessage = "Note cannot exceed 400 characters")]
        public string? Note { get; set; }
    }
}