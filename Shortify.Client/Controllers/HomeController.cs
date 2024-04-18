using Microsoft.AspNetCore.Mvc;
using Shortify.Client.Data.ViewModels;
using System.Diagnostics;

namespace Shortify.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var newUrl = new PostUrlVM();
            return View(newUrl);
        }

        public IActionResult ShortenUrl(PostUrlVM postUrlVm)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", postUrlVm);
            }

            return RedirectToAction("Index"); 
        }

    }
}