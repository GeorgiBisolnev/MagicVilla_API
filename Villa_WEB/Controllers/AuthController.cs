﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Villa_WEB.Data.Common;
using Villa_WEB.Models;
using Villa_WEB.Models.Dto;
using Villa_WEB.Services.IServices;

namespace Villa_WEB.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
                this._authService = authService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO obj = new LoginRequestDTO();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            APIResponse response = await _authService.LoginAsync<APIResponse>(obj);
            if (response!=null && response.IsSuccess==true)
            {
                LoginResponseDTO model = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(model.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(u=>u.Type=="role").Value));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString(SD.SessionToken, model.Token);
                return RedirectToAction("Index","Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
            }
            return View(obj);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var rolelist = new List<SelectListItem>()
            {
                new SelectListItem{ Text = SD.Admin, Value = SD.Admin},
                new SelectListItem{ Text = SD.Customer, Value = SD.Customer}
            };
            ViewBag.RoleList = rolelist;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDTO obj)
        {
            if(string.IsNullOrEmpty(obj.Role))
            {
                obj.Role = SD.Customer;
            }
            APIResponse result = await _authService.RegisterAsync<APIResponse>(obj);
            if (result !=null && result.IsSuccess==true)
            {
                return RedirectToAction("Login");
            }
            var rolelist = new List<SelectListItem>()
            {
                new SelectListItem{ Text = SD.Admin, Value = SD.Admin},
                new SelectListItem{ Text = SD.Customer, Value = SD.Customer}
            };
            ViewBag.RoleList = rolelist;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout() 
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken, "");
            return RedirectToAction("Login","Auth") ;
        }

        [HttpGet]
        public IActionResult AccessDenied() 
        {
            return View();
        }
    }
}
