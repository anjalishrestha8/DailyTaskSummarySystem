using ClientWebApi.ApiResponse;
using ClientWebApi.Dto.RequestDto;
using ClientWebApi.Dto.ResponseDto;

namespace ClientWebApi.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ApiResponse<string>> SetPassword(SetPasswordRequestDto setPasswordDto);
        Task<ApiResponse<string>> ForgotPassword(ForgotPasswordReqDto forgotPasswordDto);
    }
}
