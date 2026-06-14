using ClientMVC.ApiResponse;
using ClientMVC.Dto.RequestDto;
using ClientMVC.Dto.ResponseDto;

namespace ClientMVC.Services
{
    public class AuthApiService : IAuthApiService
    {
        private readonly IApiService apiService;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthApiService(IApiService apiService, IHttpContextAccessor httpContextAccessor)
        {
            this.apiService = apiService;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto registerDto)
        {
            var response = await apiService.PostAsync<RegisterRequestDto, ApiResponse<AuthResponseDto>>("Auth/Register", registerDto);
            if (response != null && response.Success)
            {
                return response;
            }
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = response?.Message ?? "Registration failed",
                Data = null
            };
        }
        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto loginDto)
        {
            var response = await apiService.PostAsync<LoginRequestDto, ApiResponse<AuthResponseDto>>("Auth/Login", loginDto);
            if (response == null)
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = response?.Message ?? "Login failed",
                    Data = null
                };
            }
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = false, 
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddHours(1)
            };
            httpContextAccessor.HttpContext?.Response.Cookies.Append("AuthToken", response!.Data!.Token!, cookieOptions);
            return response;
        }

        public async Task<ApiResponse<string>> SetPasswordAsync(SetPasswordRequestDto dto)
        {
            var response = await apiService.PostAsync<SetPasswordRequestDto, ApiResponse<string>>("Auth/SetPassword", dto);
            if (response != null && response.Success)
            {
                return response;
            }
            return new ApiResponse<string>
            {
                Success = false,
                Message = response?.Message ?? "Failed to set password",
                Data = null
            };
        }
        public async Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordReqDto dto)
        {
            var response = await apiService.PostAsync<ForgotPasswordReqDto, ApiResponse<string>>("Auth/ForgotPassword", dto);
            if (response != null && response.Success)
            {
                return response;
            }
            return new ApiResponse<string>
            {
                Success = false,
                Message = response?.Message ?? "Failed to set password",
                Data = null
            };
        }
    }
}

