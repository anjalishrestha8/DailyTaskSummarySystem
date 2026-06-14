using ClientWebApi.ApiResponse;
using ClientWebApi.Dto.RequestDto;
using ClientWebApi.Dto.ResponseDto;

namespace ClientWebApi.Services
{
    public interface IUserService
    {
       
        Task<ApiResponse<IEnumerable<AuthResponseDto>>> GetAllAsync();
        Task<ApiResponse<AuthResponseDto>> GetByIdAsync(string userId);
        Task<ApiResponse<string>> UpdateUserRoleAsync(UpdateUserRoleRequestDto updateUserDto);
        Task<ApiResponse<string>> DeleteUserAsync(string userId);
        Task<ApiResponse<IEnumerable<string>>> GetAllRolesAsync();


    }
}
