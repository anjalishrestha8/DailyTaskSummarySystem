using ClientWebApi.Dto.RequestDto;
using ClientWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            var response = await authService.RegisterAsync(registerRequestDto);
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var response = await authService.LoginAsync(loginRequestDto);
            if (!response.Success)
            {
                return Unauthorized(response);
            }
            return Ok(response);
        }

        [HttpPost("SetPassword")]
        public async Task<IActionResult> SetPassword(SetPasswordRequestDto setPasswordDto)
        {
            var response = await authService.SetPassword(setPasswordDto);
            return Ok(response);
        }
        [AllowAnonymous]
        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordReqDto forgetPasswordDto)
        {
            var response = await authService.ForgotPassword(forgetPasswordDto);
            return Ok(response);
        }
    }
}