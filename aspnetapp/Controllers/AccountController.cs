using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using aspnetapp.Models;
using HighRiskDiseaseSurvellance.Aplication.Services;
using HighRiskDiseaseSurvellance.Dto.Models;
using HighRiskDiseaseSurvellance.Dto.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspnetapp.Controllers
{
    public class AccountController : BaseController
    {
        private readonly OfficeUserService _officeUserService;

        public AccountController(OfficeUserService officeUserService)
        {
            _officeUserService = officeUserService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var signResult = await _officeUserService.PasswordSignInAsync(new PasswordSignInRequest
                                                                            {
                                                                                UserName = model.UserName,
                                                                                Password = model.Password,
                                                                            });
            await SignInAsync(signResult);
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInAsync(PasswordSignIn result)
        {
            var claims = new List<Claim>
                         {
                             new Claim(ClaimTypes.NameIdentifier, result.Id),
                             new Claim(ClaimTypes.Name, result.Name),
                             new Claim(ClaimTypes.Role, "Administrator"),
                         };

            var authProperties = new AuthenticationProperties
                                 {
                                     ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                                 };


            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                          new ClaimsPrincipal(claimsIdentity),
                                          authProperties);
        }
    }
}
