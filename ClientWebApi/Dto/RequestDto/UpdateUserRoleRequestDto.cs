namespace ClientWebApi.Dto.RequestDto
{
    public class UpdateUserRoleRequestDto
    {
        public string UserId { get; set; } = null!;
        public string RoleName { get; set; } = null!;

    }
}
