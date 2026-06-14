using ClientWebApi.ApiResponse;
using ClientWebApi.Dto.RequestDto;
using ClientWebApi.Dto.ResponseDto;
using ClientWebApi.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ClientWebApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly IEmailService _emailService;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailService emailService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            _emailService = emailService;
        }

        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto registerRequestDto)
        {
            var existingUser = await userManager.FindByNameAsync(registerRequestDto.UserName);
            var existingEmail = await userManager.FindByEmailAsync(registerRequestDto.Email);
            if (existingUser != null)
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "Username already exists",
                    Data = null
                };
            }
            if (existingEmail != null)
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "Email already exists",
                    Data = null
                };
            }

            var user = new ApplicationUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.Email,
                FullName = registerRequestDto.FullName,
                DateOfBirth = registerRequestDto.DateOfBirth
            };

            var result = await userManager.CreateAsync(user, registerRequestDto.Password);
            if (!result.Succeeded)
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "User creation failed",
                    Data = null
                };
            }
            if (registerRequestDto.RoleName == null)
            {
                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await roleManager.CreateAsync(new IdentityRole("User"));
                }
                await userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                await userManager.AddToRoleAsync(user, registerRequestDto.RoleName);
                user.isPasswordSet = false;
                await userManager.UpdateAsync(user);

                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = System.Net.WebUtility.UrlEncode(token);
                //var setPasswordLink = $"{configuration["ClientMvc:BaseUrl"]}/Account/SetPassword?email={user.Email}&token={encodedToken}";
                var setPasswordLink = $"{configuration["ClientMvc:BaseUrl"]}/Account/SetPassword?userId={user.Id}&token={encodedToken}";

                await _emailService.SendEmailAsync(
                    user.Email,
                    "Set your password",
                    $"Hello {user.FullName},<br>Your account has been created by an admin.  Click the link below to set your password:<br>" +
                    $" <a href=\"{setPasswordLink}\">Set Your Password</a>"
                );
            }

            return new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Message = "User registered successfully. Please log in.",
                Data = null
            };
        }


        public async Task<ApiResponse<string>> SetPassword(SetPasswordRequestDto setPasswordDto)
        {
            var user = await userManager.FindByIdAsync(setPasswordDto.UserId);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid User."
                };
            }
            if(user.isPasswordSet)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Password already set. Please login."
                };
            }
            var result = await userManager.ResetPasswordAsync(user, setPasswordDto.Token, setPasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid or expired link."
                };
            }

            user.isPasswordSet = true;
            await userManager.UpdateAsync(user);
            return new ApiResponse<string>
            {
                Success = true,
                Message = "Password has been set successfully."
            };
        }

        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.UserNameOrEmail);
            if (user == null)
            {
                user = await userManager.FindByNameAsync(loginRequestDto.UserNameOrEmail);
                if (user == null)
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        Success = false,
                        Message = "Invalid Email or UserName",
                        Data = null
                    };
                }
            }
            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (!isPasswordValid)
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "Invalid Password",
                    Data = null
                };
            }

            var roles = await userManager.GetRolesAsync(user);

            var token = GenerateJwtToken(user, roles);

            return new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Message = "Login successful",
                Data = new AuthResponseDto
                {
                    UserId = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    FullName = user.FullName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Roles = roles,
                    Token = token
                }
            };
        }

        private string GenerateJwtToken(ApplicationUser user, IEnumerable<string> roles)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]
                ?? throw new InvalidOperationException("JWT SecretKey is missing in configuration.")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),

                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id),
                new Claim(ClaimTypes.Name, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["ValidIssuer"],
                audience: jwtSettings["ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public async Task<ApiResponse<string>> ForgotPassword(ForgotPasswordReqDto forgotPasswordDto)
        {
            var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
            {
                return new ApiResponse<string>
                {
                    Success= false,
                    Message = "Email Not Found."
                };
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = System.Net.WebUtility.UrlEncode(token);
            var setPasswordLink = $"{configuration["ClientMvc:BaseUrl"]}/Account/SetPassword?userId={user.Id}&token={encodedToken}";

            await _emailService.SendEmailAsync(
                user.Email!,
                "Set your password",
                $"Hello {user.FullName},<br>You forgot your password? Click the link below to reset your password:<br>" +
                $" <a href=\"{setPasswordLink}\">Set Your Password</a>"
            );
            return new ApiResponse<string>
            {
                Success = true,
                Message = "Email sent to reset password."
            };
        }
    }
}
