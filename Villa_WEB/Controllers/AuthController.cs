using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Login(LoginRequestDTO obj)
        {
            return View(obj);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDTO obj)
        {
            APIResponse result = await _authService.RegisterAsync<APIResponse>(obj);
            if (result !=null && result.IsSuccess==true)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Logout() 
        {
            return View() ;
        }

        [HttpGet]
        public IActionResult AccessDenied() 
        {
            return View();
        }
    }
}
