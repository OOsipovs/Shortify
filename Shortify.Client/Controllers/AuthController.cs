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
        public IActionResult Users()
        {
            var users = usersService.GetUsers();

            return View(users);
        }

        public IActionResult Login()
        {
            return View(new LoginVM());
        }

        public IActionResult Register()
        {
            return View(new RegisterVM());
        }

        public IActionResult LoginSubmission(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", loginVM);
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", registerVM);
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
