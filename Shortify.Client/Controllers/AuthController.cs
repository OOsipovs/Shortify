using Microsoft.AspNetCore.Mvc;

namespace Shortify.Client.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Users()
        {
            return View();
        }
    }
}
