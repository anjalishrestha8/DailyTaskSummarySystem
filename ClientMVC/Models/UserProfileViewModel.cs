namespace ClientMVC.Models
{
    public class UserProfileViewModel
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public DateTime DateOfBirth { get; set; }

    }
}
