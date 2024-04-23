using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortify.Client.Data.ViewModels;
using Shortify.Data;
using Shortify.Data.Services;

namespace Shortify.Client.Controllers
{
    public class UrlController : Controller
    {
        private readonly IUrlsService urlsService;
        private readonly IMapper mapper;

        public UrlController(IUrlsService urlsService, IMapper mapper)
        {
            this.urlsService = urlsService;
            this.mapper = mapper;
        }

        public IActionResult Index()
        {
            var allUrlsFromDb = this.urlsService.GetUrls();
            var mappedAllUrls = mapper.Map<List<GetUrlVM>>(allUrlsFromDb);

            return View(mappedAllUrls);
        }

        public IActionResult Create()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            urlsService.Delete(id);
            return RedirectToAction("Index");
        }

    }
}
