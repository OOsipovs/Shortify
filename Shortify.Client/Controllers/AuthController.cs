using Microsoft.AspNetCore.Mvc;
using Shortify.Client.Data.ViewModels;

namespace Shortify.Client.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Users()
        {
            return View();
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
