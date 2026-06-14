using ClientMVC.ApiResponse;
using ClientMVC.Dto.RequestDto;
using ClientMVC.Models;
using ClientMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClientMVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        private string GetUserId()
        {
            var userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
                throw new Exception("UserId claim not found in token");
            return userId;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var response = await _userService.GetAllUsersAsync();
            var userViewModels = response.Data?.Select(u => new UserViewModel
            {
                UserId = u.UserId,
                UserName = u.UserName,
                FullName = u.FullName,
                Email = u.Email,
                Roles = u.Roles
            }).ToList() ?? new List<UserViewModel>();

            return View(userViewModels);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AdminRegisterUserPartial()
        {
            var rolesResponse = await _userService.GetAllRolesAsync();
            var roleList = rolesResponse.Success ? rolesResponse.Data : new List<string>();

            var model = new AdminRegisterUserReqDto
            {
                Roles = roleList!
            };

            return PartialView("_AdminRegisterUserPartial", model);

        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(AdminRegisterUserReqDto adminRegisterUserReqDto)
        {
            if (!ModelState.IsValid)
            {
                return Json(new ApiResponse<string>
                {
                    Success = false,
                    Message = "Invalid register request data."
                });
            }
            var response = await _userService.AdminRegisterUserAsync(adminRegisterUserReqDto);
            if (response.Success)
            {
                return Json(response);
            }
            return Json(new ApiResponse<string>
            {
                Success = false,
                Message = response.Message,
            });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> UpdateUserRolePartial(string id)
        {
            var response = await _userService.GetAllUsersAsync();
            if (!response.Success || response.Data == null)
            {
                return NotFound();
            }
            var user = response.Data.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            var rolesResponse = await _userService.GetAllRolesAsync();
            var roleList = rolesResponse.Success ? rolesResponse.Data : new List<string>();

            var updateUserViewModel = new UserViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Roles = roleList!,
                RoleName = user.Roles.FirstOrDefault() ?? "User"

            };
            return PartialView("_UpdateUserRolePartial", updateUserViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(UpdateUserRoleRequestDto updateUserRequestDto)
        {
            var updateUserDto = new UpdateUserRoleRequestDto
            {
                UserId = updateUserRequestDto.UserId,
                RoleName = updateUserRequestDto.RoleName
            };
            var response = await _userService.UpdateUserRoleAsync(updateUserDto);
            if (response.Success)
            {
                return Json(response);
            }
            return Json(new ApiResponse<string>
            {
                Success = false,
                Message = response.Message,
            });
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Profile()
        {
            var userId = GetUserId();
            var response = await _userService.GetUsersByIdAsync(userId);
            if (!response.Success || response.Data == null)
            {
                return NotFound(response.Message);
            }
            var viewModel = new UserProfileViewModel
            {
                UserId = userId,
                UserName = response.Data.UserName,
                FullName = response.Data.FullName,
                Email = response.Data.Email,
                DateOfBirth = response.Data.DateOfBirth,
                Roles = response.Data.Roles,
            };
            return View(viewModel);
        }
    }
}
