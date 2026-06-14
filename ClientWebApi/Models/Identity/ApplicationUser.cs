using Microsoft.AspNetCore.Identity;

namespace ClientWebApi.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public bool isPasswordSet { get; set; } = false;
    }
}
