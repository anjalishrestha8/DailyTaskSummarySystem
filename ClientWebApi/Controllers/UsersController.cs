using ClientWebApi.Dto.RequestDto;
using ClientWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var response = await userService.GetAllAsync();
            return Ok(response);
        }

        //[Authorize(Roles = "Admin,User")]
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById([FromQuery] string userId)
        {
            var response = await userService.GetByIdAsync(userId);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleRequestDto updateUserDto)
        {
            var response = await userService.UpdateUserRoleAsync(updateUserDto);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] string userId)
        {
            var response = await userService.DeleteUserAsync(userId);
            return Ok(response);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var response = await userService.GetAllRolesAsync();
            return Ok(response);
        }

    }
}