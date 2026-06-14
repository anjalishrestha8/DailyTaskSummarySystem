using System.ComponentModel.DataAnnotations;

namespace ClientMVC.Models
{
    public class SetPasswordViewModel
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = null!;
    }

}
