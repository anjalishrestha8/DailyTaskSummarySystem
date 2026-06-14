namespace ClientMVC.Dto.RequestDto
{
    public class RegisterRequestDto
    {
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string? RoleName { get; set; }
    }
}
