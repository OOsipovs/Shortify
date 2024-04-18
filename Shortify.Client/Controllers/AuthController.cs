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
            return View();
        }

        public IActionResult LoginSubmission(LoginVM loginVM)
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
