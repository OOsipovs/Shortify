using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortify.Client.Data.ViewModels;
using Shortify.Client.Helpers.Roles;
using Shortify.Data;
using Shortify.Data.Services;
using System.Security.Claims;

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
            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole(Role.Admin);

            var allUrlsFromDb = await this.urlsService.GetUrlsAsync(loggedInUserId, isAdmin);
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
