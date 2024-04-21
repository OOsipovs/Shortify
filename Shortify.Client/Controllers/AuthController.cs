using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortify.Client.Data.ViewModels;
using Shortify.Data;

namespace Shortify.Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext appDbContext;
        public AuthController(AppDbContext dbContext)
        {
            appDbContext = dbContext;
        }
        public IActionResult Users()
        {
            var users = appDbContext.Users.Include(u => u.Urls).ToList();

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
