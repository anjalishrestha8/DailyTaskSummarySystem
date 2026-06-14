using ClientMVC.ApiResponse;
using ClientMVC.Dto.RequestDto;
using ClientMVC.Dto.ResponseDto;

namespace ClientMVC.Services
{
    public interface IUserService
    {
        Task<ApiResponse<string>> AdminRegisterUserAsync(AdminRegisterUserReqDto adminRegisterUserReqDto);
        Task<ApiResponse<IEnumerable<AuthResponseDto>>> GetAllUsersAsync();
        Task<ApiResponse<AuthResponseDto>> GetUsersByIdAsync(string userId);
        Task<ApiResponse<IEnumerable<string>>> GetAllRolesAsync();
        Task<ApiResponse<string>> UpdateUserRoleAsync(UpdateUserRoleRequestDto updateUserDto);
        Task<ApiResponse<string>> DeleteUserAsync(string userId);
    }
}
