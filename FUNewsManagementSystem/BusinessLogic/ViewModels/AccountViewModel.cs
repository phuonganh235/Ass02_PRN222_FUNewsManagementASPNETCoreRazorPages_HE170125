using System.ComponentModel.DataAnnotations;

namespace BusinessLogic.ViewModels
{
    public class AccountViewModel
    {
        public short AccountId { get; set; }

        [Required(ErrorMessage = "Account name is required")]
        [StringLength(70, ErrorMessage = "Account name cannot exceed 70 characters")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(70, ErrorMessage = "Email cannot exceed 70 characters")]
        public string AccountEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role is required")]
        public int AccountRole { get; set; }

        [StringLength(70, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 70 characters")]
        [DataType(DataType.Password)]
        public string? AccountPassword { get; set; }

        [Compare("AccountPassword", ErrorMessage = "Password and confirmation do not match")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        public string RoleName => AccountRole switch
        {
            0 => "Admin",
            1 => "Staff",
            2 => "Lecturer",
            _ => "Unknown"
        };
    }
}