namespace ClientMVC.Models
{
    public class UserViewModel
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public string RoleName { get; set; } = null!;
    }
}
