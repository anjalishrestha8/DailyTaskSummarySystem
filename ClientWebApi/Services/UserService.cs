using ClientWebApi.ApiResponse;
using ClientWebApi.Dto.RequestDto;
using ClientWebApi.Dto.ResponseDto;
using ClientWebApi.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace ClientWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        public async Task<ApiResponse<IEnumerable<AuthResponseDto>>> GetAllAsync()
        {
            var users = userManager.Users.ToList();
            var userDtos = new List<AuthResponseDto>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                if (!roles.Contains("Admin"))
                {
                    userDtos.Add(new AuthResponseDto
                    {
                        UserId = user.Id,
                        UserName = user.UserName ?? "",
                        FullName = user.FullName ?? "",
                        Email = user.Email ?? "",
                        Roles = roles,
                        Token = null
                    });
                }
            }
            return new ApiResponse < IEnumerable < AuthResponseDto >>
            {
                Success = true,
                Message = "Users retrieved successfully",
                Data = userDtos
            };
        }

        public async Task<ApiResponse<AuthResponseDto>> GetByIdAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "User not found",
                    Data = null
                };
            }
            var roles = await userManager.GetRolesAsync(user);
            var userDto = new AuthResponseDto
            {
                UserId = user.Id,
                UserName = user.UserName ?? "",
                FullName = user.FullName ?? "",
                Email = user.Email ?? "",
                Roles = roles,
                DateOfBirth = user.DateOfBirth,
                Token = null,
                isPasswordSet = user.isPasswordSet
            };
            return new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Message = "User retrieved successfully",
                Data = userDto
            };
        }

        public async Task<ApiResponse<string>> UpdateUserRoleAsync(UpdateUserRoleRequestDto updateUserDto)
        {
            var user = await userManager.FindByIdAsync(updateUserDto.UserId);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "User not found",
                    Data = null
                };
            }

            if (!await roleManager.RoleExistsAsync(updateUserDto.RoleName))
            {
                await roleManager.CreateAsync(new IdentityRole(updateUserDto.RoleName));
            }

            var currentRoles = await userManager.GetRolesAsync(user);
            var removeResult = await userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to remove user roles",
                    Data = null
                };
            }
            var addRoleResult = await userManager.AddToRoleAsync(user, updateUserDto.RoleName);
            if (!addRoleResult.Succeeded)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Failed to add user role",
                    Data = null
                };
            }
            return new ApiResponse<string>
            {
                Success = true,
                Message = "User Role updated successfully",
                Data = null
            };
        }
        public async Task<ApiResponse<string>> DeleteUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "User not found",
                    Data = null
                };
            }
            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "User deletion failed",
                    Data = null
                };
            }
            return new ApiResponse<string>
            {
                Success = true,
                Message = "User deleted successfully",
                Data = null
            };
        }

        public async Task<ApiResponse<IEnumerable<string>>> GetAllRolesAsync()
        {
            var roles = await roleManager.Roles.Select(r => r.Name).ToListAsync();
            return new ApiResponse<IEnumerable<string>>
            {
                Success = true,
                Message = "Roles retrieved successfully",
                Data = roles!
            };
        }

        

    }
}
