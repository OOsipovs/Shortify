using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortify.Client.Data.ViewModels;
using Shortify.Data;

namespace Shortify.Client.Controllers
{
    public class UrlController : Controller
    {
        private readonly AppDbContext dbContext;

        public UrlController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IActionResult Index()
        {
            var allUrlsFromDb = this.dbContext.Urls.Include(n => n.User).Select(u => new GetUrlVM
            {
                Id = u.Id,
                OriginalLink = u.OriginalLink,
                ShortLink = u.ShortLink,
                NrOfClicks = u.NrOfClicks,
                UserId = u.UserId,
                User = u.User != null ? new GetUserVM
                {
                    Id = u.Id,
                    FullName = u.User.FullName
                } : new GetUserVM()
            }).ToList();


            return View(allUrlsFromDb);
        }

        public IActionResult Create()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var url = dbContext.Urls.FirstOrDefault(n => n.Id == id);
            dbContext.Urls.Remove(url);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
