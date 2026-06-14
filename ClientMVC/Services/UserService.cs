using ClientMVC.ApiResponse;
using ClientMVC.Dto.RequestDto;
using ClientMVC.Dto.ResponseDto;

namespace ClientMVC.Services
{
    public class UserService : IUserService
    {
        private readonly IApiService apiService;
        private readonly IAuthApiService _authApiService;

        public UserService(IApiService apiService, IHttpContextAccessor httpContextAccessor, IAuthApiService authApiService)
        {
            this.apiService = apiService;
            _authApiService = authApiService;
        }

        public async Task<ApiResponse<string>> AdminRegisterUserAsync(AdminRegisterUserReqDto adminRegisterUserReqDto)
        {
            string generatedPassword = $"{adminRegisterUserReqDto.UserName}@123";
            var registerRequest = new RegisterRequestDto
            {
                UserName = adminRegisterUserReqDto.UserName,
                FullName = adminRegisterUserReqDto.FullName,
                Email = adminRegisterUserReqDto.Email,
                Password = generatedPassword,
                DateOfBirth = adminRegisterUserReqDto.DateOfBirth,
                RoleName = adminRegisterUserReqDto.RoleName
            };
            var registerResponse = await _authApiService.RegisterAsync(registerRequest);
            if (registerResponse == null || !registerResponse.Success)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = registerResponse?.Message ?? "Failed to register user",
                    Data = null
                };
            }
            //var roleUpdateResponse = await UpdateUserRoleAsync(new UpdateUserRoleRequestDto
            //{
            //    UserId = registerResponse.Data.UserId,
            //    RoleName = adminRegisterUserReqDto.RoleName
            //});

            //if (roleUpdateResponse == null || !roleUpdateResponse.Success)
            //{
            //    return new ApiResponse<string>
            //    {
            //        Success = false,
            //        Message = roleUpdateResponse?.Message ?? "User created, but failed to assign role",
            //        Data = null
            //    };
            //}

            return new ApiResponse<string>
            {
                Success = true,
                Message = $"User registered successfully and email sent.",
                Data =null
            };
        }

        public async Task<ApiResponse<IEnumerable<AuthResponseDto>>> GetAllUsersAsync()
        {
            var response = await apiService.GetAsync<ApiResponse<IEnumerable<AuthResponseDto>>>("Users/GetAllUsers");
            if (response != null && response.Success)
            {
                return response;
            }
            return new ApiResponse<IEnumerable<AuthResponseDto>>
            {
                Success = false,
                Message = response?.Message ?? "Failed to retrieve users",
                Data = null
            };
        }
        public async Task<ApiResponse<AuthResponseDto>> GetUsersByIdAsync(string userId)
        {
            var response = await apiService.GetAsync<ApiResponse<AuthResponseDto>>($"Users/GetUserById?userId={userId}");
            if (response != null && response.Success)
            {
                return response;
            }
            return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = response?.Message ?? "Failed to retrieve user by id ",
                Data = null
            };
        }

        public async Task<ApiResponse<string>> UpdateUserRoleAsync(UpdateUserRoleRequestDto updateUserDto)
        {
            var response = await apiService.PutAsync<UpdateUserRoleRequestDto, ApiResponse<string>>("Users/UpdateUserRole", updateUserDto);
            if (response != null && response.Success)
            {
                return response;
            }
            return new ApiResponse<string>
            {
                Success = false,
                Message = response?.Message ?? "Failed to update user's role",
                Data = null
            };
        }

        public async Task<ApiResponse<string>> DeleteUserAsync(string userId)
        {
            var response = await apiService.DeleteAsync<ApiResponse<string>>($"Users/DeleteUser/userId={userId}");
            if (response != null && response.Success)
            {
                return response;
            }
            return new ApiResponse<string>
            {
                Success = false,
                Message = response?.Message ?? "Failed to delete user",
                Data = null
            };
        }

        public async Task<ApiResponse<IEnumerable<string>>> GetAllRolesAsync()
        {
            var response = await apiService.GetAsync<ApiResponse<IEnumerable<string>>>("Users/GetAllRoles");
            if (response != null && response.Success)
                return response;

            return new ApiResponse<IEnumerable<string>>
            {
                Success = false,
                Message = response?.Message ?? "Failed to retrieve roles",
                Data = new List<string>()
            };
        }
    }
}

