using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shortify.Client.Data.ViewModels;
using Shortify.Data;
using Shortify.Data.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace Shortify.Client.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext appDbContext;

        public HomeController(ILogger<HomeController> logger, AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
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

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var newUrl = new Url()
            {
                OriginalLink = postUrlVm.Url,
                ShortLink = GenerateShortUrl(5),
                NrOfClicks = 0,
                UserId = loggedInUserId,
                DateCreated = DateTime.UtcNow,
            };

            appDbContext.Urls.Add(newUrl);
            appDbContext.SaveChanges();

            TempData["Message"] = $"Your Url was shortened to {newUrl.ShortLink}";

            return RedirectToAction("Index"); 
        }

        private string GenerateShortUrl(int length)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            return new string(
                Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray()
                );
        }

    }
}