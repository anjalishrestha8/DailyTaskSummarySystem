namespace ClientWebApi.Dto.RequestDto
{
    public class SetPasswordRequestDto
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}