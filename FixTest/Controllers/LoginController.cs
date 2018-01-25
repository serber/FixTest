using System.Security.Claims;
using System.Threading.Tasks;
using FixTest.Entities;
using FixTest.Models;
using FixTest.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace FixTest.Controllers
{
    [Route("account")]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("login")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = await _userService.Get(model.Login, model.Password);
            if (user == null)
            {
                return View(model);
            }

            ClaimsIdentity id = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            id.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            id.AddClaim(new Claim(ClaimTypes.Name, user.Login));

            ClaimsPrincipal principal = new ClaimsPrincipal(id);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = model.RememberMe });

            return RedirectToAction("Index", "Home");
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
    }
}