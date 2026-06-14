namespace ClientWebApi.Dto.ResponseDto
{
    public class AuthResponseDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;  
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IEnumerable<string> Roles { get; set; } = new List<string>();

        public DateTime DateOfBirth { get; set; }

        public string? Token { get; set; }
        public bool isPasswordSet { get; set; }
    }
}
