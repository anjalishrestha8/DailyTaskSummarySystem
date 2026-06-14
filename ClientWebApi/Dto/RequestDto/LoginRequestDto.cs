namespace ClientWebApi.Dto.RequestDto
{
    public class LoginRequestDto
    {
        public string UserNameOrEmail { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
