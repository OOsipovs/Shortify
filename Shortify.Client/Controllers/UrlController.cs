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

        public async Task<IActionResult> Index()
        {
            var allUrlsFromDb = await this.urlsService.GetUrlsAsync();
            var mappedAllUrls = mapper.Map<List<GetUrlVM>>(allUrlsFromDb);

            return View(mappedAllUrls);
        }

        public async Task<IActionResult> Create()
        {
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int id)
        {
            await urlsService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

    }
}
