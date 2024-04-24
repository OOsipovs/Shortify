using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortify.Client.Data.ViewModels;
using Shortify.Data;
using Shortify.Data.Services;

namespace Shortify.Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUsersService  usersService;
        public AuthController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public async Task<IActionResult> Users()
        {
            var users = await usersService.GetUsersAsync();

            return View(users);
        }

        public async Task<IActionResult> Login()
        {
            return View(new LoginVM());
        }

        public async Task<IActionResult> Register()
        {
            return View(new RegisterVM());
        }

        public async Task<IActionResult> LoginSubmission(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", loginVM);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", registerVM);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
