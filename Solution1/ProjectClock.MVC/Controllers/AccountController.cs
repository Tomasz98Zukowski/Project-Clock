using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectClock.BusinessLogic.Dtos.AccountDtos;
using System.Security.Claims;
using ProjectClock.MVC.Extensions;
using ProjectClock.BusinessLogic.Services.AccountServices;
using ProjectClock.BusinessLogic.Dtos.Account.Dtos;
using System.Drawing;
using Microsoft.Extensions.Localization;


namespace ProjectClock.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountServices _accountService;
        private readonly IStringLocalizer<AccountController> _localizer;

        public AccountController(IAccountServices accountService, IMapper mapper, IStringLocalizer<AccountController> localizer)
        {
            _accountService = accountService;
            _localizer = localizer;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            var dto = new LoginDto() { LoginFailed = false, UserIsActive = true, UserExist = true };

            return View(dto);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var resultDto = await _accountService.LoginAccount(dto);
            if (!resultDto.UserExist)
            {
                TempData["UserNotExist"] = _localizer["UserNotExist"].Value;
                return View(dto);
            }

            if (resultDto.LoginFailed && !resultDto.AccountActive)
            {
                return RedirectToAction("Active", new { email = dto.Email });
            }
            if (resultDto.LoginFailed && resultDto.AccountActive)
            {
                dto.LoginFailed = true;                
                return View(dto);
            }
            

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(resultDto.ClaimsIdentity),
                resultDto.AuthProp);

            return RedirectToAction("index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            var dto = new RegisterDto() 
            { 
                Results = new RegisterResultDto()
                {
                    EmailAlreadyInUse = false,
                    RegistrationFailed = false,
                }
            };

            return View(dto);
        }

        [AllowAnonymous]
        public IActionResult Active(string email)
        {
            var dto = new ActiveAccountDto()
            {
                Email = email,
            };

            return View(dto);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Active(ActiveAccountDto dto)
        {
            if (await _accountService.ChangeUserStatus(dto))
            {
                TempData["SuccessMessage"] = _localizer["AccActived"].Value;
            }
            else
            {
                TempData["ErrorMessage"] = _localizer["AccWrong"].Value;
                return RedirectToAction("Active", new { email = dto.Email });
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }


            var resultDto = await _accountService.RegisterAccount(dto);
            dto.Results = resultDto;

            if (resultDto.RegistrationFailed)
            {             
                return View(dto);
            }

            return RedirectToAction("Login");
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login", "Account");
        }

        [Authorize(Roles = "User")]
        public  IActionResult EditEmail()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> EditEmail(EditEmailDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var userId))
            {
                return RedirectToAction("Index", "Home");
            }

            dto.Id = userId;

            var resultDto = await _accountService.EditAccountEmail(dto);

            if (resultDto.EditEmailFailed)
            {
                dto.Result = resultDto;
                return View(dto);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "User")]
        public ActionResult EditPassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> EditPassword(EditPasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var userId))
            {
                return RedirectToAction("Index", "Home");
            }

            dto.UserId = userId;

            var resultDto = await _accountService.EditAccountPassword(dto);

            if (resultDto.EditPasswordFailed)
            {
                dto.Result = resultDto;
                return View(dto);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "User")]
        public IActionResult Delete()
        {      
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Delete(DeleteAccountDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            if (!HttpContext.User.Claims.TryGetAuthenticatedUserId(out var userId))
            {
                return RedirectToAction("Index", "Home");
            }

            dto.Id = userId;

            var deletionSuccessful = await _accountService.DeleteAccount(dto);

            if (deletionSuccessful)
            {
                return RedirectToAction("Logout");
            }

            dto.DeleteAccountFailed = true;
            return View(dto);
        }
    }
}
