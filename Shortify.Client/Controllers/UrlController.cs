using Microsoft.AspNetCore.Mvc;

namespace Shortify.Client.Controllers
{
    public class UrlController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
