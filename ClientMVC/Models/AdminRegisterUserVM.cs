namespace ClientMVC.Models
{
    public class AdminRegisterUserVM
    {
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public string RoleName { get; set; } = null!;
    }
}
