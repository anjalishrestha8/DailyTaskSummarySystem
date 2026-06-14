using ClientMVC.ApiResponse;
using ClientMVC.Dto.RequestDto;
using ClientMVC.Models;
using ClientMVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Security.Claims;

namespace ClientMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthApiService authApiService;
        private readonly IUserService _userService;
        public AccountController(IAuthApiService authApiService, IUserService userService)
        {
            this.authApiService = authApiService;
            _userService = userService;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var registerDto = new RegisterRequestDto
                {
                    UserName = registerViewModel.UserName,
                    FullName = registerViewModel.FullName,
                    Email = registerViewModel.Email,
                    Password = registerViewModel.Password,
                    DateOfBirth = registerViewModel.DateOfBirth,
                };
                var response = await authApiService.RegisterAsync(registerDto);

                return Json(response);
            }
            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var loginDto = new LoginRequestDto
                    {
                        UserNameOrEmail = loginViewModel.UserNameOrEmail,
                        Password = loginViewModel.Password
                    };

                    var response = await authApiService.LoginAsync(loginDto);
                    if (response.Success && response.Data != null)
                    {
                        var token = response.Data.Token;
                        var claims = new List<Claim>
                    {
                        new Claim("UserId",response.Data.UserId),
                        new Claim(ClaimTypes.Name, response.Data.UserName),
                        new Claim(ClaimTypes.Email, response.Data.Email),
                    };
                        foreach (var role in response.Data.Roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(claimsIdentity);

                        await HttpContext.SignInAsync("CookieAuth", principal,
                          new AuthenticationProperties
                          {
                              IsPersistent = true,
                              ExpiresUtc = DateTime.UtcNow.AddHours(1)
                          });

                        if (!string.IsNullOrEmpty(token))
                        {
                            Response.Cookies.Append("AuthToken", token, new CookieOptions
                            {
                                HttpOnly = true,
                                Secure = false,
                                SameSite = SameSiteMode.Lax,
                                Expires = DateTimeOffset.UtcNow.AddHours(1)
                            });
                        }
                        return Json(response);
                        //TempData["SuccessMessage"] = "Login successful!";
                        //return RedirectToAction("Index", "Clients");
                    }
                    return Json(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Invalid Credentials.",
                    });
                    //ModelState.AddModelError(string.Empty, response.Message);

                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred during login.", ex);
                }
            }
            return View(loginViewModel);
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync("CookieAuth");
            Response.Cookies.Delete("AuthToken");

            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
                return NotFound();

            var response = await _userService.GetUsersByIdAsync(userId);
            if (response == null || !response.Success || response.Data == null)
            {
                return NotFound();
            }
            var user = response.Data;
            if(user.isPasswordSet)
            {
                return View("PasswordAlreadySet");
            }
            var model = new SetPasswordViewModel
            {
                UserId = userId,
                Email = user.Email,
                Token = token
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid form submission." });
            }

            var dto = new SetPasswordRequestDto
            {
                UserId = model.UserId,
                Email = model.Email,
                Token = model.Token,
                NewPassword = model.NewPassword
            };

            var response = await authApiService.SetPasswordAsync(dto);

            if (response != null && response.Success)
            {
                return Json(new { success = true, message = response.Message });
            }

            return Json(new { success = false, message = response?.Message ?? "Failed to set password." });
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid email submission." });
            }

            var dto = new ForgotPasswordReqDto
            {
                Email = model.Email
            };
            var response = await authApiService.ForgotPasswordAsync(dto);

            if (response != null && response.Success)
            {
                return Json(new { success = true, message = response.Message });
            }

            return Json(new { success = false, message = response?.Message ?? "Failed to submit email." });
        }
    }
}
