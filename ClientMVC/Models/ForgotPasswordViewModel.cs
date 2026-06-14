using System.ComponentModel.DataAnnotations;

namespace ClientMVC.Models
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        public string Email { get; set; } = null!;
    }
}
