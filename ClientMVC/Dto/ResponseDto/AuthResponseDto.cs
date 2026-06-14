namespace ClientMVC.Dto.ResponseDto
{
    public class AuthResponseDto
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public DateTime DateOfBirth { get; set; }

        public string? Token { get; set; }
        public bool isPasswordSet { get; set; }

    }
}
