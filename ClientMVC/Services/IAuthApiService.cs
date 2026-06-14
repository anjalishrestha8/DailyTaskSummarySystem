using ClientMVC.ApiResponse;
using ClientMVC.Dto.RequestDto;
using ClientMVC.Dto.ResponseDto;

namespace ClientMVC.Services
{
    public interface IAuthApiService
    {
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto registerDto);
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto loginDto);
        Task<ApiResponse<string>> SetPasswordAsync(SetPasswordRequestDto dto);
        Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordReqDto dto);
    }
}
